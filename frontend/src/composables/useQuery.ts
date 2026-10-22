import { onScopeDispose, ref } from "vue"
import type { ApiError } from "@/types/ApiError";

export function useQuery<T, Args extends unknown[] = []>(
    apiFn: (signal: AbortSignal, ...args: Args) => Promise<T>, immediate = true) {
    const data = ref<T | null>(null)
    const loading = ref(false);
    const error = ref<ApiError | null>(null)

    let requestId: number = 0;
    let controller: AbortController | null = null

    onScopeDispose(() => controller?.abort())

    async function run(...args: Args) {
        loading.value = true
        error.value = null

        const currentId = ++requestId
        controller?.abort()
        const local = new AbortController()
        controller = local

        try {
            const result = await apiFn(local.signal, ...args)
            if (currentId === requestId)
                data.value = result
        }
        catch (e) {
            if ((e as Error).name === 'AbortError') return
            if (currentId === requestId)
                error.value = e as ApiError
        }
        finally {
            if (currentId === requestId)
                loading.value = false
        }
    }
    if (immediate) (run as () => void)()
    return { data, loading, error, run }
}