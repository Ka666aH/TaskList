using Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Presentation.Mappers;

namespace Presentation.Controllers
{
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("/exception")]
    public class ExceptionController : ControllerBase
    {
        [HttpGet, HttpPost, HttpPut, HttpDelete, HttpPatch]
        public IActionResult Handle()
        {
            var exception = HttpContext.Features.Get<IExceptionHandlerPathFeature>()?.Error;
            return MapException(exception);
        }
        private IActionResult MapException(Exception? ex)
        {
            if (ex is AppException appEx)
            {
                return appEx switch
                {
                    EnvException => StatusCode(500, appEx.ToResponse()),

                    UserNotFoundException or
                    GoalNotFoundException or
                    RoleNotFoundException =>
                        StatusCode(404, appEx.ToResponse()),

                    UserEmptyLoginException or
                    UserEmptyHashedPasswordException or
                    GoalEmptyTitleException or
                    DeadlineExpiredException or
                    PageSizeException or
                    PageNumberException =>
                        BadRequest(appEx.ToResponse()),

                    ChangeDefaultAdminRoleException or
                    DeleteDefaultAdminException =>
                        StatusCode(403, appEx.ToResponse()),

                    IncorrectPasswordException =>
                        Unauthorized(appEx.ToResponse()),

                    LoginExistException =>
                        Conflict(appEx.ToResponse()),

                    ChangeRoleException =>
                        StatusCode(500, appEx.ToResponse()),

                    _ => StatusCode(500, appEx.ToResponse())
                };
            }
            return ex != null ?
                StatusCode(500, ex.ToResponse()) :
                Problem(statusCode: 500, title: "Server error");
        }
    }
}