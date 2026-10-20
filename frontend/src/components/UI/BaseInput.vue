<script setup lang="ts">
withDefaults(
    defineProps<{
        type?: 'text' | 'password',
        placeholder?: string
    }>(),
    {
        type: 'text',
        placeholder: ''
    })
const model = defineModel<string>({ required: true })
</script>

<template>
    <input :type="type" :placeholder="placeholder" v-model="model">
</template>

<style scoped></style>