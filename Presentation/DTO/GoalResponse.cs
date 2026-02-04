namespace Presentation.DTO
{
    public record GoalResponse(Guid Id, string Title, string? Description, DateTime CreateAt, DateTime? Deadline);
}
