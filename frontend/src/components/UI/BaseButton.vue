<script setup lang="ts">
withDefaults(
  defineProps<{
    type?: 'button' | 'submit' | 'reset'
    variant?: 'primary' | 'danger' | 'ghost'
  }>(),
  { type: 'button', variant: 'primary' },
)
</script>

<template>
  <button :type="type" class="inline rounded-lg px-4 py-2
          text-sm font-medium transition-colors duration-150 cursor-pointer
          disabled:cursor-not-allowed disabled:opacity-50
          focus-visible:outline-2 focus-visible:outline-offset-2" :class="{
            'bg-accent text-accent-text hover:bg-accent-hover focus-visible:outline-accent': variant === 'primary',
            'bg-error text-white hover:bg-red-700 focus-visible:outline-error': variant === 'danger',
            'bg-transparent text-text hover:bg-element-bg-focus focus-visible:outline-accent': variant === 'ghost',
          }">
    <slot />
  </button>
</template>
