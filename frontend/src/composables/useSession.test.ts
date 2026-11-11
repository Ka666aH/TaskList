import { describe, test, expect, vi, beforeEach, afterEach } from 'vitest'
import { mount, enableAutoUnmount } from '@vue/test-utils'
import { defineComponent } from 'vue'
import { VueQueryPlugin, QueryClient } from '@tanstack/vue-query'
import type { ApiError } from '@/types/ApiError'
import type { MeResponse } from '@/types/MeResponse'
import type { Credentials } from '@/types/Credentials'
import { authApi } from '@/api/auth'
import { useSession, useLogin, useLogout } from './useSession'

// Фабрика подменяет модуль целиком: authApi превращается в набор vi.fn()
vi.mock('@/api/auth', () => ({
  authApi: {
    reg: vi.fn(),
    login: vi.fn(),
    logout: vi.fn(),
    me: vi.fn(),
  },
}))

const mockMe = vi.mocked(authApi.me)
const err401: ApiError = { code: 'HTTP_401', message: 'Unauthorized', innerMessage: null }
const alice: MeResponse = { login: 'alice', role: 'Client' }

// Хуки composables живут только внутри setup(), поэтому монтируем
// жертвенный компонент и вытаскиваем результат в замыкание.
type Exposed = ReturnType<typeof useSession> & {
  login: ReturnType<typeof useLogin>
  logout: ReturnType<typeof useLogout>
}

function mountSession(): Exposed {
  let exposed!: Exposed
  const Harness = defineComponent({
    setup() {
      exposed = { ...useSession(), login: useLogin(), logout: useLogout() }
      return () => null
    },
  })
  mount(Harness, {
    global: {
      plugins: [
        [
          VueQueryPlugin,
          { queryClient: new QueryClient({ defaultOptions: { queries: { retry: false } } }) },
        ],
      ],
    },
  })
  return exposed
}

enableAutoUnmount(afterEach)

beforeEach(() => {
  vi.clearAllMocks()
})

describe('useSession', () => {
  test('Guest: /auth/me return 401', async () => {
    mockMe.mockRejectedValue(err401)
    const session = mountSession()

    await vi.waitFor(() => expect(session.error.value).not.toBeNull())
    expect(session.error.value?.code).toBe('HTTP_401')
    expect(session.isAuthed.value).toBe(false)
    expect(session.isAdmin.value).toBe(false)
  })

  test('Admin: /auth/me return data', async () => {
    mockMe.mockResolvedValue({ login: 'admin', role: 'Admin' })
    const session = mountSession()

    await vi.waitFor(() => expect(session.isAuthed.value).toBe(true))
    expect(session.isAdmin.value).toBe(true)
  })

  test('Session expiration: was authorized, 401 on update → guest', async () => {
    mockMe.mockResolvedValue(alice)
    const session = mountSession()
    await vi.waitFor(() => expect(session.isAuthed.value).toBe(true))

    mockMe.mockRejectedValue(err401)
    await session.refetch()
    await vi.waitFor(() => expect(session.isAuthed.value).toBe(false))
  })

  test('Successful login clear cache → /auth/me refetching', async () => {
    mockMe.mockRejectedValueOnce(err401) // первый me: гость
    mockMe.mockResolvedValue(alice) // все последующие
    const session = mountSession()
    await vi.waitFor(() => expect(session.error.value).not.toBeNull()) // первый me завершился

    const credentials: Credentials = { login: 'alice', password: '1234' }
    await session.login.mutateAsync(credentials)

    await vi.waitFor(() => expect(session.isAuthed.value).toBe(true))
    expect(session.data.value).toEqual(alice)
    expect(mockMe).toHaveBeenCalledTimes(2)
  })
})
