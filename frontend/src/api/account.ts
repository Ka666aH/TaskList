import { request } from './http'

export const accountApi = {
    changePassword: (newPassword: string) => request<void>('/account/password', { method: 'PATCH', body: JSON.stringify({ newPassword }) }),
    remove: () => request<void>('/account', { method: 'DELETE' })
}