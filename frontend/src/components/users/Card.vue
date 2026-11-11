<script setup lang="ts">
import { ref } from 'vue'
import type { Role } from '@/types/Role'
import type { User } from '@/types/User'
import { useGoalsAmountByUser } from '@/composables/useAdmin'

const props = defineProps<{
  user: User
}>()

const emit = defineEmits<{
  askRole: [login: string, role: Role]
}>()

const isGoalsOpen = ref(false)
const {
  data: goalsAmount,
  error: goalsAmountError,
  isFetching: isGoalsFetching,
} = useGoalsAmountByUser(props.user.login, isGoalsOpen)
</script>

<template>
  <article
    class="flex items-center justify-between gap-4 rounded-xl border border-input-border bg-element-bg p-4"
  >
    <p class="font-medium text-text">{{ user.login }}</p>
    <div class="flex items-center gap-2">
      <span v-if="isGoalsOpen" class="text-sm text-text-muted">
        <span v-if="isGoalsFetching">…</span>
        <span v-else-if="goalsAmountError" class="text-error">{{ goalsAmountError.message }}</span>
        <span v-else>Goals: {{ goalsAmount?.amount ?? '—' }}</span>
      </span>
      <BaseButton variant="ghost" @click="isGoalsOpen = !isGoalsOpen">Goals</BaseButton>
      <BaseButton @click="emit('askRole', user.login, 'Client')">User</BaseButton>
      <BaseButton @click="emit('askRole', user.login, 'Admin')">Admin</BaseButton>
    </div>
  </article>
</template>
