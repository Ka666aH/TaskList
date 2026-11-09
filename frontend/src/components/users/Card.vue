<script setup lang="ts">
import { useChangeRole, useGoalsAmountByUser } from '@/composables/useAdmin';
import type { Role } from '@/types/Role';
import type { User } from '@/types/User';
import { ref } from 'vue';
import ConfirmDialog from '../UI/ConfirmDialog.vue';

const props = defineProps<{
  user: User
}>()

const { data: goalsAmount, error: goalsAmountError } = useGoalsAmountByUser(props.user.login)
const { mutate: changeRole } = useChangeRole()

const isDeleteOpen = ref(false)
const role = ref<Role>('Client')

function onClick(newRole: Role) {
  role.value = newRole
  isDeleteOpen.value = true
}

function onConfirm(newRole: Role) {
  changeRole({ userLogin: props.user.login, newRole })
  isDeleteOpen.value = false
}
</script>

<template>
  <article class="rounded-xl border border-input-border bg-element-bg p-4">
    <div class="flex justify-between items-center gap-5">
      <p>{{ user.login }}</p>
      <p>Goals: {{ goalsAmount ? goalsAmount.amount : goalsAmountError }}</p>
      <div class="flex gap-2">
        <BaseButton @click="onClick('Client')">User</BaseButton>
        <BaseButton @click="onClick('Admin')">Admin</BaseButton>
      </div>
    </div>
    <ConfirmDialog v-model="isDeleteOpen" :message="`Set role ${role} for ${user.login}?`" confirm-label="Set role"
      @confirm="onConfirm(role)" />
  </article>
</template>
