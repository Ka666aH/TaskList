import { queryOptions } from '@tanstack/vue-query'
import { authApi } from '@/api/auth'
import { retryUnlessAuthError } from '@/lib/retry'
import type { MeResponse } from '@/types/MeResponse'

export const meQueryOptions = queryOptions({
  queryKey: ['auth', 'me'],
  queryFn: ({ signal }: { signal: AbortSignal }): Promise<MeResponse | null> => authApi.me(signal),
  retry: retryUnlessAuthError,
})
