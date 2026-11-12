import { describe, expect, test } from 'vitest'
import { toDisplayDate, toIso, toLocalInput } from './date'

const iso = '2000-01-01T12:00:00.000Z'
// формат en-US (интерфейс английский), время московское (+3) — зона
// зафиксирована в vitest.setup.ts
const local = '1/1/2000, 3:00:00 PM'
const localInput = '2000-01-01T15:00'

describe('date', () => {
  test('toDisplayDate', () => {
    expect(toDisplayDate(iso)).toBe(local)
  })
  test('toLocalInput', () => {
    expect(toLocalInput(iso)).toBe(localInput)
  })
  test('toISO', () => {
    expect(toIso(localInput)).toBe(iso)
  })
})
