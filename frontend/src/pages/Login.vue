<script setup lang="ts">
import { ref } from 'vue';
import Credentials from '@/components/Credentials.vue';

const login = ref('')
const password = ref('')
</script>

<template>
  <h1>Login</h1>
  <form>
    <Credentials v-model:login="login" v-model:password="password" />
    <BaseButton type="submit">Log in</BaseButton>
  </form>
</template>
