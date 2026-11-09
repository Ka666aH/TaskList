<script setup lang="ts">
import BaseDialog from './BaseDialog.vue'

const isOpen = defineModel<boolean>({ required: true })

defineProps<{
  message: string
  confirmLabel?: string
  isPending?: boolean
  error?: string | null
}>()

const emit = defineEmits<{ confirm: [] }>()
</script>

<template>
  <Teleport to="body">
    <BaseDialog v-model="isOpen">
      <div class="flex flex-col gap-3 rounded-xl border border-input-border bg-element-bg p-4">
        <p class="text-m">{{ message }}</p>
        <p v-if="error" class="text-sm text-error">{{ error }}</p>
        <div class="flex gap-2">
          <BaseButton @click="emit('confirm')">
            {{ isPending ? '…' : (confirmLabel ?? 'Confirm') }}
          </BaseButton>
          <BaseButton variant="ghost" @click="isOpen = false">Cancel</BaseButton>
        </div>
      </div>
    </BaseDialog>
  </Teleport>
</template>
