import { vi } from 'vitest'

// jsdom не реализует нативный <dialog>: у элемента нет showModal/close.
// В тестах нативное поведение модалки не проверяется — подменяем методы заглушками,
// чтобы BaseDialog не падал в onMounted/watch.
HTMLDialogElement.prototype.showModal ??= vi.fn()
HTMLDialogElement.prototype.close ??= vi.fn()
