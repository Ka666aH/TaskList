<script setup lang="ts"></script>

<template>
  <header>
    <nav>
      <RouterLink :to="{ name: 'login' }">Login</RouterLink>
      <RouterLink :to="{ name: 'reg' }">Register</RouterLink>
    </nav>
  </header>
  <main><router-view /></main>
</template>
