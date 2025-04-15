using System;
using System.Linq.Expressions;
using DevHabit.Api.Database.Entities;

namespace DevHabit.Api.DTOs;

internal static class TagQueries
{
    public static Expression<Func<Tag, TagDto>> ProjectToDto() =>
        tag => new TagDto
        {
            Id = tag.Id,
            Name = tag.Name,
            Description = tag.Description,
            CreatedAtUtc = tag.CreatedAtUtc,
            UpdatedAtUtc = tag.UpdatedAtUtc
        };
}