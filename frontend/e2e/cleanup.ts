import { request } from '@playwright/test'

// Чистит БД от e2e-пользователей через API клиента (DELETE /account).
// Admin API удалять пользователей не умеет (только роли и отчёты),
// поэтому входим под самим e2e-юзером: пароли известны — это константы
// из happy-path.spec.ts. Юзер, упавший посреди теста, мог успеть
// сменить пароль — пробуем оба варианта.

const BASE = 'http://localhost:5173'
const E2E_LOGIN = /^e2e/
// все пароли, которые e2e-пользователь мог получить в happy-path
const E2E_PASSWORDS = ['e2e-pass', 'e2e-pass-2']

// дефолт из Domain/Constants/DefaultAdmin.cs. Если бэкенд запущен с
// переменной DEFAULT_ADMIN_PASSWORD — задай здесь E2E_ADMIN_PASSWORD.
const adminPassword = process.env.E2E_ADMIN_PASSWORD ?? 'admin'

export async function cleanupE2eUsers() {
  const admin = await request.newContext({ baseURL: BASE })

  // 1. кто вообще есть в БД (админский список, постранично)
  const login = await admin.post('/api/auth/login', {
    data: { login: 'admin', password: adminPassword },
  })
  if (!login.ok()) {
    await admin.dispose()
    throw new Error(`[e2e cleanup] admin login failed: ${login.status()} — задай E2E_ADMIN_PASSWORD`)
  }

  const logins: string[] = []
  let seen = 0
  for (let page = 1; ; page++) {
    const res = await admin.get(`/api/admin/users?pageSize=100&page=${page}`)
    if (!res.ok()) throw new Error(`[e2e cleanup] users list failed: ${res.status()}`)
    const { items, total } = (await res.json()) as { items: { login: string }[]; total: number }
    logins.push(...items.map((u) => u.login).filter((l) => E2E_LOGIN.test(l)))
    seen += items.length
    if (seen >= total || items.length === 0) break
  }
  await admin.dispose()

  // 2. под каждым e2e-юзером входим и удаляем его аккаунт
  for (const user of logins) {
    const ctx = await request.newContext({ baseURL: BASE })
    try {
      for (const password of E2E_PASSWORDS) {
        const res = await ctx.post('/api/auth/login', { data: { login: user, password } })
        if (res.ok()) {
          await ctx.delete('/api/account')
          console.log(`[e2e cleanup] removed user: ${user}`)
          break
        }
      }
    } finally {
      await ctx.dispose()
    }
  }
}
