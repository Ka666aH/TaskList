import { request } from './http'
import type { User } from '@/types/User'
import type { Role } from '@/types/Role'
import type { Page } from '@/types/Page'
import type { Amount } from '@/types/Amount'

export const adminApi = {
    changeRole: (userLogin: User['login'], newRole: Role) => request<void>(`/admin/users/${userLogin}/role`, { method: 'PATCH', body: JSON.stringify({ newRole }) }),
    getUsersAmount: () => request<Amount>('/admin/users/amount'),
    getUsersPage: (pageSize: number, page: number) => request<Page<User>>(`/admin/users?pageSize=${pageSize}&page=${page}`),
    getGoalsAmount: () => request<Amount>(`/admin/goals/amount`),
    getGoalsAmountByUser: (userLogin: User['login']) => request<Amount>(`/goals/amount/${userLogin}`),
}