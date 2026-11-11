<script setup lang="ts">
import type { Role } from '@/types/Role'
import type { User } from '@/types/User'
import Card from './Card.vue'

defineProps<{
  users: User[]
}>()

const emit = defineEmits<{
  askRole: [login: string, role: Role]
}>()

const forward = (login: string, role: Role) => emit('askRole', login, role)
</script>

<template>
  <div class="flex flex-col gap-3">
    <Card v-for="user in users" :key="user.login" :user="user" @ask-role="forward" />
  </div>
</template>
