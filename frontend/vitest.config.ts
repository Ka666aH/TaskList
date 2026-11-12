import { fileURLToPath, URL } from 'node:url'
import { configDefaults, defineConfig } from 'vitest/config'
import vue from '@vitejs/plugin-vue'

// Тесты даты держат ожидания в московском времени (+3), а CI-раннер живёт в
// UTC. Задавать TZ в setup-файле поздно: воркеры пула vmThreads снапшотят
// переменные окружения при своём старте, и запись изнутри воркера до ICU не
// доходит. Конфиг исполняется в главном процессе ДО спавна воркеров — они
// наследуют уже выставленную зону.
process.env.TZ = 'Europe/Moscow'

export default defineConfig({
  plugins: [vue()],
  resolve: {
    alias: {
      '@': fileURLToPath(new URL('./src', import.meta.url)),
    },
  },
  test: {
    environment: 'jsdom',
    setupFiles: ['./vitest.setup.ts'],
    pool: 'vmThreads',
    // e2e/*.spec.ts — файлы Playwright, Vitest их запускать не должен
    // (иначе playwright-core падает в jsdom: «TransformStream is not defined»)
    exclude: [...configDefaults.exclude, 'e2e/**'],
  },
})
