import { test, expect } from '@playwright/test'

// ≤10 символов — лимит login в бэке (varchar(10)). Уникальный на каждый прогон:
// повторный запуск не падает с «логин занят».
const uniqueLogin = () => 'e2e' + Math.random().toString(36).slice(2, 6)
const password = 'e2e-pass'
const newPassword = 'e2e-pass-2'

// datetime-local принимает ЛОКАЛЬНУЮ строку 'yyyy-MM-ddTHH:mm'.
// toISOString() дал бы UTC (отстал на 3 часа) — тот же переход local/UTC, что в Form.vue.
function futureLocalInput() {
  const d = new Date(Date.now() + 24 * 60 * 60 * 1000)
  const pad = (n: number) => String(n).padStart(2, '0')
  return `${d.getFullYear()}-${pad(d.getMonth() + 1)}-${pad(d.getDate())}T${pad(d.getHours())}:${pad(d.getMinutes())}`
}

// Полный цикл клиента. Аккаунт удаляется в конце — тест не оставляет
// пользователя в БД (задачи удалены раньше, на шаге «удаление задачи»).
test('register → goal crud → change password → login → delete account', async ({ page }) => {
  const user = uniqueLogin()

  // регистрация
  await page.goto('/login')
  await page.getByRole('link', { name: 'Register' }).click()
  // /reg грузится лениво: пока импорт идёт, в DOM ещё страница /login.
  // Без этого ожидания fill попадает в СТАРОЕ поле Login (на /login),
  // страница переключается — и на /reg поле пустое: «Login is required».
  await expect(page.getByRole('heading', { name: 'Register' })).toBeVisible()
  await page.getByLabel('Login').fill(user)
  await page.getByLabel('Password', { exact: true }).fill(password)
  await page.getByLabel('Confirm password').fill(password)
  await page.getByRole('button', { name: 'Register' }).click()
  await expect(page).toHaveURL('/')

  // создание задачи
  await page.getByRole('button', { name: 'Add goal' }).click()
  const form = page.getByRole('dialog') // открытый <dialog> ровно один
  await form.getByPlaceholder('Title').fill('E2E goal')
  await form.locator('input[type="datetime-local"]').fill(futureLocalInput())
  await form.getByRole('button', { name: 'Save' }).click()
  await expect(page.getByRole('heading', { name: 'E2E goal' })).toBeVisible()

  // изменение задачи
  await page.getByRole('button', { name: 'Edit' }).click()
  await form.getByPlaceholder('Title').fill('E2E goal (edited)')
  await form.getByRole('button', { name: 'Save' }).click()
  await expect(page.getByRole('heading', { name: 'E2E goal (edited)' })).toBeVisible()

  // удаление задачи
  await page.getByRole('button', { name: 'Remove' }).click()
  await page.getByRole('dialog').getByRole('button', { name: 'Delete', exact: true }).click()
  await expect(page.getByText('No tasks yet.')).toBeVisible()

  // смена пароля. Приложение после смены само разлогинивает
  // (useChangePassword.onSuccess → logout) — это и есть шаг «логаут».
  await page.getByRole('link', { name: 'Account' }).click()
  await expect(page).toHaveURL('/account')
  await page.getByRole('button', { name: 'Change password' }).click()
  const pwd = page.getByRole('dialog')
  await pwd.getByLabel('Password', { exact: true }).fill(newPassword)
  await pwd.getByLabel('Confirm password').fill(newPassword)
  await pwd.getByRole('button', { name: 'Save' }).click()
  // 401-обработчик добавляет ?redirect=/account — точное сравнение не годится
  await expect(page).toHaveURL(/\/login/)

  // логин с новым паролем; ?redirect=/account возвращает на аккаунт
  await page.getByLabel('Login').fill(user)
  await page.getByLabel('Password', { exact: true }).fill(newPassword)
  await page.getByRole('button', { name: 'Log in', exact: true }).click()
  await expect(page.getByRole('heading', { name: 'Account' })).toBeVisible()

  // удаление аккаунта — заодно убирает тестового пользователя из БД
  await page.getByRole('button', { name: 'Delete Account' }).click()
  await page.getByRole('dialog').getByRole('button', { name: 'Delete', exact: true }).click()
  await expect(page).toHaveURL(/\/login/)
  // сессия сброшена: в шапке снова ссылки гостя
  await expect(page.getByRole('link', { name: 'Login' })).toBeVisible()
})
