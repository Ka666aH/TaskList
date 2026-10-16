import router from "@/router"
import type { ApiError } from "@/types/ApiError"

const BASE_URL = '/api'

export async function request<T>(path: string, init: RequestInit = {}): Promise<T> {
    const response = await fetch(`${BASE_URL}${path}`, {
        ...init,
        credentials: 'include',
        headers: {
            'Content-Type': 'application/json',
            ...init.headers,
        },
    })

    if (!response.ok) {
        let body: Partial<ApiError> = {}
        try { body = await response.json() } catch { }

        if (response.status === 401 && !path.startsWith('/auth/me')) {
            //TODO session reset
            
            // router.push({ name: 'login', query: { redirect: router.currentRoute.value.fullPath } })
        }

        throw {
            code: body.code ?? `HTTP_${response.status}`,
            message: body.message ?? 'Error',
            innerMessage: body.innerMessage ?? null,
        } satisfies ApiError
    }
    if (response.status === 204) return undefined as T

    return response.json() as Promise<T>
}