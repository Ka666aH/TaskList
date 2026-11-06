<script setup lang="ts">
import type { Goal } from '@/types/Goal'
import { toDisplayDate } from '@/utils/date';

defineProps<{ goal: Goal }>()
const emit = defineEmits<{
  edit: [id: Goal['id']]
  delete: [id: Goal['id']]
}>()
</script>

<template>
  <div class="flex flex-col gap-2 rounded-xl border border-input-border bg-element-bg p-4">
    <div class="flex items-start justify-between gap-2">
      <h3 class="min-w-0 font-semibold text-text text-xl">{{ goal.title }}</h3>
      <div class="flex gap-1 shrink-0">
        <BaseButton @click="emit('edit', goal.id)">Edit</BaseButton>
        <BaseButton @click="emit('delete', goal.id)">Remove</BaseButton>
      </div>
    </div>

    <p v-if="goal.description" class="text-sm text-text-muted line-clamp-3">{{ goal.description }}</p>

    <div class="flex justify-between text-xs text-text-muted">
      <span v-if="goal.deadline" class="text-error font-bold text-[110%]">
        {{ new Date(goal.deadline).toLocaleString() }}</span>
      <span class="ml-auto">{{ toDisplayDate(goal.createAt) }}</span>
    </div>
  </div>
</template>
