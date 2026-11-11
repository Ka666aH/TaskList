import { describe, test, expect } from 'vitest'
import { retryUnlessAuthError } from './retry'
import type { ApiError } from '@/types/ApiError'

const apiError = (code: string): ApiError => ({ code, message: 'x', innerMessage: null })

describe('retryUnlessAuthError', () => {
  test('do not retry 401', () => {
    expect(retryUnlessAuthError(0, apiError('HTTP_401'))).toBe(false)
  })

  test('do not retry 403', () => {
    expect(retryUnlessAuthError(0, apiError('HTTP_403'))).toBe(false)
  })

  test('retry server errors two times', () => {
    expect(retryUnlessAuthError(0, apiError('HTTP_500'))).toBe(true)
    expect(retryUnlessAuthError(1, apiError('HTTP_500'))).toBe(true)
    expect(retryUnlessAuthError(2, apiError('HTTP_500'))).toBe(false)
  })
})
