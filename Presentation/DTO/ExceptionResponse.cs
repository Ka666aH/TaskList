namespace Presentation.DTO
{
    public record ExceptionResponse(string Code, string Message, string? InnerMessage = null);
}
