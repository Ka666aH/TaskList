import axios, { AxiosError, type AxiosRequestConfig } from 'axios'
import type { ApiError } from '@/types/ApiError'

const apiClient = axios.create({
  baseURL: '/api',
  withCredentials: true,
})

let unauthorizedHandler: (() => void) | null = null
export function setUnauthorizedHandler(handler: () => void) {
  unauthorizedHandler = handler
}

apiClient.interceptors.response.use(
  (response) => response,
  (error: AxiosError) => {
    if (axios.isCancel(error)) {
      return Promise.reject(error)
    }

    if (!error.response) {
      const networkError: ApiError = {
        code: 'NETWORK',
        message: 'Network error',
        innerMessage: null,
      }
      return Promise.reject(networkError)
    }

    const { status, data } = error.response
    const responseData = data as Partial<ApiError>

    if (status === 401) {
      try {
        unauthorizedHandler?.()
      } catch {}
    }

    const apiError: ApiError = {
      code: responseData.code ?? `HTTP_${status}`,
      message: responseData.message ?? 'Error',
      innerMessage: responseData.innerMessage ?? null,
    }
    return Promise.reject(apiError)
  },
)

export async function request<T>(path: string, config: AxiosRequestConfig = {}): Promise<T> {
  const response = await apiClient.request<T>({
    url: path,
    ...config,
  })

  return response.data
}
