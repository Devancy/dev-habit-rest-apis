using DevHabit.Api.Database.Entities;
using System;

namespace DevHabit.Api.DTOs;

public sealed record CreateHabitDto
{
    public required string Name { get; init; }
    public string? Description { get; init; }
    public required HabitType HabitType { get; init; }
    public required FrequencyDto Frequency { get; init; }
    public required TargetDto Target { get; init; }
    public DateOnly? EndDate { get; init; }
    public MileStoneDto? MileStone { get; init; }
};