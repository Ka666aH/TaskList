import { queryClient } from '@/lib/queryClient'
import { meQueryOptions } from '@/queries/auth'
import { createRouter, createWebHistory } from 'vue-router'

const routes = [
  {
    path: '/login',
    name: 'login',
    component: () => import('@/pages/Login.vue'),
    meta: { guestOnly: true },
  },
  {
    path: '/reg',
    name: 'reg',
    component: () => import('@/pages/Register.vue'),
    meta: { guestOnly: true },
  },
  {
    path: '/',
    name: 'tasks',
    component: () => import('@/pages/Tasks.vue'),
    meta: { requiresAuth: true },
  },
  {
    path: '/account',
    name: 'account',
    component: () => import('@/pages/Account.vue'),
    meta: { requiresAuth: true },
  },
  {
    path: '/admin',
    name: 'admin',
    component: () => import('@/pages/Admin.vue'),
    meta: { adminOnly: true },
  },
]

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes,
})

router.beforeEach(async (to) => {
  let me = null
  try {
    me = await queryClient.query({ ...meQueryOptions, staleTime: 'static' })
  } catch {
    me = null
  }

  const isAuthed = !!me
  const isAdmin = me?.role === 'Admin'

  if (to.meta.guestOnly && isAuthed) return { name: 'tasks' }
  if (to.meta.requiresAuth && !isAuthed) return { name: 'login', query: { redirect: to.fullPath } }
  if (to.meta.adminOnly && !isAdmin) return { name: 'tasks' }
  return true
})

export default router
