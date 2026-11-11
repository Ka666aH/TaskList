import { enableAutoUnmount, mount } from '@vue/test-utils'
import { afterEach, describe, expect, test, vi } from 'vitest'
import ConfirmDialog from './ConfirmDialog.vue'
import BaseButton from './BaseButton.vue'

const mountDialog = (message: string) => {
  const onUpdate = vi.fn()
  const wrapper = mount(ConfirmDialog, {
    props: {
      message,
      modelValue: true,
      'onUpdate:modelValue': onUpdate,
    },
    global: {
      // Teleport рендерит в body, а wrapper.text() видит только своё дерево —
      // заглушка рендерит контент на месте.
      components: { BaseButton },
      stubs: { teleport: true },
    },
  })
  return { wrapper, onUpdate }
}

enableAutoUnmount(afterEach)

describe('ConfirmDialog', () => {
  test('shows message', () => {
    const { wrapper } = mountDialog('test')
    expect(wrapper.text()).toContain('test')
  })

  test('cancel click emits update:modelValue false', async () => {
    const { wrapper, onUpdate } = mountDialog('test')
    const cancel = wrapper.findAll('button').find((b) => b.text() === 'Cancel')
    expect(cancel).toBeDefined()
    await cancel!.trigger('click')
    expect(onUpdate).toHaveBeenCalledWith(false)
  })

  test('confirm click emits confirm', async () => {
    const { wrapper } = mountDialog('test')
    const confirm = wrapper.findAll('button').find((b) => b.text() === 'Confirm')
    expect(confirm).toBeDefined()
    await confirm!.trigger('click')
    expect(wrapper.emitted('confirm')).toHaveLength(1)
  })
})
