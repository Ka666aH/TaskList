using Application.Interfaces.ServiceInterfaces;
using Infrastructure.Token;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.DTO;
using Presentation.Options;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("account")]
    [Authorize(Policy = Policies.RequireLogin)]
    public class UserController : ControllerBase
    {
        private readonly IUserControlService _ucs;

        public UserController(IUserControlService ucs)
        {
            _ucs = ucs;
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteAccount(CancellationToken ct)
        {
            var login = User.FindFirst(Claims.Login)!.Value;
            var result = await _ucs.DeleteAccountAsync(login, ct);
            if (result) return NoContent();
            else return Problem();
        }
        
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