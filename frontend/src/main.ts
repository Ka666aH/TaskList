import { createApp } from 'vue'
import { createPinia } from 'pinia'
import App from './App.vue'
import router from './router'
import components from '@/components/UI'
import { useSession } from './stores/session.ts'
import { setUnauthorizedHandler } from './api/http.ts'

const app = createApp(App)

Object.entries(components).forEach(([name, component]) => {
    app.component(name, component)
})

app.use(createPinia())

const session = useSession()
setUnauthorizedHandler(() => {
    session.reset('expired')
    router.push({ name: 'login', query: { redirect: router.currentRoute.value.fullPath } })
})

app.use(router)

router.isReady().then(() => app.mount('#app'))
