<script setup lang="ts">
import { computed, watch } from 'vue'
import { toTypedSchema } from '@vee-validate/zod'
import { useField, useForm } from 'vee-validate'
import z from 'zod'
import type { Goal, GoalRequest } from '@/types/Goal'
import { useCreateGoal, useUpdateGoal } from '@/composables/useGoals'
import { toIso, toLocalInput } from '@/utils/date'

const isOpen = defineModel<boolean>({ required: true })
const close = () => (isOpen.value = false)
const props = defineProps<{
  goal?: Goal
}>()

const schema = toTypedSchema(
  z.object({
    title: z.string().trim().min(1, 'Title is required'),
    description: z.preprocess(
      (v) => (typeof v === 'string' && v.trim() === '' ? null : v),
      z.string().trim().nullable(),
    ),
    deadline: z.preprocess(
      (v) => (v === '' || v === null || v === undefined ? null : v),
      z
        .string()
        .nullable()
        .refine((v) => v === null || new Date(v) > new Date(), {
          message: 'Deadline must be in the future',
        }),
    ),
  }),
)

function getInitialValues() {
  return {
    title: props.goal?.title ?? '',
    description: props.goal?.description ?? '',
    deadline: toLocalInput(props.goal?.deadline),
  }
}

const { handleSubmit, isSubmitting, submitCount, resetForm } = useForm({
  validationSchema: schema,
  initialValues: getInitialValues(),
})

const { value: title, errorMessage: titleError } = useField<string>('title')
const { value: description } = useField<string>('description')
const {
  value: deadline,
  errorMessage: deadlineError,
  meta: deadlineMeta,
} = useField<string>('deadline')

const showTitleError = computed(() => (submitCount.value > 0 ? titleError.value : undefined))

const showDeadlineError = computed(() =>
  deadlineMeta.dirty || submitCount.value > 0 ? deadlineError.value : undefined,
)

watch(isOpen, (open) => {
  if (!open) return
  resetForm({ values: getInitialValues() })
  resetCreate()
  resetUpdate()
})

const { mutateAsync: create, error: createError, reset: resetCreate } = useCreateGoal()
const { mutateAsync: update, error: updateError, reset: resetUpdate } = useUpdateGoal()
const serverError = computed(() => createError.value ?? updateError.value)

const onSubmit = handleSubmit(async (values) => {
  try {
    const gr: GoalRequest = {
      title: values.title,
      description: values.description,
      deadline: toIso(values.deadline),
    }
    if (props.goal) await update({ id: props.goal.id, gr })
    else await create(gr)
    close()
  } catch {}
})
</script>

<template>
  <Teleport to="body">
    <BaseDialog v-model="isOpen">
      <form
        class="flex flex-col gap-3 rounded-xl border border-input-border bg-element-bg p-4"
        @submit="onSubmit"
      >
        <BaseInput v-model="title" placeholder="Title" />
        <span v-if="showTitleError" class="text-sm text-error">{{ showTitleError }}</span>

        <textarea
          v-model="description"
          placeholder="Description"
          class="w-full rounded-lg border border-input-border bg-element-bg px-3 py-1 text-m text-element-text placeholder:text-text-muted transition-colors duration-150 focus:border-accent focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-accent"
        />

        <BaseInput v-model="deadline" type="datetime-local" />
        <span v-if="showDeadlineError" class="text-sm text-error">{{ showDeadlineError }}</span>

        <p v-if="serverError" class="text-sm text-error">{{ serverError.message }}</p>

        <div class="flex gap-2">
          <BaseButton type="submit" :disabled="isSubmitting">
            {{ isSubmitting ? 'Saving…' : 'Save' }}
          </BaseButton>

          <BaseButton variant="ghost" @click="close()">Cancel</BaseButton>
        </div>
      </form>
    </BaseDialog>
  </Teleport>
</template>
