<script setup lang="ts">
import { computed, ref } from 'vue';
import Credentials from '@/components/Credentials.vue';

const login = ref('')
const password = ref('')
const confirm = ref('')

const PASSWORD_PATTERN = /^[A-Za-z0-9!@#$%^&*()_+\-=[\]{};':"\\|,.<>/?]+$/
const PASSWORD_MIN_LENGTH = 4
const validationErrors = computed(() => {
  const errors: string[] = []
  if (password.value !== confirm.value) errors.push('Passwords do not match')
  if (!PASSWORD_PATTERN.test(password.value)) errors.push('Password contains an invalid character')
  if (password.value.length < PASSWORD_MIN_LENGTH) errors.push(`Password must be longer or equal to ${PASSWORD_MIN_LENGTH} symbols`)
  return errors
})
const isValid = computed(() => validationErrors.value.length === 0)
</script>

<template>
  <h1 class="mb-6 text-center text-3xl">Register</h1>
  <form class="mx-auto flex w-[90%] max-w-sm flex-col gap-3 rounded-xl border border-input-border bg-element-bg p-6">
    <Credentials v-model:login="login" v-model:password="password" v-model:confirm="confirm" />
    <ul v-if="!isValid" class="text-error text-sm list-disc list-inside">
      <li v-for="e in validationErrors" :key="e">
        {{ e }}
      </li>
    </ul>
    <BaseButton type="submit" class="w-full">Register</BaseButton>
  </form>
</template>
