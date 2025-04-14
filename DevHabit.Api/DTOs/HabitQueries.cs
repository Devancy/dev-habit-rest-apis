using DevHabit.Api.Database.Entities;
using System;
using System.Linq.Expressions;

namespace DevHabit.Api.DTOs;

internal static class HabitQueries
{
    public static Expression<Func<Habit, HabitDto>> ProjectToDto()
    {
        return habit => new HabitDto
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
}
