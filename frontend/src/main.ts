import App from './App.vue'
import { createApp } from 'vue'
import components from '@/components/UI'
import { createPinia } from 'pinia'
import router from './router'
import { VueQueryPlugin } from '@tanstack/vue-query'
import { queryClient } from '@/lib/queryClient'
import { setUnauthorizedHandler } from './api/http'
import { meQueryOptions } from './queries/auth'

import './assets/styles/main.css'

const app = createApp(App)

Object.entries(components).forEach(([name, component]) => app.component(name, component))

app.use(createPinia())
app.use(router)
app.use(VueQueryPlugin, { queryClient })

setUnauthorizedHandler(() => {
  queryClient.setQueryData(meQueryOptions.queryKey, null)
  const cur = router.currentRoute.value
  if (cur.name !== 'login') {
    router.push({ name: 'login', query: { redirect: cur.fullPath } })
  }
})

app.mount('#app')
