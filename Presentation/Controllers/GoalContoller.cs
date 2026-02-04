using Application.Interfaces.ServiceInterfaces;
using Domain;
using Infrastructure.Token;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.DTO;
using Presentation.Mappers;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("goals")]
    public class GoalContoller : ControllerBase
    {
        private readonly IGoalControlService _gcs;
        
        public GoalContoller(IGoalControlService gcs)
        {
            _gcs = gcs;
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetGoals(CancellationToken ct)
        {
            var login = User.FindFirst(Claims.Login)?.Value;
            if (login == null) throw new UnauthorizedAccessException("Login claim missing.");

            var user = await _gcs.GetUserAsync(login, ct);
            return Ok(GoalMapper.ToResponseList(user.Goals));
        }
        [Authorize]
        [HttpGet("{goalId}")]
        public async Task<IActionResult> GetGoal(Guid goalId, CancellationToken ct)
        {
            var login = User.FindFirst(Claims.Login)?.Value;
            if (login == null) throw new UnauthorizedAccessException("Login claim missing.");

            var user = await _gcs.GetUserAsync(login, ct);
            var goal = user.Goals.FirstOrDefault(g => g.Id == goalId);
            if (goal == null) return NotFound("Goal not found.");

            return Ok(GoalMapper.ToResponse(goal));
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddGoal([FromBody] GoalRequest request, CancellationToken ct)
        {
            var login = User.FindFirst(Claims.Login)?.Value;
            if (login == null) throw new UnauthorizedAccessException("Login claim missing.");

            var goal = GoalMapper.ToGoal(login, request);
            var result = await _gcs.AddGoalAsync(login, goal, ct);
            return result ? CreatedAtAction(nameof(GetGoal), new { goalId = goal.Id }, GoalMapper.ToResponse(goal)) : Problem();
        }
        [Authorize]
        [HttpDelete("{goalId}")]
        public async Task<IActionResult> RemoveGoal(Guid goalId, CancellationToken ct)
        {
            var login = User.FindFirst(Claims.Login)?.Value;
            if (login == null) throw new UnauthorizedAccessException("Login claim missing.");

            var user = await _gcs.GetUserTrackAsync(login, ct);
            var goal = user.Goals.FirstOrDefault(g => g.Id == goalId);
            if (goal == null) return NotFound("Goal not found.");

            var result = await _gcs.RemoveGoalAsync(login, goal, ct);
            return result ? NoContent() : Problem();
        }
        [Authorize]
        [HttpPut("{goalId}")]
        public async Task<IActionResult> UpdateGoal(Guid goalId, [FromBody] GoalRequest request, CancellationToken ct)
        {
            var login = User.FindFirst(Claims.Login)?.Value;
            if (login == null) throw new UnauthorizedAccessException("Login claim missing.");

            var user = await _gcs.GetUserTrackAsync(login, ct);
            var goal = GoalMapper.ToGoal(login, request);

            var result = await _gcs.UpdateGoalAsync(login, goalId, goal, ct);
            return result ? Ok(GoalMapper.ToResponse(goal)) : Problem();
        }
    }
}