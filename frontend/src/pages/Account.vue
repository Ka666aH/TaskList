<script setup lang="ts">
import ChangePasswordForm from '@/components/account/ChangePasswordForm.vue';
import { useChangePassword, useDeleteAccount } from '@/composables/useAccount';
import { passwordSchema } from '@/validation/password';
import { toTypedSchema } from '@vee-validate/zod';
import { useField, useForm } from 'vee-validate';
import { computed } from 'vue';
import z from 'zod';

const { mutate: remove, isPending: isRemoving, error: removeError } = useDeleteAccount()
const { mutateAsync: change, isPending: isChanging, error: changeError } = useChangePassword()

const onChange = () => formOpen.value = true

const formOpen = defineModel<boolean>({ required: true })
const close = () => formOpen.value = false

const schema = toTypedSchema(
  z.object({
    password: passwordSchema,
    confirm: z.string(),
  }).refine((d) => d.password === d.confirm, {
    message: 'Passwords do not match',
    path: ['confirm'],
  }),
)
const { handleSubmit, isSubmitting, submitCount } = useForm({
  validationSchema: schema,
  initialValues: { password: '', confirm: '' },
})
const { value: password, errors: passwordErrors, meta: passwordMeta } = useField<string>('password')
const { value: confirm, errorMessage: confirmError, meta: confirmMeta } = useField<string>('confirm')

const showPasswordErrors = computed(() =>
  (passwordMeta.dirty || submitCount.value > 0) ? passwordErrors.value : undefined
)
const showConfirmError = computed(() =>
  (confirmMeta.dirty || submitCount.value > 0) ? confirmError.value : undefined
)

const onSubmit = handleSubmit(async (values) => {
  try {
    await change(values.password)
    close()
  }
  catch { }
})

</script>

<template>
  <div class="mx-auto flex w-full max-w-5xl flex-col gap-6 p-6">
    <h1 class="text-2xl font-semibold text-text">Account</h1>
    <div class="flex flex-col gap-4">
      <div>
        <BaseButton @click="remove">{{ isRemoving ? 'Deleting...' : 'Delete Account' }}</BaseButton>
        <p v-if="removeError" class="text-sm text-error mt-1">{{ removeError.message }}</p>
      </div>
      <div>
        <BaseButton @click="onChange">{{ isChanging ? 'Changing...' : 'Change password' }}</BaseButton>
        <p v-if="changeError" class="text-sm text-error">{{ changeError.message }}</p>
      </div>
      <ChangePasswordForm v-model:is-open="formOpen" @change="onSubmit" v-model:password="password"
        v-model:confirm="confirm" :password-errors="showPasswordErrors" :confirm-error="showConfirmError"
        :is-submitting="isSubmitting" />
    </div>
  </div>
</template>
