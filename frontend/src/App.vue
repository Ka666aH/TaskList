<script setup lang="ts">
import { VueQueryDevtools } from '@tanstack/vue-query-devtools'
</script>

<template>
  <VueQueryDevtools />
  <div class="flex min-h-screen flex-col">
    <header class="border-b border-input-border bg-element-bg">
      <nav class="flex justify-center gap-6 py-3 text-xl">
        <RouterLink :to="{ name: 'login' }" class="text-accent hover:text-accent-hover transition-colors duration-150">
          Login</RouterLink>
        <RouterLink :to="{ name: 'reg' }" class="text-accent hover:text-accent-hover transition-colors duration-150">
          Register</RouterLink>
      </nav>
    </header>
    <main class="mx-auto flex w-[90%] max-w-2xl flex-1 flex-col justify-center py-8">
      <router-view />
    </main>
  </div>
</template>
