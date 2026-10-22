import { defineStore } from "pinia";
import { ref } from "vue";
import { goalsApi } from "@/api/goals";
import type { ApiError } from "@/types/ApiError";
import type { Goal, GoalRequest } from "@/types/Goal";

export const useGoals = defineStore('goals', () => {
    const goals = ref<Goal[]>([])
    const loading = ref<boolean>(false)
    const error = ref<ApiError | null>(null)

    async function fetchGoals() {
        loading.value = true
        error.value = null
        try { goals.value = await goalsApi.getAll() }
        catch (e) { error.value = e as ApiError }
        finally { loading.value = false }
    }
    async function createGoal(body: GoalRequest) {
        const created = await goalsApi.create(body)
        goals.value.push(created)
        return created
    }

    async function updateGoal(id: string, body: GoalRequest) {
        const updated = await goalsApi.update(id, body)
        const i = goals.value.findIndex((g) => g.id === id)
        if (i !== -1) goals.value[i] = updated
        return updated
    }

    async function deleteGoal(id: string) {
        try { await goalsApi.remove(id) }
        catch (e) { if ((e as ApiError).code !== 'HTTP_404') throw e }
        goals.value = goals.value.filter((g) => g.id !== id)
    }

    return {
        goals, loading, error,
        fetchGoals, createGoal, updateGoal, deleteGoal,
    }
})