using DevHabit.Api.Database.Entities;
using Microsoft.OpenApi.Expressions;
using System;

namespace DevHabit.Api.DTOs;

internal static class HabitMappings
{
    public static Habit ToEntity(this CreateHabitDto dto) =>
        new()
        {
            Id = $"h_{Guid.CreateVersion7()}",
            Name = dto.Name,
            Description = dto.Description,
            HabitType = dto.HabitType,
            Frequency = new Frequency
            {
                Type = dto.Frequency.Type,
                TimePerPeriod = dto.Frequency.TimePerPeriod
            },
            Target = new Target
            {
                Value = dto.Target.Value,
                Unit = dto.Target.Unit
            },
            Status = HabitStatus.Ongoing,
            IsArchived = false,
            EndDate = dto.EndDate,
            MileStone = dto.MileStone is null ? null : new MileStone
            {
                Target = dto.MileStone.Target,
                Current = dto.MileStone.Current
            },
            CreatedAtUtc = DateTime.UtcNow,
            // UpdatedAtUtc = null,
            // LastCompletedAtUtc = null
        };

    public static HabitDto ToDto(this Habit habit) =>
        new()
        {
            Id = habit.Id,
            Name = habit.Name,
            Description = habit.Description,
            HabitType = habit.HabitType,
            Frequency = new FrequencyDto
            {
                Type = habit.Frequency.Type,
                TimePerPeriod = habit.Frequency.TimePerPeriod
            },
            Target = new TargetDto
            {
                Value = habit.Target.Value,
                Unit = habit.Target.Unit
            },
            Status = habit.Status,
            IsArchived = habit.IsArchived,
            EndDate = habit.EndDate,
            MileStone = habit.MileStone == null ? null : new MileStoneDto
            {
                Target = habit.MileStone.Target,
                Current = habit.MileStone.Current
            },
            CreatedAtUtc = habit.CreatedAtUtc,
            UpdatedAtUtc = habit.UpdatedAtUtc,
            LastCompletedAtUtc = habit.LastCompletedAtUtc
        };
}
