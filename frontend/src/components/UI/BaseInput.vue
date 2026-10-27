<script setup lang="ts">
withDefaults(
  defineProps<{
    type?: 'text' | 'password' | 'datetime-local'
    placeholder?: string
  }>(),
  { type: 'text', placeholder: '' },
)
const model = defineModel<string>({ required: true })
</script>

<template>
  <input
    :type="type"
    :placeholder="placeholder"
    v-model="model"
    class="w-full rounded-lg border border-input-border bg-element-bg px-3 py-1 text-m text-element-text placeholder:text-text-muted transition-colors duration-150 focus:border-accent focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-accent disabled:cursor-not-allowed disabled:opacity-50"
  />
</template>
