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
]

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes,
})

export default router
