import { request } from './http'
import type { Role } from '@/types/Role'
import type { Page } from '@/types/Page'
import type { Amount } from '@/types/Amount'

export const adminApi = {
    changeRole: (userLogin: string, newRole: Role) =>
        request<void>(`/admin/users/${encodeURIComponent(userLogin)}/role`, { method: 'PATCH', body: JSON.stringify({ newRole }) }),
    getUsersAmount: (signal?: AbortSignal) => request<Amount>('/admin/users/amount', { signal }),
    getUsersPage: (pageSize: number, page: number, signal?: AbortSignal) =>
        request<Page<string>>(`/admin/users?pageSize=${pageSize}&page=${page}`, { signal }),
    getGoalsAmount: (signal?: AbortSignal) => request<Amount>(`/admin/goals/amount`, { signal }),
    getGoalsAmountByUser: (userLogin: string, signal?: AbortSignal) =>
        request<Amount>(`/goals/amount/${encodeURIComponent(userLogin)}`, { signal }),
}