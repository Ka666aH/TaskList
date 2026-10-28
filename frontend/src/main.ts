import { createApp } from 'vue'
import { createPinia } from 'pinia'
import { QueryClient, VueQueryPlugin } from '@tanstack/vue-query'

import components from '@/components/UI'

import App from './App.vue'
import router from './router'

import './assets/styles/main.css'

const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      staleTime: 30_000,
    },
  },
})

const app = createApp(App)

Object.entries(components).forEach(([name, component]) => {
  app.component(name, component)
})

app.use(createPinia())
app.use(router)
app.use(VueQueryPlugin, { queryClient })

app.mount('#app')
