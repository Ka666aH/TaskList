<script setup lang="ts">
import List from '@/components/users/List.vue';
import { useGoalsAmount, useUsers, useUsersAmount } from '@/composables/useAdmin';
import { computed, ref, watch } from 'vue';

const PAGE_SIZE = 1
const PAGE = 1

const pageSize = ref(PAGE_SIZE)
const page = ref(PAGE)

const { data: usersPage, error: usersError } = useUsers(pageSize, page)
const { data: usersAmount, error: usersAmountError } = useUsersAmount()
const { data: goalsAmount, error: goalsAmountError } = useGoalsAmount()

const totalPages = computed(() =>
  usersPage.value?.total ? Math.max(1, Math.ceil(usersPage.value.total / pageSize.value)) : 1,
)

const first = () => page.value = 1
const prev = () => page.value = Math.max(1, page.value - 1)
const next = () => page.value = Math.min(totalPages.value, page.value + 1)
const last = () => page.value = totalPages.value

watch(pageSize, () => { page.value = 1 })
</script>

<template>
  <div class="mx-auto flex w-full max-w-5xl flex-col gap-6 p-6">
    <h1 class="text-2xl font-semibold text-text">Admin</h1>
    <div class="text-xl text-accent">
      <p>
        Users amount:
        <span v-if="usersAmount">{{ usersAmount.amount }}</span>
        <span v-else-if="usersAmountError" class="text-error">{{ usersAmountError.message }}</span>
        <span v-else>…</span>
      </p>
      <p>
        Goals amount:
        <span v-if="goalsAmount">{{ goalsAmount.amount }}</span>
        <span v-else-if="goalsAmountError" class="text-error">{{ goalsAmountError.message }}</span>
        <span v-else>…</span>
      </p>
    </div>
    <div class="flex justify-between">
      <BaseButton :disabled="page <= 1" @click="first">&lt;&lt;</BaseButton>
      <BaseButton :disabled="page <= 1" @click="prev">&lt;</BaseButton>
      <span class="self-center text-sm text-text-muted">{{ page }} / {{ totalPages }}</span>
      <BaseButton :disabled="page >= totalPages" @click="next">&gt;</BaseButton>
      <BaseButton :disabled="page >= totalPages" @click="last">&gt;&gt;</BaseButton>
    </div>
    <List v-if="usersPage && usersPage.items.length > 0" :users="usersPage.items" />
    <p v-else-if="usersError" class="text-error">{{ usersError.message }}</p>
    <p v-else-if="usersPage" class="text-sm text-text-muted">No users on this page.</p>
  </div>
</template>
