using DevHabit.Api.Database;
using DevHabit.Api.Database.Entities;
using DevHabit.Api.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;

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
        .Select(HabitQueries.ProjectToDto())
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
            .Select(HabitQueries.ProjectToDto())
            .FirstOrDefaultAsync(h => h.Id == id);

        if (habit == null)
        {
            return NotFound();
        }

        return Ok(habit);
    }

    [HttpPost]
    public async Task<ActionResult<HabitDto>> CreateHabit([FromBody] CreateHabitDto? createHabitDto)
    {
        if (createHabitDto == null)
        {
            return BadRequest("Habit data is required.");
        }

        Habit habit = createHabitDto.ToEntity();

        dbContext.Habits.Add(habit);
        await dbContext.SaveChangesAsync();

        HabitDto habitDto = habit.ToDto();

        return CreatedAtAction(nameof(GetHabit), new { id = habitDto.Id }, habitDto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<HabitDto>> UpdateHabit(string id, [FromBody] UpdateHabitDto? updateHabitDto)
    {
        if (updateHabitDto is null)
        {
            return BadRequest("Habit data is required.");
        }

        Habit? habit = await dbContext.Habits.FindAsync(id);

        if (habit is null)
        {
            return NotFound();
        }

        habit.UpdateFromDto(updateHabitDto);
        await dbContext.SaveChangesAsync();

        HabitDto habitDto = habit.ToDto();

        return Ok(habitDto);
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult> PathHabit(string id, [FromBody] JsonPatchDocument<HabitDto> patchDoc)
    {
        Habit? habit = await dbContext.Habits.FindAsync(id);
        if (habit is null)
        {
            return NotFound();
        }
        
        HabitDto habitDto = habit.ToDto();
        patchDoc.ApplyTo(habitDto, ModelState);
        
        if (!TryValidateModel(habitDto))
        {
            return ValidationProblem(ModelState);
        }
        
        // Path the Name and Description only
        habit.Name = habitDto.Name;
        habit.Description = habitDto.Description;
        habit.UpdatedAtUtc = DateTime.UtcNow;
        
        await dbContext.SaveChangesAsync();
        
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteHabit(string id)
    {
        Habit? habit = await dbContext.Habits.FindAsync(id);

        if (habit is null)
        {
            return NotFound();
        }

        dbContext.Habits.Remove(habit);
        await dbContext.SaveChangesAsync();

        return NoContent();
    }
}
