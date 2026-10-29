import { queryOptions } from '@tanstack/vue-query'
import { goalsApi } from '@/api/goals'
import type { Goal } from '@/types/Goal'

export const getAllQueryOptions = queryOptions({
  queryKey: ['goals'],
  queryFn: ({ signal }: { signal: AbortSignal }): Promise<Goal[] | null> => goalsApi.getAll(signal),
  retry: (n, err) => {
    return err.code === 'HTTP_401' ? false : n < 2
  },
})
