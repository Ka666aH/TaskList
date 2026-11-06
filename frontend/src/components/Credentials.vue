<script setup lang="ts">
defineProps<{
  loginError?: string
  passwordErrors?: string[]
  confirmError?: string
}>()

const login = defineModel<string>('login', { required: true })
const password = defineModel<string>('password', { required: true })
const confirm = defineModel<string>('confirm')
</script>

<template>
  <div class="flex flex-col gap-2">
    <label class="flex flex-col gap-1">
      <span class="text-sm font-medium">Login</span>
      <BaseInput v-model="login" placeholder="Enter your user login" />
      <span v-if="loginError" class="text-sm text-error">{{ loginError }}</span>
    </label>

    <label class="flex flex-col gap-1">
      <span class="text-sm font-medium">Password</span>
      <BaseInput v-model="password" type="password" placeholder="****" />
      <ul v-if="passwordErrors?.length" class="text-sm text-error">
        <li v-for="e in passwordErrors" :key="e">{{ e }}</li>
      </ul>
    </label>

    <label v-if="confirm !== undefined" class="flex flex-col gap-1">
      <span class="text-sm font-medium">Confirm password</span>
      <BaseInput v-model="confirm" type="password" placeholder="****" />
      <span v-if="confirmError" class="text-sm text-error">{{ confirmError }}</span>
    </label>
  </div>
</template>
