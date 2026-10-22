import { ref } from "vue"
import type { ApiError } from "@/types/ApiError";

export function useMutation<T, Args extends unknown[] = []>(
    apiFn: (...args: Args) => Promise<T>) {
    const data = ref<T | null>(null)
    const loading = ref(false);
    const error = ref<ApiError | null>(null)

    async function run(...args: Args) {
        if (loading.value) return
        loading.value = true
        error.value = null

        try { data.value = await apiFn(...args) }
        catch (e) { error.value = e as ApiError }
        finally { loading.value = false }
    }
    return { data, loading, error, run }
}