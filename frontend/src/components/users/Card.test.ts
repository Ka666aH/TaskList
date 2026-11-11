import { describe, test, expect, vi, afterEach } from 'vitest'
import { mount, enableAutoUnmount } from '@vue/test-utils'
import { VueQueryPlugin, QueryClient } from '@tanstack/vue-query'
import Card from './Card.vue'
import BaseButton from '@/components/UI/BaseButton.vue'

vi.mock('@/api/admin', () => ({
  adminApi: {
    getGoalsAmountByUser: vi.fn().mockResolvedValue({ amount: 3 }),
  },
}))

const mountCard = (login = 'alice') =>
  mount(Card, {
    props: { user: { login } },
    global: {
      // глобальная регистрация из main.ts в тестах не существует — передаём руками
      components: { BaseButton },
      plugins: [[VueQueryPlugin, { queryClient: new QueryClient() }]],
    },
  })

enableAutoUnmount(afterEach)

describe('users/Card', () => {
  test('shows users login', () => {
    const wrapper = mountCard('bob')
    expect(wrapper.text()).toContain('bob')
  })

  test('button click emits askRole', async () => {
    const wrapper = mountCard()
    const admin = wrapper.findAll('button').find((b) => b.text() === 'Admin')
    expect(admin).toBeDefined()
    await admin!.trigger('click')
    expect(wrapper.emitted('askRole')?.[0]).toEqual(['alice', 'Admin'])
  })

  test('goals open amount', async () => {
    const wrapper = mountCard()
    expect(wrapper.text()).not.toContain('Goals:')

    const goals = wrapper.findAll('button').find((b) => b.text() === 'Goals')
    await goals!.trigger('click')

    await vi.waitFor(() => expect(wrapper.text()).toContain('Goals: 3'))
  })
})
