import { useQuery, useMutation, useQueryClient } from '@tanstack/vue-query'
import { getAllQueryOptions } from '@/queries/goals'
import { goalsApi } from '@/api/goals'
import type { Goal, GoalRequest } from '@/types/Goal'

export function useGoals() {
  const query = useQuery(getAllQueryOptions)
  return { ...query, error: query.error }
}

export function useCreateGoal() {
  const qc = useQueryClient()
  return useMutation({
    mutationFn: (gr: GoalRequest) => goalsApi.create(gr),
    onMutate: async (gr: GoalRequest) => {
      await qc.cancelQueries({ queryKey: ['goals'] })
      const snapshot = qc.getQueryData<Goal[]>(['goals'])
      const newGoal = createGoalDraft(gr)
      qc.setQueryData<Goal[]>(['goals'], (old) => (old ? [...old, newGoal] : [newGoal]))
      return { snapshot }
    },
    onError: (_err, _variables, onMutateResult) => {
      if (onMutateResult) {
        qc.setQueryData(['goals'], onMutateResult.snapshot)
      }
    },
    onSettled: () => qc.invalidateQueries({ queryKey: ['goals'] }),
  })
}

export function useUpdateGoal() {
  const qc = useQueryClient()
  return useMutation({
    mutationFn: ({ id, gr }: { id: Goal['id']; gr: GoalRequest }) => goalsApi.update(id, gr),
    onMutate: async ({ id, gr }) => {
      await qc.cancelQueries({ queryKey: ['goals'] })
      const snapshot = qc.getQueryData<Goal[]>(['goals'])
      qc.setQueryData<Goal[]>(['goals'], (old) =>
        old?.map((g) => (g.id === id ? { ...g, ...gr } : g)),
      )
      return { snapshot }
    },
    onError: (_err, _variables, onMutateResult) => {
      if (onMutateResult) {
        qc.setQueryData<Goal[]>(['goals'], onMutateResult.snapshot)
      }
    },
    onSettled: () => qc.invalidateQueries({ queryKey: ['goals'] }),
  })
}

export function useDeleteGoal() {
  const qc = useQueryClient()
  return useMutation({
    mutationFn: (id: Goal['id']) => goalsApi.remove(id),
    onMutate: async (id) => {
      await qc.cancelQueries({ queryKey: ['goals'] })
      const snapshot = qc.getQueryData<Goal[]>(['goals'])
      qc.setQueryData<Goal[]>(['goals'], (old) => old?.filter((g) => g.id !== id))
      return { snapshot }
    },
    onError: (_err, _variables, onMutateResult) => {
      if (onMutateResult) {
        qc.setQueryData<Goal[]>(['goals'], onMutateResult.snapshot)
      }
    },
    onSettled: () => qc.invalidateQueries({ queryKey: ['goals'] }),
  })
}

function createGoalDraft(g: GoalRequest): Goal {
  return {
    id: crypto.randomUUID(),
    createdAt: new Date().toISOString(),
    title: g.title,
    description: g.description,
    deadline: g.deadline,
  }
}
