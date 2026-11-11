import { adminApi } from '@/api/admin'
import {
  goalsAmountByUserQueryOptions,
  goalsAmountQueryOptions,
  usersAmountQueryOptions,
  usersQueryOptions,
} from '@/queries/admin'
import type { Role } from '@/types/Role'
import { useMutation, useQuery } from '@tanstack/vue-query'
import type { MaybeRefOrGetter } from 'vue'

export function useUsersAmount() {
  const query = useQuery(usersAmountQueryOptions)
  return { ...query, error: query.error }
}

export function useGoalsAmount() {
  const query = useQuery(goalsAmountQueryOptions)
  return { ...query, error: query.error }
}

export function useUsers(pageSize: MaybeRefOrGetter<number>, page: MaybeRefOrGetter<number>) {
  const query = useQuery(usersQueryOptions(pageSize, page))
  return { ...query, error: query.error }
}

// enabled выключен, пока карточка не раскрыта: количество задач запрашивается по кнопке, а не пачкой на каждый рендер списка.
export function useGoalsAmountByUser(userLogin: string, enabled: MaybeRefOrGetter<boolean> = true) {
  const query = useQuery(goalsAmountByUserQueryOptions(userLogin, enabled))
  return { ...query, error: query.error }
}

export function useChangeRole() {
  return useMutation({
    mutationFn: ({ userLogin, newRole }: { userLogin: string; newRole: Role }) =>
      adminApi.changeRole(userLogin, newRole),
  })
}
