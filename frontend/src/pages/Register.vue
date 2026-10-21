<script setup lang="ts">
import { computed, ref } from 'vue';
import { useSession } from '@/stores/session';
import { useRoute, useRouter } from 'vue-router';
import { useMutation } from '@/composables/useMutation';
import Credentials from '@/components/Credentials.vue';

const session = useSession()
const route = useRoute()
const router = useRouter()

const login = ref('')
const password = ref('')
const confirm = ref('')

const PASSWORD_PATTERN = /^[A-Za-z0-9!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?]+$/
const PASSWORD_MIN_LENGTH = 4
const validationErrors = computed(() => {
    const errors: string[] = []
    if (password.value !== confirm.value) errors.push('Passwords do not match')
    if (!PASSWORD_PATTERN.test(password.value)) errors.push('Password contains an invalid character')
    if (password.value.length < PASSWORD_MIN_LENGTH) errors.push(`Password must be longer or equal to ${PASSWORD_MIN_LENGTH} symbols`)
    return errors
})
const isValid = computed(() => validationErrors.value.length === 0)

const { loading, error, run: reg } = useMutation(() =>
    session.register({ login: login.value, password: password.value }),
)

async function onSubmit() {
    await reg()
    if (!error.value) {
        const q = route.query.redirect
        const redirect = Array.isArray(q) ? q[0] : q
        router.push((redirect as string | undefined) ?? { name: 'tasks' })
    }
}
</script>

<template>
    <h1>Register</h1>
    <form @submit.prevent="onSubmit">
        <Credentials v-model:login="login" v-model:password="password" v-model:confirm="confirm" />
        <div v-if="!isValid">
            <p v-for="e in validationErrors" :key="e">{{ e }}</p>
        </div>
        <BaseButton type="submit" :disabled="loading || !isValid">
            {{ loading ? 'Signing up...' : 'Sign up' }}
        </BaseButton>
        <p v-if="error">{{ error.message }}</p>
    </form>
</template>

<style scoped></style>