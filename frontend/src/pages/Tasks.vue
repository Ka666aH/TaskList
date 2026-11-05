<script setup lang="ts">
import { computed, ref } from 'vue'
import List from '@/components/goals/List.vue'
import { useCreateGoal, useDeleteGoal, useGoals, useUpdateGoal } from '@/composables/useGoals'
import type { Goal, GoalRequest } from '@/types/Goal'

const { data: goals, isLoading, error } = useGoals()
const { mutate: remove } = useDeleteGoal()
const { mutate: create, isPending: isCreating } = useCreateGoal()
const { mutate: update, isPending: isUpdating } = useUpdateGoal()

const formOpen = ref(false)
const editingId = ref<Goal['id'] | null>(null)
const form = ref({ title: '', description: '', deadline: '' })
const isSaving = computed(() => isCreating.value || isUpdating.value)

function openCreate() {
  editingId.value = null
  form.value = { title: '', description: '', deadline: '' }
  formOpen.value = true
}

function openEdit(goal: Goal) {
  editingId.value = goal.id
  form.value = {
    title: goal.title,
    description: goal.description ?? '',
    // сервер отдаёт ISO с Z, инпуту нужен YYYY-MM-DDTHH:mm
    deadline: goal.deadline ? goal.deadline.slice(0, 16) : '',
  }
  formOpen.value = true
}

function onEdit(id: Goal['id']) {
  const goal = goals?.value?.find((g) => g.id === id)
  if (goal) openEdit(goal)
}

function closeForm() {
  formOpen.value = false
  editingId.value = null
}

function submit() {
  const gr: GoalRequest = {
    title: form.value.title.trim(),
    description: form.value.description.trim() || null,
    deadline: form.value.deadline || null,
  }
  if (editingId.value) {
    update({ id: editingId.value, gr }, { onSuccess: closeForm })
  } else {
    create(gr, { onSuccess: closeForm })
  }
}
</script>

<template>
  <div class="mx-auto flex w-full max-w-5xl flex-col gap-6 p-6">
    <h1 class="text-2xl font-semibold text-text">Tasks</h1>

    <BaseButton v-if="!formOpen" class="self-start" @click="openCreate">Add goal</BaseButton>

    <form v-else class="flex flex-col gap-3 rounded-xl border border-input-border bg-element-bg p-4"
      @submit.prevent="submit">
      <BaseInput v-model="form.title" placeholder="Title" />
      <textarea v-model="form.description" placeholder="Description"
        class="w-full rounded-lg border border-input-border bg-element-bg px-3 py-1 text-m text-element-text placeholder:text-text-muted transition-colors duration-150 focus:border-accent focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-accent"></textarea>
      <BaseInput v-model="form.deadline" type="datetime-local" />

      <div class="flex gap-2">
        <BaseButton type="submit" :disabled="isSaving || !form.title.trim()">
          {{ isSaving ? 'Saving…' : 'Save' }}
        </BaseButton>
        <BaseButton variant="ghost" @click="closeForm">Cancel</BaseButton>
      </div>
    </form>

    <p v-if="isLoading" class="text-sm text-text-muted">Loading…</p>
    <p v-else-if="error" class="text-sm text-error">{{ error.message }}</p>
    <p v-else-if="!goals?.length" class="text-sm text-text-muted">No tasks yet.</p>

    <List v-else :goals="goals" @edit="onEdit" @delete="remove" />
  </div>
</template>
