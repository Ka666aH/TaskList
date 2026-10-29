import { computed } from 'vue'
import { useQuery, useMutation, useQueryClient } from '@tanstack/vue-query'
import { authApi } from '@/api/auth'
import type { Credentials } from '@/types/Credentials'
import { meQueryOptions } from '@/queries/auth'

export function useSession() {
  const query = useQuery(meQueryOptions)
  return {
    ...query,
    isAuthed: computed(() => !query.error.value && !!query.data.value),
    isAdmin: computed(() => query.data.value?.role === 'Admin'),
    error: query.error,
  }
}

export function useLogin() {
  const qc = useQueryClient()
  return useMutation({
    mutationFn: (c: Credentials) => authApi.login(c),
    onSuccess: () => qc.invalidateQueries({ queryKey: ['auth', 'me'] }),
  })
}

export function useLogout() {
  const qc = useQueryClient()
  return useMutation({
    mutationFn: () => authApi.logout(),
    onSuccess: () => qc.resetQueries({ queryKey: ['auth', 'me'] }),
  })
}

export function useRegister() {
  const qc = useQueryClient()
  return useMutation({
    mutationFn: async (c: Credentials) => {
      await authApi.reg(c)
      await authApi.login(c)
    },
    onSuccess: () => qc.invalidateQueries({ queryKey: ['auth', 'me'] }),
  })
}
