import { fileURLToPath, URL } from 'node:url'
import { configDefaults, defineConfig } from 'vitest/config'
import vue from '@vitejs/plugin-vue'

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
