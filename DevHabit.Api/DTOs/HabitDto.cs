using DevHabit.Api.Database.Entities;
using System;
using System.Collections.Generic;

namespace DevHabit.Api.DTOs;

public sealed record HabitsCollectionDto
{
    public required List<HabitDto> Data { get; init; }
}

public sealed record HabitDto
{
    public required string Id { get; init; }
    public required string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public required HabitType HabitType { get; init; }
    public required FrequencyDto Frequency { get; init; }
    public required TargetDto Target { get; init; }
    public required HabitStatus Status { get; init; }
    public required bool IsArchived { get; init; }
    public DateOnly? EndDate { get; init; }
    public MileStoneDto? MileStone { get; init; }
    public required DateTime CreatedAtUtc { get; init; }
    public DateTime? UpdatedAtUtc { get; init; }
    public DateTime? LastCompletedAtUtc { get; init; }
}

public sealed record FrequencyDto
{
    public required FrequencyType Type { get; init; }
    public required int TimePerPeriod { get; init; }
}

/// <summary>
/// Value object for validation
/// </summary>
public sealed record TargetDto
{
    public required int Value { get; init; }
    public required string Unit { get; init; }
}

public sealed record MileStoneDto
{
    /// <summary>
    /// Number of unit of work to archive
    /// </summary>
    public required int Target { get; init; }
    
    /// <summary>
    /// Track current status of the MileStone
    /// </summary>
    public required int Current { get; init; }
}