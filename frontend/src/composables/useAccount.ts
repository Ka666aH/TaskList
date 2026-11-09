import { accountApi } from '@/api/account'
import { useMutation } from '@tanstack/vue-query'
import { useLogout } from './useSession'
import { computed } from 'vue'

export function useDeleteAccount() {
  const { mutate: logout, isPending: isLoggingOut } = useLogout()
  const mutation = useMutation({
    mutationFn: accountApi.remove,
    onSuccess: () => logout(),
  })
  const isPending = computed(() => mutation.isPending.value || isLoggingOut.value)
  return { ...mutation, isPending }
}
export function useChangePassword() {
  const { mutate: logout, isPending: isLoggingOut } = useLogout()
  const mutation = useMutation({
    mutationFn: (np: string) => accountApi.changePassword(np),
    onSuccess: () => logout(),
  })
  const isPending = computed(() => mutation.isPending.value || isLoggingOut.value)
  return { ...mutation, isPending }
}
