using System;
using System.Collections.Generic;

namespace DevHabit.Api.DTOs;

public sealed record TagsCollectionDto(List<TagDto> Data);

public sealed record TagDto
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }
    public DateTime CreatedAtUtc { get; init; }
    public DateTime? UpdatedAtUtc { get; init; }
}
