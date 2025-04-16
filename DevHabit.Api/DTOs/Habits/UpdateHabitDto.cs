using System;
using DevHabit.Api.Database.Entities;

namespace DevHabit.Api.DTOs.Habits;

public class UpdateHabitDto
{
    public required string Name { get; init; }
    public string? Description { get; init; }
    public required HabitType HabitType { get; init; }
    public required FrequencyDto Frequency { get; init; }
    public required TargetDto Target { get; init; }
    public DateOnly? EndDate { get; init; }
    public UpdateMileStoneDto? MileStone { get; init; }
}

public sealed record UpdateMileStoneDto
{
    /// <summary>
    /// Number of unit of work to archive
    /// </summary>
    public required int Target { get; init; }
}