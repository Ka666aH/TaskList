<script setup lang="ts">
import { computed, watch } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import z from 'zod'
import { toTypedSchema } from '@vee-validate/zod'
import { useField, useForm } from 'vee-validate'
import { useLogin } from '@/composables/useSession'
import Credentials from '@/components/Credentials.vue'
import BaseButton from '@/components/UI/BaseButton.vue'

const route = useRoute()
const router = useRouter()

const { mutateAsync, error, reset } = useLogin()

const schema = z.object({
  login: z.string().min(1, 'Login is required'),
  password: z.string().min(1, 'Password is required'),
})
const validationSchema = toTypedSchema(schema)
const { handleSubmit, isSubmitting, submitCount } = useForm({
  validationSchema: validationSchema,
  initialValues: { login: '', password: '' },
})

const { value: login, errorMessage: loginError, handleBlur: loginBlur, meta: loginMeta } = useField<string>('login')
const { value: password, errors: passwordErrors, handleBlur: passwordBlur, meta: passwordMeta } = useField<string>('password')

const showLoginError = computed(() =>
  (loginMeta.touched || submitCount.value > 0) ? loginError.value : undefined
)
const showPasswordErrors = computed(() =>
  (passwordMeta.touched || submitCount.value > 0) ? passwordErrors.value : undefined
)

const onSubmit = handleSubmit(async (values) => {
  try {
    await mutateAsync(values)
    redirect()
  }
  catch { }
})
function redirect() {
  const q = route.query.redirect
  const redirect = Array.isArray(q) ? q[0] : q
  router.push((redirect as string | undefined) ?? { name: 'tasks' })
}
watch([login, password], () => { if (error.value) reset() })
</script>

<template>
  <h1 class="mb-6 text-center text-3xl">Login</h1>
  <form @submit.prevent="onSubmit"
    class="mx-auto flex w-[90%] max-w-sm flex-col gap-3 rounded-xl border border-input-border bg-element-bg p-6">
    <Credentials v-model:login="login" v-model:password="password" :login-error="showLoginError"
      :password-errors="showPasswordErrors" @login-blur="loginBlur" @password-blur="passwordBlur" />
    <p v-if="error" class="text-sm" :style="{ color: 'var(--error)' }">
      {{ error.message }}
    </p>
    <BaseButton type="submit" class="w-full" :disabled="isSubmitting">
      {{ isSubmitting ? 'Logging in…' : 'Log in' }}
    </BaseButton>
  </form>
</template>
