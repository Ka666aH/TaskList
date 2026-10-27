<script setup lang="ts">
const login = defineModel<string>('login', { required: true })
const password = defineModel<string>('password', { required: true })
const confirm = defineModel<string>('confirm')
</script>

<template>
  <div class="flex flex-col gap-2">
    <label class="flex flex-col gap-1">
      <span class="text-sm font-medium">Login</span>
      <BaseInput v-model="login" placeholder="Enter your user login" />
    </label>
    <label class="flex flex-col gap-1">
      <span class="text-sm font-medium">Password</span>
      <BaseInput v-model="password" type="password" placeholder="****" />
    </label>
    <label v-if="confirm !== undefined" class="flex flex-col gap-1">
      <span class="text-sm font-medium">Confirm password</span>
      <BaseInput v-model="confirm" type="password" placeholder="****" />
    </label>
  </div>
</template>
