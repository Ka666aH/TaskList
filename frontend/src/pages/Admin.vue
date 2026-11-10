<script setup lang="ts">
import { computed, ref, watch } from 'vue';
import List from '@/components/users/List.vue';
import { useChangeRole, useGoalsAmount, useUsers, useUsersAmount } from '@/composables/useAdmin';
import type { Role } from '@/types/Role';

const PAGE_SIZE_OPTIONS = [5, 10, 25, 50];

const pageSize = ref(10);
const page = ref(1);

const { data: usersPage, error: usersError, isFetching: isUsersFetching } = useUsers(pageSize, page);
const { data: usersAmount, error: usersAmountError } = useUsersAmount();
const { data: goalsAmount, error: goalsAmountError } = useGoalsAmount();

const totalPages = computed(() =>
  usersPage.value ? Math.max(1, Math.ceil(usersPage.value.total / pageSize.value)) : 1,
);

const first = () => (page.value = 1);
const prev = () => (page.value = Math.max(1, page.value - 1));
const next = () => (page.value = Math.min(totalPages.value, page.value + 1));
const last = () => (page.value = totalPages.value);

watch(pageSize, () => (page.value = 1));

const isRoleOpen = ref(false);
const pendingUser = ref<{ login: string; role: Role } | null>(null);
const {
  mutate: changeRole,
  isPending: isChanging,
  error: changeError,
  reset: resetChange,
} = useChangeRole();

function askRole(login: string, role: Role) {
  pendingUser.value = { login, role };
  resetChange();
  isRoleOpen.value = true;
}

function onConfirm() {
  if (!pendingUser.value) return;
  changeRole(
    { userLogin: pendingUser.value.login, newRole: pendingUser.value.role },
    { onSuccess: () => (isRoleOpen.value = false) },
  );
}
</script>

<template>
  <div class="mx-auto flex w-full max-w-5xl flex-col gap-6 p-6">
    <h1 class="text-2xl font-semibold text-text">Admin</h1>

    <div class="flex gap-8">
      <p class="text-sm text-text-muted">
        Users:
        <span v-if="usersAmountError" class="text-error">{{ usersAmountError.message }}</span>
        <span v-else class="font-medium text-text">{{ usersAmount?.amount ?? '…' }}</span>
      </p>
      <p class="text-sm text-text-muted">
        Goals:
        <span v-if="goalsAmountError" class="text-error">{{ goalsAmountError.message }}</span>
        <span v-else class="font-medium text-text">{{ goalsAmount?.amount ?? '…' }}</span>
      </p>
    </div>

    <div class="flex items-center justify-end gap-2 text-sm text-text-muted">
      <label for="page-size">Per page</label>
      <select id="page-size" v-model="pageSize"
        class="cursor-pointer rounded-lg border border-input-border bg-element-bg px-2 py-1.5 text-text focus-visible:outline-2 focus-visible:outline-accent">
        <option v-for="size in PAGE_SIZE_OPTIONS" :key="size" :value="size">{{ size }}</option>
      </select>
    </div>

    <p v-if="isUsersFetching && !usersPage" class="text-sm text-text-muted">Loading…</p>
    <p v-else-if="usersError" class="text-sm text-error">{{ usersError.message }}</p>
    <List v-else-if="usersPage && usersPage.items.length" :users="usersPage.items" @ask-role="askRole" />
    <p v-else-if="usersPage" class="text-sm text-text-muted">No users on this page.</p>

    <ConfirmDialog v-model="isRoleOpen"
      :message="pendingUser ? `Set role ${pendingUser.role} for ${pendingUser.login}?` : ''" confirm-label="Set role"
      :is-pending="isChanging" :error="changeError?.message" @confirm="onConfirm" />

    <div class="flex items-center justify-center gap-2">
      <BaseButton variant="ghost" :disabled="page <= 1" @click="first">&laquo;</BaseButton>
      <BaseButton variant="ghost" :disabled="page <= 1" @click="prev">&lsaquo;</BaseButton>
      <span class="min-w-16 text-center text-sm text-text-muted">{{ page }} / {{ totalPages }}</span>
      <BaseButton variant="ghost" :disabled="page >= totalPages" @click="next">&rsaquo;</BaseButton>
      <BaseButton variant="ghost" :disabled="page >= totalPages" @click="last">&raquo;</BaseButton>
    </div>
  </div>
</template>
