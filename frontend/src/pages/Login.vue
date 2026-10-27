<script setup lang="ts">
import { ref } from 'vue'
import Credentials from '@/components/Credentials.vue'

const login = ref('')
const password = ref('')
</script>

<template>
  <h1 class="mb-6 text-center text-3xl">Login</h1>
  <form
    class="mx-auto flex w-[90%] max-w-sm flex-col gap-3 rounded-xl border border-input-border bg-element-bg p-6"
  >
    <Credentials v-model:login="login" v-model:password="password" />
    <BaseButton type="submit" class="w-full">Log in</BaseButton>
  </form>
</template>
