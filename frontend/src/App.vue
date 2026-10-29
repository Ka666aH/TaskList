<script setup lang="ts">
import { VueQueryDevtools } from '@tanstack/vue-query-devtools'
import { useLogout, useSession } from './composables/useSession';
import { useRouter } from 'vue-router';

const router = useRouter()

const { isAuthed, isAdmin } = useSession()
const { mutate, isPending } = useLogout();
function onLogout() {
  mutate(undefined,
    {
      onSuccess: () => router.push({ name: 'login' }),
    },
  )
}
</script>

<template>
  <VueQueryDevtools />
  <div class="flex min-h-screen flex-col">
    <header class="flex items-center justify-between pr-2 pl-2 border-b border-input-border bg-element-bg">
      <nav
        class="flex justify-center gap-3 py-3 text-xl text-accent hover:text-accent-hover transition-colors duration-150">
        <RouterLink :to="{ name: 'login' }" v-if="!isAuthed">Login</RouterLink>
        <RouterLink :to="{ name: 'reg' }" v-if="!isAuthed">Register</RouterLink>
        <RouterLink :to="{ name: 'tasks' }" v-if="isAuthed" class="">Tasks</RouterLink>
        <RouterLink :to="{ name: 'admin' }" v-if="isAdmin">Admin</RouterLink>
      </nav>
      <div v-if="isAuthed" class="flex items-center gap-3">
        <RouterLink :to="{ name: 'account' }" class="inline-flex items-center justify-center w-9 h-9 rounded-full
        border-2 border-accent text-accent
        hover:border-accent hover:bg-element-bg-focus
        transition-colors" aria-label="Account">
          <div class="relative w-5 h-5">
            <div class="absolute left-1/2 top-0 -translate-x-1/2 w-2.5 h-2.5 rounded-full bg-current" />
            <div class="absolute bottom-0 left-1/2 -translate-x-1/2 w-4 h-2 rounded-t-full bg-current" />
          </div>
        </RouterLink>
        <BaseButton @click="onLogout">{{ isPending ? 'Logging out...' : 'Logout' }}</BaseButton>
      </div>
    </header>
    <main class="mx-auto flex w-[90%] max-w-2xl flex-1 flex-col justify-center py-8">
      <router-view />
    </main>
  </div>
</template>
