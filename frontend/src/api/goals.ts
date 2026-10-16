import { request } from './http'
import type { Goal, GoalRequest } from '@/types/Goal'

export const goalsApi = {
    getAll: () => request<Goal[]>('/goals'),
    getOne: (goalId: Goal['id']) => request<Goal>(`/goals/${goalId}`),
    create: (body: GoalRequest) => request<Goal>('/goals', { method: 'POST', body: JSON.stringify(body) }),
    update: (goalId: Goal['id'], body: GoalRequest) => request<Goal>(`/goals/${goalId}`, { method: 'PUT', body: JSON.stringify(body) }),
    remove: (goalId: Goal['id']) => request<void>(`/goals/${goalId}`, { method: 'DELETE' }),
}