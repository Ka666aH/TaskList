<script setup lang="ts"></script>

<template>
  <header class="flex justify-center">
    <nav class="flex gap-3 text-xl">
      <RouterLink :to="{ name: 'login' }" class="text-accent hover:text-accent-hover transition-colors duration-150">
        Login</RouterLink>
      <RouterLink :to="{ name: 'reg' }" class="text-accent hover:text-accent-hover transition-colors duration-150">
        Register</RouterLink>
    </nav>
  </header>
  <main><router-view /></main>
</template>
