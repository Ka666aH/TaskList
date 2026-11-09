<script setup lang="ts">
import type { User } from '@/types/User';
import Card from './Card.vue';

defineProps<{
  users: User[]
}>()
</script>

<template>
  <div class="flex flex-col gap-3">
    <Card v-for="user in users" :key="user.login" :user="user" />
  </div>
</template>
