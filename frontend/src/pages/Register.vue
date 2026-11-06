<script setup lang="ts">
import { computed, ref, watch } from 'vue'
import Credentials from '@/components/Credentials.vue'
import { useRoute, useRouter } from 'vue-router'
import { useRegister } from '@/composables/useSession'
import { toTypedSchema } from '@vee-validate/zod'
import z from 'zod'
import { useField, useForm } from 'vee-validate'

const route = useRoute()
const router = useRouter()

const { mutateAsync, error, reset } = useRegister()

// const PASSWORD_MIN_LENGTH = 4
const PASSWORD_PATTERN = /^[A-Za-z0-9!@#$%^&*()_+\-=[\]{};':"\\|,.<>/?]*$/
const schema = toTypedSchema(
  z.object({
    login: z.string().min(1, 'Login is required'),
    password: z
      .string()
      .min(4, 'Password must be longer or equal to 4 symbols')
      .regex(PASSWORD_PATTERN, 'Password contains an invalid character'),
    confirm: z.string(),
  }).refine((data) => data.password === data.confirm, {
    message: 'Passwords do not match',
    path: ['confirm'],
  })
)

const { handleSubmit, isSubmitting, submitCount } = useForm({
  validationSchema: schema,
  initialValues: { login: '', password: '', confirm: '' },
})
const { value: login, errorMessage: loginError, handleBlur: loginBlur, meta: loginMeta } = useField<string>('login')
const { value: password, errors: passwordErrors, handleBlur: passwordBlur, meta: passwordMeta } = useField<string>('password')
const { value: confirm, errorMessage: confirmError, handleBlur: confirmBlur, meta: confirmMeta } = useField<string>('confirm')

const showLoginError = computed(() =>
  (loginMeta.touched || submitCount.value > 0) ? loginError.value : undefined
)

const passwordSettled = ref(false)
let passwordTimer: ReturnType<typeof setTimeout> | undefined
watch(password, () => {
  passwordSettled.value = false
  clearTimeout(passwordTimer)
  passwordTimer = setTimeout(() => {
    passwordSettled.value = true
  }, 500)
})

const showPasswordErrors = computed(() =>
  (passwordMeta.touched || submitCount.value > 0 || passwordSettled.value) ? passwordErrors.value : undefined
)
const showConfirmError = computed(() =>
  (confirmMeta.dirty || submitCount.value > 0) ? confirmError.value : undefined
)

const onSubmit = handleSubmit(async (values) => {
  try {
    await mutateAsync({ login: values.login, password: values.password })
    redirect()
  }
  catch { }
})
function redirect() {
  const q = route.query.redirect
  const redirect = Array.isArray(q) ? q[0] : q
  router.push((redirect as string | undefined) ?? { name: 'tasks' })
}
watch([login, password, confirm], () => { if (error.value) reset() })

</script>

<template>
  <h1 class="mb-6 text-center text-3xl">Register</h1>
  <form @submit="onSubmit"
    class="mx-auto flex w-[90%] max-w-sm flex-col gap-3 rounded-xl border border-input-border bg-element-bg p-6">
    <Credentials v-model:login="login" v-model:password="password" v-model:confirm="confirm"
      :login-error="showLoginError" :password-errors="showPasswordErrors" :confirm-error="showConfirmError"
      @login-blur="loginBlur" @password-blur="passwordBlur" @confirm-blur="confirmBlur" />
    <p v-if="error" class="text-sm mt-2" :style="{ color: 'var(--error)' }">{{ error.message }}</p>
    <BaseButton type="submit" :disabled="isSubmitting" class="w-full">
      {{ isSubmitting ? 'Registering...' : 'Register' }}
    </BaseButton>
  </form>
</template>
