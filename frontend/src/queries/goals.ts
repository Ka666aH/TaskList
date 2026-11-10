import { queryOptions } from '@tanstack/vue-query'
import { goalsApi } from '@/api/goals'
import { retryUnlessAuthError } from '@/lib/retry'
import type { Goal } from '@/types/Goal'

export const getAllQueryOptions = queryOptions({
  queryKey: ['goals'],
  queryFn: ({ signal }: { signal: AbortSignal }): Promise<Goal[] | null> => goalsApi.getAll(signal),
  retry: retryUnlessAuthError,
})
