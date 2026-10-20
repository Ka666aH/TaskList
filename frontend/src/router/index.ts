import { useSession } from '@/stores/session'
import { createRouter, createWebHistory } from 'vue-router'

const routes = [
  { path: '/login', name: 'login', component: () => import('@/pages/Login.vue'), meta: { guestOnly: true } },
  { path: '/reg', name: 'reg', component: () => import('@/pages/Register.vue'), meta: { guestOnly: true } },
  { path: '/', name: 'tasks', component: () => import('@/pages/Tasks.vue'), meta: { requiresAuth: true } },
  { path: '/account', name: 'account', component: () => import('@/pages/Account.vue'), meta: { requiresAuth: true } },
  { path: '/admin', name: 'admin', component: () => import('@/pages/Admin.vue'), meta: { requiresAuth: true, adminOnly: true } },
  { path: '/:pathMatch(.*)*', name: 'notFound', component: () => import('@/pages/NotFound.vue') },
]

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes,
})

router.beforeEach(async (to) => {
  const session = useSession()
  await session.init()

  if (to.meta.guestOnly && session.isAuthed) { return { name: 'tasks' } }
  if (to.meta.requiresAuth && !session.isAuthed) { return { name: 'login', query: { redirect: to.fullPath } } }
  if (to.meta.adminOnly && !session.isAdmin) { return { name: 'tasks' } }

  return true
})

export default router