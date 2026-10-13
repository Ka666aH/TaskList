namespace Application.DTO;

public record PageResult<T>(IList<T> Items, int Total);