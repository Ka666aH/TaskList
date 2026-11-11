import { cleanupE2eUsers } from './cleanup'

// после тестов: выполняется даже при падении — подчищаем за собой
export default async function globalTeardown() {
  await cleanupE2eUsers()
}
