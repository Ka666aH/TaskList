using Application.Interfaces.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("user")]
    public class UserController : ControllerBase
    {
        private readonly IUserControlService _ucs;

        public UserController(IUserControlService ucs)
        {
            _ucs = ucs;
        }
        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> DeleteAccount(CancellationToken ct)
        {
            var login = User.FindFirst("login")?.Value;
            if (login == null) throw new UnauthorizedAccessException("Login claim missing.");
            var result = await _ucs.DeleteAccountAsync(login, ct);
            if (result) return NoContent();
            else return Problem();
        }
        [Authorize]
        [HttpPatch("password")]
        public async Task<IActionResult> ChangePassword([FromBody]string newPassword, CancellationToken ct)
        {
            var login = User.FindFirst("login")?.Value;
            if (login == null) throw new UnauthorizedAccessException("Login claim missing.");

            var result = await _ucs.ChangePasswordAsync(login, newPassword, ct);
            if (result) return Ok();
            else return Problem();
        }
    }
}
