import type { ApiError } from "@/types/ApiError"

const BASE_URL = '/api'

let unauthorizedHandler: (() => void) | null = null
export function setUnauthorizedHandler(handler: () => void) { unauthorizedHandler = handler }

export async function request<T>(path: string, init: RequestInit = {}): Promise<T> {
    let response: Response
    try {
        response = await fetch(`${BASE_URL}${path}`, {
            ...init,
            credentials: 'include',
            headers: {
                ...(init.body ? { 'Content-Type': 'application/json' } : {}),
                ...init.headers,
            },
        })
    } catch (e) {
        if ((e as Error).name === 'AbortError') throw e
        throw { code: 'NETWORK', message: 'Network error', innerMessage: null } satisfies ApiError
    }

    if (!response.ok) {
        let body: Partial<ApiError> = {}
        try { body = await response.json() } catch { }

        if (response.status === 401)
            try { unauthorizedHandler?.() } catch { }

        throw {
            code: body.code ?? `HTTP_${response.status}`,
            message: body.message ?? 'Error',
            innerMessage: body.innerMessage ?? null,
        } satisfies ApiError
    }
    const text = await response.text()
    return (text ? JSON.parse(text) : undefined) as T
}