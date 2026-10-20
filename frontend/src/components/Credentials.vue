<script setup lang="ts">
const login = defineModel<string>('login', { required: true })
const password = defineModel<string>('password', { required: true })
const confirm = defineModel<string>('confirm')
</script>

<template>
    <div>
        <label>
            <span>Login</span>
            <BaseInput v-model="login" placeholder="Enter your user login" />
        </label>
        <label>
            <span>Password</span>
            <BaseInput v-model="password" type="password" placeholder="****" />
        </label>
        <label v-if="confirm !== undefined">
            <span>Confirm password</span>
            <BaseInput v-model="confirm" type="password" placeholder="****" />
        </label>
    </div>
</template>

<style scoped></style>