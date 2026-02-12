using Application.Interfaces.ServiceInterfaces;
using Domain.Exceptions;
using Infrastructure.Token;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.DTO;
using Presentation.Mappers;
using Presentation.Options;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("goals")]
    [Authorize(Policy = Policies.RequireLogin)]
    public class GoalController : ControllerBase
    {
        private readonly IGoalControlService _gcs;
        
        public GoalController(IGoalControlService gcs)
        {
            _gcs = gcs;
        }
        [HttpGet]
        public async Task<IActionResult> GetGoals(CancellationToken ct)
        {
            var login = User.FindFirst(Claims.Login)!.Value;

            var user = await _gcs.GetUserAsync(login, ct);
            return Ok(GoalMapper.ToResponseList(user.Goals));
        }
        [HttpGet("{goalId}")]
        public async Task<IActionResult> GetGoal(Guid goalId, CancellationToken ct)
        {
            var login = User.FindFirst(Claims.Login)!.Value;

            var user = await _gcs.GetUserAsync(login, ct);
            var goal = user.Goals.FirstOrDefault(g => g.Id == goalId);
            if (goal == null) throw new GoalNotFoundException();

            return Ok(GoalMapper.ToResponse(goal));
        }
        [HttpPost]
        public async Task<IActionResult> AddGoal([FromBody] GoalRequest request, CancellationToken ct)
        {
            var login = User.FindFirst(Claims.Login)!.Value;

            var goal = GoalMapper.ToGoal(login, request);
            var result = await _gcs.AddGoalAsync(login, goal, ct);
            return result ? CreatedAtAction(nameof(GetGoal), new { goalId = goal.Id }, GoalMapper.ToResponse(goal)) : Problem();
        }
        [HttpDelete("{goalId}")]
        public async Task<IActionResult> RemoveGoal(Guid goalId, CancellationToken ct)
        {
            var login = User.FindFirst(Claims.Login)!.Value;

            var user = await _gcs.GetUserTrackAsync(login, ct);
            var goal = user.Goals.FirstOrDefault(g => g.Id == goalId);
            if (goal == null) throw new GoalNotFoundException();

            var result = await _gcs.RemoveGoalAsync(login, goal, ct);
            return result ? NoContent() : Problem();
        }
        
        [HttpPut("{goalId}")]
        public async Task<IActionResult> UpdateGoal(Guid goalId, [FromBody] GoalRequest request, CancellationToken ct)
        {
            var login = User.FindFirst(Claims.Login)!.Value;

            var user = await _gcs.GetUserTrackAsync(login, ct);
            var goal = GoalMapper.ToGoal(login, request);

            var result = await _gcs.UpdateGoalAsync(login, goalId, goal, ct);
            return result ? Ok(GoalMapper.ToResponse(goal)) : Problem();
        }
    }
}