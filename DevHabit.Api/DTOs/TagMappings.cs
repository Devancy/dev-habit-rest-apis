using System;
using DevHabit.Api.Database.Entities;

namespace DevHabit.Api.DTOs;

internal static class TagMappings
{
    public static TagDto ToDto(this Tag tag) => new TagDto
    {
        Id = tag.Id,
        Name = tag.Name,
        Description = tag.Description,
        CreatedAtUtc = tag.CreatedAtUtc,
        UpdatedAtUtc = tag.UpdatedAtUtc
    };

    public static Tag ToEntity(this CreateTagDto createTagDto) => new()
    {
        Id = $"t_{Guid.CreateVersion7()}",
        Name = createTagDto.Name,
        Description = createTagDto.Description,
        CreatedAtUtc = DateTime.UtcNow,
    };

    public static void UpdateFromDto(this Tag tag, UpdateTagDto updateTagDto)
    {
        tag.Name = updateTagDto.Name;
        tag.Description = updateTagDto.Description;
        tag.UpdatedAtUtc = DateTime.UtcNow;
    }
}