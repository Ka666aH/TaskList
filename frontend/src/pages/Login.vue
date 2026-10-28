<script setup lang="ts">
import { ref } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import Credentials from '@/components/Credentials.vue'
import BaseButton from '@/components/UI/BaseButton.vue'
import { useLogin } from '@/composables/useSession'

const login = ref('')
const password = ref('')

const route = useRoute()
const router = useRouter()

const { mutate, error, isPending } = useLogin()

function onSubmit() {
  mutate(
    { login: login.value, password: password.value },
    {
      onSuccess: () => {
        const q = route.query.redirect
        const redirect = Array.isArray(q) ? q[0] : q
        router.push((redirect as string | undefined) ?? { name: 'tasks' })
      },
    },
  )
}
</script>

<template>
  <h1 class="mb-6 text-center text-3xl">Login</h1>
  <form @submit.prevent="onSubmit"
    class="mx-auto flex w-[90%] max-w-sm flex-col gap-3 rounded-xl border border-input-border bg-element-bg p-6">
    <Credentials v-model:login="login" v-model:password="password" />

    <p v-if="error" class="text-sm" :style="{ color: 'var(--error)' }">
      {{ error.message }}
    </p>

    <BaseButton type="submit" class="w-full" :disabled="isPending">
      {{ isPending ? 'Logging in…' : 'Log in' }}
    </BaseButton>
  </form>
</template>
