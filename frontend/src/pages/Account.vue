<script setup lang="ts">
import { ref } from 'vue';
import ChangePasswordForm from '@/components/account/ChangePasswordForm.vue';
import { useDeleteAccount } from '@/composables/useAccount';

const isFormOpen = ref(false)
const isDeleteOpen = ref(false)

const {
  mutate: remove,
  isPending: isRemoving,
  error: removeError,
  reset: resetRemove,
} = useDeleteAccount()
</script>

<template>
  <div class="mx-auto flex w-full max-w-5xl flex-col gap-6 p-6">
    <h1 class="text-2xl font-semibold text-text">Account</h1>
    <div class="flex flex-col gap-4">
      <div>
        <BaseButton @click="isFormOpen = true">Change password</BaseButton>
      </div>
      <div>
        <BaseButton variant="ghost" @click="isDeleteOpen = true; resetRemove()">
          Delete Account
        </BaseButton>
      </div>
    </div>

    <ChangePasswordForm v-model="isFormOpen" />

    <ConfirmDialog
      v-model="isDeleteOpen"
      message="Delete your account? This cannot be undone."
      confirm-label="Delete"
      :is-pending="isRemoving"
      :error="removeError?.message"
      @confirm="remove"
    />
  </div>
</template>
