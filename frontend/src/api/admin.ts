import { request } from './http'
import type { User } from '@/types/User'
import type { Role } from '@/types/Role'
import type { Page } from '@/types/Page'
import type { Amount } from '@/types/Amount'

export const adminApi = {
    changeRole: (userLogin: User['login'], newRole: Role) =>
        request<void>(`/admin/users/${encodeURIComponent(userLogin)}/role`, { method: 'PATCH', body: JSON.stringify({ newRole }) }),
    getUsersAmount: (signal?: AbortSignal) => request<Amount>('/admin/users/amount', { signal }),
    getUsersPage: (pageSize: number, page: number, signal?: AbortSignal) =>
        request<Page<User>>(`/admin/users?pageSize=${pageSize}&page=${page}`, { signal }),
    getGoalsAmount: (signal?: AbortSignal) => request<Amount>(`/admin/goals/amount`, { signal }),
    getGoalsAmountByUser: (userLogin: User['login'], signal?: AbortSignal) =>
        request<Amount>(`/goals/amount/${encodeURIComponent(userLogin)}`, { signal }),
}