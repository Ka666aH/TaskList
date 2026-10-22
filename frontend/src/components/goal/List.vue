<script setup lang="ts">
import type { Goal } from '@/types/Goal';
import GoalCard from './Card.vue';
import { useGoals } from '@/stores/goals.ts';
import { ref } from 'vue';
import type { Command } from '@/types/Command.ts';

const goalStore = useGoals()

const props = defineProps<{
    goals: Goal[]
}>()

const confirmOpened = ref(false)
const confirmTitle = ref('')
const confirmText = ref('')
const confirmCommands = ref<Command[]>([])
const confirmCancel = ref(false)
</script>

<template>
    <section>
        <Confirm :isOpen="confirmOpened" :title="confirmTitle" :text="confirmText" :commands="confirmCommands"
            :cancel="confirmCancel" />
        <ul>
            <li v-for="goal in goals" :key="goal.id">
                <GoalCard :goal="goal" />
            </li>
        </ul>
    </section>
</template>

<style scoped></style>