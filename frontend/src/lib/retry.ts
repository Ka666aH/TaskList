import type { ApiError } from '@/types/ApiError';

// 401/403 отдаёт мидлвара пустым телом, поэтому код собирается на фронте (api/http.ts) как HTTP_401/HTTP_403.
// Повторять их бессмысленно: пока сессия не восстановлена, ответ не изменится.
// ApiError здесь как TError по умолчанию — задан в src/api/tanstack.d.ts (registerQueryDefault 'defaultError').
export const retryUnlessAuthError = (attempt: number, error: ApiError): boolean =>
  error.code === 'HTTP_401' || error.code === 'HTTP_403' ? false : attempt < 2;
