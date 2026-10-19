import { ref } from "vue"
import type { ApiError } from "@/types/ApiError";

export function useMutation<T>(apiFn: () => Promise<T>) {
    const data = ref<T | null>(null)
    const loading = ref(false);
    const error = ref<ApiError | null>(null)

    async function run() {
        if (loading.value) return
        loading.value = true
        error.value = null

        try { data.value = await apiFn() }
        catch (e) { error.value = e as ApiError }
        finally { loading.value = false }
    }
    return { data, loading, error, run }
}