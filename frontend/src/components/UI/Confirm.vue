<script setup lang="ts">
import { ref } from 'vue'
import type { Command } from '@/types/Command'

const isOpen = defineModel<boolean>({ required: true })
const props = withDefaults(
    defineProps<{
        title: string
        text: string
        commands: Command[]
        cancel?: boolean
    }>(),
    {
        cancel: true
    })

const loading = ref(false)
const error = ref<string | null>(null)

function onClose() {
    if (loading.value) return
    isOpen.value = false
}

async function run(command: Command) {
    if (loading.value) return
    loading.value = true
    error.value = null
    try {
        await command.func()
        isOpen.value = false
    }
    catch (e) { error.value = (e as Error).message ?? 'Error' }
    finally { loading.value = false }
}
</script>

<template>
    <BaseDialog :open="isOpen" @close="onClose">
        <div>
            <h2>{{ title }}</h2>
            <p>{{ text }}</p>
            <p v-if="error" class="error">{{ error }}</p>
        </div>
        <div>
            <BaseButton v-for="c in commands" :key="c.name" :disabled="loading" @click="run(c)">
                {{ c.name }}
            </BaseButton>
            <BaseButton v-if="cancel" :disabled="loading" @click="onClose">Cancel</BaseButton>
        </div>
    </BaseDialog>
</template>