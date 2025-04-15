namespace DevHabit.Api.DTOs;

public sealed record CreateTagDto
{
    public required string Name { get; init; }
    public string? Description { get; init; }
}
