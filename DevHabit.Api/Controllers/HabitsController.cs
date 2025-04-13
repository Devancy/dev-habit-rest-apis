using DevHabit.Api.Database;
using DevHabit.Api.Database.Entities;
using DevHabit.Api.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevHabit.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class HabitsController(ApplicationDbContext dbContext) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<HabitsCollectionDto>> GetHabits()
    {
        List<HabitDto> habits = await dbContext
        .Habits
        .Select(h => new HabitDto
        {
            Id = h.Id,
            Name = h.Name,
            Description = h.Description,
            HabitType = h.HabitType,
            Frequency = new FrequencyDto
            {
                Type = h.Frequency.Type,
                TimePerPeriod = h.Frequency.TimePerPeriod
            },
            Target = new TargetDto
            {
                Value = h.Target.Value,
                Unit = h.Target.Unit
            },
            Status = h.Status,
            IsArchived = h.IsArchived,
            EndDate = h.EndDate,
            MileStone = h.MileStone == null ? null : new MileStoneDto
            {
                Target = h.MileStone.Target,
                Current = h.MileStone.Current
            },
            CreatedAtUtc = h.CreatedAtUtc,
            UpdatedAtUtc = h.UpdatedAtUtc,
            LastCompletedAtUtc = h.LastCompletedAtUtc
        })
        .ToListAsync();

        HabitsCollectionDto habitsCollection = new()
        {
            Data = habits
        };
        return Ok(habitsCollection);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<HabitDto>> GetHabit(string id)
    {
        HabitDto? habit = await dbContext
            .Habits
            .Select(h => new HabitDto
            {
                Id = h.Id,
                Name = h.Name,
                Description = h.Description,
                HabitType = h.HabitType,
                Frequency = new FrequencyDto
                {
                    Type = h.Frequency.Type,
                    TimePerPeriod = h.Frequency.TimePerPeriod
                },
                Target = new TargetDto
                {
                    Value = h.Target.Value,
                    Unit = h.Target.Unit
                },
                Status = h.Status,
                IsArchived = h.IsArchived,
                EndDate = h.EndDate,
                MileStone = h.MileStone == null ? null : new MileStoneDto
                {
                    Target = h.MileStone.Target,
                    Current = h.MileStone.Current
                },
                CreatedAtUtc = h.CreatedAtUtc,
                UpdatedAtUtc = h.UpdatedAtUtc,
                LastCompletedAtUtc = h.LastCompletedAtUtc
            })
            .FirstOrDefaultAsync(h => h.Id == id);

        if (habit == null)
        {
            return NotFound();
        }

        return Ok(habit);
    }
}