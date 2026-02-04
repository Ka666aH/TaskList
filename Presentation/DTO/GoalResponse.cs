namespace Presentation.DTO
{
    public record GoalResponse(string Title, string? Description, DateTime CreateAt, DateTime? Deadline);
}
