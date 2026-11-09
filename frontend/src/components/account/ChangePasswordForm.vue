<script setup lang="ts">
import { computed, watch } from 'vue';
import { toTypedSchema } from '@vee-validate/zod';
import { useField, useForm } from 'vee-validate';
import z from 'zod';
import { passwordSchema } from '@/validation/password';
import { useChangePassword } from '@/composables/useAccount';
import Credentials from '../Credentials.vue';

const isOpen = defineModel<boolean>({ required: true })
const close = () => isOpen.value = false

const schema = toTypedSchema(
  z.object({
    password: passwordSchema,
    confirm: z.string(),
  }).refine((d) => d.password === d.confirm, {
    message: 'Passwords do not match',
    path: ['confirm'],
  }),
)

const { handleSubmit, isSubmitting, submitCount, resetForm } = useForm({
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

const { mutateAsync: change, error: changeError, reset: resetChange } = useChangePassword()

const onSubmit = handleSubmit(async (values) => {
  try {
    await change(values.password)
    close()
  }
  catch { }
})

watch(isOpen, (open) => {
  if (!open) return
  resetForm({ values: { password: '', confirm: '' } })
  resetChange()
})
</script>

<template>
  <Teleport to="body">
    <BaseDialog v-model="isOpen">
      <form @submit="onSubmit"
        class="flex flex-col gap-3 rounded-xl border border-input-border bg-element-bg p-4">
        <Credentials v-model:password="password" v-model:confirm="confirm"
          :password-errors="showPasswordErrors" :confirm-error="showConfirmError" />
        <p v-if="changeError" class="text-sm text-error">{{ changeError.message }}</p>
        <div class="flex gap-2">
          <BaseButton type="submit" :disabled="isSubmitting">
            {{ isSubmitting ? 'Saving…' : 'Save' }}
          </BaseButton>

          <BaseButton variant="ghost" @click="close()">Cancel</BaseButton>
        </div>
      </form>
    </BaseDialog>
  </Teleport>
</template>
