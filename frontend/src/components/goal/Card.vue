<script setup lang="ts">
import { useMutation } from '@/composables/useMutation';
import { useGoals } from '@/stores/goals';
import type { Goal } from '@/types/Goal';

const goalStore = useGoals()

const props = defineProps<{
    goal: Goal
}>()
const onEdit = () => { } //TODO Call GoalForm
async function onRemove() {
    await remove()
}
const { loading: removeLoading, error: removeError, run: remove } =
    useMutation(() => goalStore.deleteGoal(props.goal.id))
</script>

<template>
    <article>
        <div>
            <h3>{{ goal.title }}</h3>
            <p>{{ goal.description }}</p>
            <div>
                <span>{{ goal.createdAt }}</span>
                <span v-if="goal.deadline">{{ goal.deadline }}</span>
            </div>
        </div>
        <div>
            <BaseButton @click="onEdit">Edit</BaseButton>
            <BaseButton @click="onRemove">&times;</BaseButton>
        </div>
    </article>
</template>

<style scoped></style>