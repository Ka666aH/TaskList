import { adminApi } from '@/api/admin'
import { retryUnlessAuthError } from '@/lib/retry'
import type { Amount } from '@/types/Amount'
import { keepPreviousData, queryOptions } from '@tanstack/vue-query'
import { computed, toValue, type MaybeRefOrGetter } from 'vue'

export const usersAmountQueryOptions = queryOptions({
  queryKey: ['admin', 'users', 'amount'],
  queryFn: ({ signal }: { signal: AbortSignal }): Promise<Amount | null> =>
    adminApi.getUsersAmount(signal),
  retry: retryUnlessAuthError,
})

export const goalsAmountQueryOptions = queryOptions({
  queryKey: ['admin', 'goals', 'amount'],
  queryFn: ({ signal }: { signal: AbortSignal }): Promise<Amount | null> =>
    adminApi.getGoalsAmount(signal),
  retry: retryUnlessAuthError,
})

export const usersQueryOptions = (
  pageSize: MaybeRefOrGetter<number>,
  page: MaybeRefOrGetter<number>,
) =>
  queryOptions({
    queryKey: computed(() => ['admin', 'users', toValue(pageSize), toValue(page)]),
    queryFn: ({ signal }) => adminApi.getUsersPage(toValue(pageSize), toValue(page), signal),
    placeholderData: keepPreviousData,
    retry: retryUnlessAuthError,
  })

export const goalsAmountByUserQueryOptions = (
  userLogin: string,
  enabled: MaybeRefOrGetter<boolean> = true,
) =>
  queryOptions({
    queryKey: ['admin', 'goals', 'amount', userLogin],
    queryFn: ({ signal }): Promise<Amount | null> =>
      adminApi.getGoalsAmountByUser(userLogin, signal),
    enabled,
    retry: retryUnlessAuthError,
  })
