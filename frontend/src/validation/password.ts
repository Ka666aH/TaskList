import { z } from 'zod'

export const PASSWORD_PATTERN = /^[A-Za-z0-9!@#$%^&*()_+\-=[\]{};':"\\|,.<>/?]*$/
export const PASSWORD_MIN_LENGTH = 4

export const passwordSchema = z
  .string()
  .min(PASSWORD_MIN_LENGTH, `Password must be longer or equal to ${PASSWORD_MIN_LENGTH} symbols`)
  .regex(PASSWORD_PATTERN, 'Password contains an invalid character')
