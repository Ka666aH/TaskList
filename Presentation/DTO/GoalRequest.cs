namespace Presentation.DTO
{
    public record GoalRequest(string Title, string? Description, DateTime? Deadline);
}
