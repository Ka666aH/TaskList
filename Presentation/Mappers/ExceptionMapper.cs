using Domain.Exceptions;
using Presentation.DTO;

namespace Presentation.Mappers
{
    public static class ExceptionMapper
    {
        private const string DefaultExceptionCode = "UNKNOWN_EXCEPTION";
        public static ExceptionResponse ToResponse(this AppException appEx) =>
            new(appEx.Code, appEx.Message, appEx.InnerException);
        public static ExceptionResponse ToResponse(this Exception ex) =>
            new(DefaultExceptionCode, ex.Message, ex.InnerException);
    }
}