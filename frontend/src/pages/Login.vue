<script setup lang="ts">
import { ref } from 'vue';
import { useSession } from '@/stores/session';
import { useRoute, useRouter } from 'vue-router';
import { useMutation } from '@/composables/useMutation';
import Credentials from '@/components/Credentials.vue';

const session = useSession()
const route = useRoute()
const router = useRouter()

const login = ref('')
const password = ref('')
const { loading, error, run: logIn } = useMutation(() =>
    session.logIn({ login: login.value, password: password.value }),
)

async function onSubmit() {
    await logIn()
    if (!error.value) {
        const q = route.query.redirect
        const redirect = Array.isArray(q) ? q[0] : q
        router.push((redirect as string | undefined) ?? { name: 'tasks' })
    }
}
</script>

<template>
    <h1>Login</h1>
    <p v-if="session.expired">Session expired. Login again</p>
    <form @submit.prevent="onSubmit">
        <Credentials v-model:login="login" v-model:password="password" />
        <BaseButton type="submit" :disabled="loading">
            {{ loading ? 'Logging in...' : 'Log in' }}
        </BaseButton>
        <p v-if="error">{{ error.message }}</p>
    </form>
</template>

<style scoped></style>