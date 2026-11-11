import { cleanupE2eUsers } from './cleanup'

// до тестов: убираем мусор от прошлых падений
export default async function globalSetup() {
  await cleanupE2eUsers()
}
