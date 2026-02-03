using Application.Interfaces.ServiceInterfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Presentation.DTO;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
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
            var result = await _as.RegisterAsync(userRequest.login, userRequest.password, ct);
            return Created();
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserRequest userRequest, CancellationToken ct)
        {
            var token = await _as.LoginAsync(userRequest.login, userRequest.password, ct);
            if (string.IsNullOrEmpty(token)) return Unauthorized(new { error = "Invalid credentials." });
            Response.Cookies.Append("jwt", token);
            return Ok();
        }
    }
}
