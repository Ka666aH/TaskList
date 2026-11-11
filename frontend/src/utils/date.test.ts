import { describe, expect, test } from 'vitest'
import { toDisplayDate, toIso, toLocalInput } from './date'

const iso = '2000-01-01T12:00:00.000Z'
const local = '01.01.2000, 15:00:00'
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
