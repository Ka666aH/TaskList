<script setup lang="ts">
import Credentials from '../Credentials.vue';

defineProps<{
  passwordErrors?: string[]
  confirmError?: string

  isSubmitting: boolean
}>()
defineEmits<{
  change: []
}>()

const isOpen = defineModel<boolean>('isOpen', { required: true })
const close = () => isOpen.value = false

const password = defineModel<string>('password', { required: true })
const confirm = defineModel<string>('confirm')

</script>

<template>
  <Teleport to="body">
    <BaseDialog v-model="isOpen">
      <form @submit.prevent="$emit('change')"
        class="flex flex-col gap-3 rounded-xl border border-input-border bg-element-bg p-4">
        <Credentials v-model:password="password" v-model:confirm="confirm" :password-errors="passwordErrors"
          :confirm-error="confirmError" />
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
