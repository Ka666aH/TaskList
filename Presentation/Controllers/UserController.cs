using Application.Interfaces.ServiceInterfaces;
using Domain;
using Infrastructure.Token;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.DTO;
using Presentation.Options;
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
        [Authorize(Policy = Policies.RequireLogin)]
        [HttpDelete]
        public async Task<IActionResult> DeleteAccount(CancellationToken ct)
        {
            var login = User.FindFirst(Claims.Login)!.Value;
            var result = await _ucs.DeleteAccountAsync(login, ct);
            if (result) return NoContent();
            else return Problem();
        }
        [Authorize(Policy = Policies.RequireLogin)]
        [HttpPatch("password")]
        public async Task<IActionResult> ChangePassword([FromBody]ChangePasswordRequest request, CancellationToken ct)
        {
            var login = User.FindFirst(Claims.Login)!.Value;

            var result = await _ucs.ChangePasswordAsync(login, request.NewPassword, ct);
            if (result) return Ok();
            else return Problem();
        }
    }
}
