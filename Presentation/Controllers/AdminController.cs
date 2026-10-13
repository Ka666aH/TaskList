using Application.DTO;
using Application.Interfaces.ServiceInterfaces;
using Domain.Constants;
using Domain.Exceptions;
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
        public async Task<IActionResult> ChangeRole([FromRoute]string userLogin, [FromBody] ChangeRoleRequest request, CancellationToken ct)
        {
            var input = Enum.TryParse<RoleType>(request.NewRole, out var role);
            if (!input) throw new RoleNotFoundException();

            var result = await _ucs.ChangeRoleAsync(userLogin, role, ct);
            if (!result) throw new ChangeRoleException();
            return Ok();
        }
        [HttpGet("users/amount")]
        public async Task<IActionResult> GetUsersAmount(CancellationToken ct)
        {
            var amount = await _rp.GetUsersAmountAsync(ct);
            return Ok(new UsersAmountResponse(amount));
        }
        [HttpGet("users")]
        public async Task<IActionResult> GetUsersPage([FromQuery] int pageSize, [FromQuery] int page, CancellationToken ct)
        {
            var pageResult = await _rp.GetUsersPageAsync(pageSize, page, ct);
            return Ok(new PageResult<UserResponse>(UserMapper.ToResponseList(pageResult.Items), pageResult.Total));
        }
        [HttpGet("goals/amount")]
        public async Task<IActionResult> GetGoalsAmount(CancellationToken ct)
        {
            var amount = await _rp.GetGoalsAmountAsync(ct);
            return Ok(new GoalsAmountResponse(amount));
        }
        [HttpGet("/goals/amount/{userLogin}")]
        public async Task<IActionResult> GetUserGoalsAmount([FromRoute]string userLogin, CancellationToken ct)
        {
            var amount = await _rp.GetUserGoalsAmountAsync(userLogin, ct);
            return Ok(new GoalsAmountResponse(amount));
        }
    }
}