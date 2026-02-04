namespace Presentation.DTO
{
    public record GoalRequest(string Title, string? Description, DateTime CreateAt, DateTime? Deadline);
}
