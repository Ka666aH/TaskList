import { describe, expect, test } from 'vitest'
import { passwordSchema } from './password'

describe('password', () => {
  test('pass valid password', () => {
    expect(passwordSchema.safeParse('aB12!').success).toBe(true)
  })
  test('reject short password', () => {
    expect(passwordSchema.safeParse('a').success).toBe(false)
  })
  test('reject empty password', () => {
    expect(passwordSchema.safeParse('').success).toBe(false)
  })
  test('reject whitespace password', () => {
    expect(passwordSchema.safeParse('         ').success).toBe(false)
  })
  test('reject cyrillic', () => {
    expect(passwordSchema.safeParse('абвг').success).toBe(false)
  })
})
