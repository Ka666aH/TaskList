<script setup lang="ts">
import { useMutation } from '@/composables/useMutation';
import { useGoals } from '@/stores/goals';
import type { Goal } from '@/types/Goal';
import { computed, ref } from 'vue';

const goalStore = useGoals()
const isOpen = defineModel<boolean>({ required: true })
const onClose = () => isOpen.value = false

const props = defineProps<{
    goal?: Goal
}>()
const updateMode = computed(() => !!props.goal)

const { loading, error, run: save } = useMutation(() =>
    updateMode.value ?
        goalStore.updateGoal(props.goal!.id, draft.value) :
        goalStore.createGoal(draft.value)
)

async function onSubmit() {
    await save()
    //TODO error handler
}

const title = ref('')
const description = ref('')
const deadline = ref('')

const draft = computed(() => ({
    title: title.value,
    description: description.value,
    deadline: deadline.value
}))
//TODO loading processor
//TODO validation
</script>
<template>
    <BaseDialog :open="isOpen" @close="onClose">
        <form @submit.prevent="onSubmit">
            <div>
                <BaseInput v-model="title" />
                <BaseInput v-model="description" />
                <BaseInput type="datetime-local" v-model="deadline" />
            </div>
            <div>
                <BaseButton type="submit">{{ updateMode ? 'Edit' : 'Create' }}</BaseButton>
                <BaseButton @click="onClose">Cancel</BaseButton>
            </div>
        </form>
    </BaseDialog>
</template>

<style scoped></style>