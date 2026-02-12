namespace Presentation.DTO
{
    public record ExceptionResponse(string Code, string Message, Exception? InnerException = null);
}
