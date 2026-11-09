import { adminApi } from '@/api/admin'
import type { Amount } from '@/types/Amount'
import { keepPreviousData, queryOptions } from '@tanstack/vue-query'
import { computed, toValue, type MaybeRefOrGetter } from 'vue'

export const usersAmountQueryOptions = queryOptions({
  queryKey: ['admin', 'users', 'amount'],
  queryFn: ({ signal }: { signal: AbortSignal }): Promise<Amount | null> =>
    adminApi.getUsersAmount(signal),
  retry: (n, err) => {
    return err.code === 'HTTP_401' || err.code === 'HTTP_403' ? false : n < 2
  },
})

export const goalsAmountQueryOptions = queryOptions({
  queryKey: ['admin', 'goals', 'amount'],
  queryFn: ({ signal }: { signal: AbortSignal }): Promise<Amount | null> =>
    adminApi.getGoalsAmount(signal),
  retry: (n, err) => {
    return err.code === 'HTTP_401' || err.code === 'HTTP_403' ? false : n < 2
  },
})

export const usersQueryOptions = (
  pageSize: MaybeRefOrGetter<number>,
  page: MaybeRefOrGetter<number>,
) =>
  queryOptions({
    queryKey: computed(() => ['admin', 'users', toValue(pageSize), toValue(page)]),
    queryFn: ({ signal }) => adminApi.getUsersPage(toValue(pageSize), toValue(page), signal),
    placeholderData: keepPreviousData,
    retry: (n, err) => (err.code === 'HTTP_401' || err.code === 'HTTP_403' ? false : n < 2),
  })

export const goalsAmountByUserQueryOptions = (userLogin: string) =>
  queryOptions({
    queryKey: ['admin', 'goals', 'amount', userLogin],
    queryFn: ({ signal }): Promise<Amount | null> =>
      adminApi.getGoalsAmountByUser(userLogin, signal),
    retry: (n, err) => (err.code === 'HTTP_401' || err.code === 'HTTP_403' ? false : n < 2),
  })
