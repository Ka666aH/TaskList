<script setup lang="ts">
import { ref } from 'vue'
import List from '@/components/goals/List.vue'
import { useDeleteGoal, useGoals } from '@/composables/useGoals'
import type { Goal } from '@/types/Goal'
import Form from '@/components/goals/Form.vue'

const { data: goals, isLoading, error } = useGoals()
const { mutate: remove } = useDeleteGoal()

const formOpen = ref(false)
const editGoal = ref<Goal | undefined>()

function onCreate() {
  editGoal.value = undefined
  formOpen.value = true
}

function onEdit(id: Goal['id']) {
  const goal = goals?.value?.find((g) => g.id === id)
  if (goal) {
    editGoal.value = goal
    formOpen.value = true
  }
}
</script>

<template>
  <div class="mx-auto flex w-full max-w-5xl flex-col gap-6 p-6">
    <h1 class="text-2xl font-semibold text-text">Tasks</h1>

    <BaseButton class="self-start" @click="onCreate">Add goal</BaseButton>
    <Form v-model="formOpen" :goal="editGoal" />

    <p v-if="isLoading" class="text-sm text-text-muted">Loading…</p>
    <p v-else-if="error" class="text-sm text-error">{{ error.message }}</p>
    <p v-else-if="!goals?.length" class="text-sm text-text-muted">No tasks yet.</p>
    <List v-else :goals="goals" @edit="onEdit" @delete="remove" />
  </div>
</template>
