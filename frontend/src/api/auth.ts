import { request } from './http'
import type { Credentials } from '@/types/Credantials'
import type { MeResponse } from '@/types/MeResponse'

export const api = {
    reg: (body: Credentials) => request<void>('/auth/reg', { method: 'POST', body: JSON.stringify(body), }),
    login: (body: Credentials) => request<void>('/auth/login', { method: 'POST', body: JSON.stringify(body), }),
    logout: () => request<void>('/auth/logout', { method: 'POST', }),
    me: (signal?: AbortSignal) => request<MeResponse>('/auth/me', { signal }),
}