<script setup lang="ts">
const login = defineModel<string>('login', { required: true })
const password = defineModel<string>('password', { required: true })
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
    </div>
</template>

<style scoped></style>