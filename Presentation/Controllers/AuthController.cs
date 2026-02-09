using Application.Interfaces.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Cookies;
using Presentation.DTO;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _as;

        public AuthController(IAuthService @as)
        {
            _as = @as;
        }

        [HttpPost("reg")]
        public async Task<IActionResult> Register([FromBody] UserRequest userRequest, CancellationToken ct)
        {
            var result = await _as.RegisterAsync(userRequest.Login, userRequest.Password, ct);
            return Created();
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserRequest userRequest, CancellationToken ct)
        {
            var token = await _as.LoginAsync(userRequest.Login, userRequest.Password, ct);
            if (string.IsNullOrEmpty(token)) return Unauthorized(new { error = "Invalid credentials." });
            Response.SetAuthCookie(token); 
            return Ok();
        }
        [Authorize]
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.DeleteAuthCookie();
            return Ok();
        }
    }
}
