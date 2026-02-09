using Application.Interfaces.ServiceInterfaces;
using Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.DTO;
using Presentation.Mappers;
using Presentation.Options;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("admin")]
    [Authorize(Policy = Policies.RequireAdminAccess)]
    public class AdminController : ControllerBase
    {
        private readonly IUserControlService _ucs;
        private readonly IReportService _rp;

        public AdminController(IUserControlService ucs, IReportService rp)
        {
            _ucs = ucs;
            _rp = rp;
        }
        [HttpPatch("users/{userLogin}/role")]
        public async Task<IActionResult> ChangeRole(string userLogin, [FromBody] ChangeRoleRequest request, CancellationToken ct)
        {
            var input = Enum.TryParse<RoleType>(request.NewRole, out var role);
            if (!input) throw new ArgumentException("Role not found.");

            var result = await _ucs.ChangeRoleAsync(userLogin, role, ct);
            if (!result) return Problem("Cannot change role.");
            return Ok();
        }
        [HttpGet("/users/amount")]
        public async Task<IActionResult> GetUsersAmount(CancellationToken ct)
        {
            var amount = await _rp.GetUsersAmountAsync(ct);
            return Ok(new UsersAmountResponse(amount));
        }
        [HttpGet("/users")]
        public async Task<IActionResult> GetUsersPage([FromQuery]int pageSize, [FromQuery]int page, CancellationToken ct)
        {
            var users = await _rp.GetUsersPageAsync(pageSize, page, ct);
            return Ok(UserMapper.ToResponseList(users));
        }
        [HttpGet("/goals/amount")]
        public async Task<IActionResult> GetGoalsAmount(CancellationToken ct)
        {
            var amount = await _rp.GetGoalsAmountAsync(ct);
            return Ok(new GoalsAmountResponse(amount));
        }
        [HttpGet("/users/{userLoign}/goals/amount")]
        public async Task<IActionResult> GetUserGoalsAmount(string userLoign, CancellationToken ct)
        {
            var amount = await _rp.GetUserGoalsAmountAsync(userLoign, ct);
            return Ok(new GoalsAmountResponse(amount));
        }
    }
}