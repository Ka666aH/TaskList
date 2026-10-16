export interface ApiError{
    code: string,
    message: string,
    innerMessage: string | null
}