namespace DevHabit.Api.DTOs;

public sealed class UpdateTagDto
{
    public required string Name { get; init; }
    public string? Description { get; init; }
}