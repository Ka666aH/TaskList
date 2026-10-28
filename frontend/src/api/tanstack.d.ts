import '@tanstack/vue-query'
import type { ApiError } from '@/types/ApiError'

declare module '@tanstack/vue-query' {
  interface Register {
    defaultError: ApiError
  }
}
