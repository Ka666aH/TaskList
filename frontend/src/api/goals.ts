import { request } from './http'
import type { Goal, GoalRequest } from '@/types/Goal'

export const goalsApi = {
    getAll: (signal?: AbortSignal) => request<Goal[]>('/goals', { signal }),
    getOne: (goalId: Goal['id'], signal?: AbortSignal) => request<Goal>(`/goals/${goalId}`, { signal }),
    create: (body: GoalRequest) => request<Goal>('/goals', { method: 'POST', body: JSON.stringify(body) }),
    update: (goalId: Goal['id'], body: GoalRequest) => request<Goal>(`/goals/${goalId}`, { method: 'PUT', body: JSON.stringify(body) }),
    remove: (goalId: Goal['id']) => request<void>(`/goals/${goalId}`, { method: 'DELETE' }),
}