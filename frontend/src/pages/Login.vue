<script setup lang="ts">
import { ref } from 'vue';
import Credentials from '@/components/Credentials.vue';

const login = ref('')
const password = ref('')
</script>

<template>
  <h1 class="text-center text-3xl">Login</h1>
  <form class="flex flex-col gap-3 w-[90%] mx-auto">
    <Credentials v-model:login="login" v-model:password="password" />
    <BaseButton type="submit">Log in</BaseButton>
  </form>
</template>
