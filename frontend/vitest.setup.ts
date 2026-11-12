import { vi } from 'vitest'

// Тесты даты держат ожидания в московском времени (+3), а CI-раннер живёт в
// UTC. Фиксируем зону до импорта тестов — setupFiles выполняются в каждом
// воркере раньше тестовых модулей, поэтому getHours()/toISOString() детерминированы.
process.env.TZ = 'Europe/Moscow'

// jsdom не реализует нативный <dialog>: у элемента нет showModal/close.
// В тестах нативное поведение модалки не проверяется — подменяем методы заглушками,
// чтобы BaseDialog не падал в onMounted/watch.
HTMLDialogElement.prototype.showModal ??= vi.fn()
HTMLDialogElement.prototype.close ??= vi.fn()
