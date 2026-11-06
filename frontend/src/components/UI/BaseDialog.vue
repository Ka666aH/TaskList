<script setup lang="ts">
import { onMounted, ref, watch } from 'vue';

const isOpen = defineModel<boolean>({ required: true });
const dialog = ref<HTMLDialogElement | null>(null)
const onClose = () => isOpen.value = false

function syncDialog() {
  if (!dialog.value) return
  if (isOpen.value) dialog.value.showModal()
  else dialog.value.close()
}
function onBackdropClick(e: MouseEvent) {
  if (e.target === dialog.value) onClose()
}

onMounted(syncDialog)
watch(isOpen, syncDialog)
</script>

<template>
  <dialog ref="dialog" @close="onClose" @click="onBackdropClick">
    <div>
      <slot />
    </div>
  </dialog>
</template>
