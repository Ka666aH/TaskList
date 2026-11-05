<script setup lang="ts">
import Card from '@/components/goals/Card.vue'
import type { Goal } from '@/types/Goal'

defineProps<{ goals: Goal[] }>()
const emit = defineEmits<{
  edit: [id: Goal['id']]
  delete: [id: Goal['id']]
}>()
</script>

<template>
  <div class="flex flex-col gap-3">
    <Card v-for="goal in goals" :key="goal.id" :goal="goal" @edit="emit('edit', $event)"
      @delete="emit('delete', $event)" />
  </div>
</template>
