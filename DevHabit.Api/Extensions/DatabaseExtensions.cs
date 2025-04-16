using DevHabit.Api.Database;
using DevHabit.Api.Database.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevHabit.Api.Extensions;

public static class DatabaseExtensions
{
    public static async Task ApplyMigrationAsync(this WebApplication app)
    {
        ArgumentNullException.ThrowIfNull(app);

        try
        {
            await using AsyncServiceScope scope = app.Services.CreateAsyncScope();
            await using ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            
            // Check if there are pending migrations before applying them
            IEnumerable<string> pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();
            if (pendingMigrations.Any())
            {
                app.Logger.LogInformation("Applying {Count} pending migrations: {Migrations}", 
                    pendingMigrations.Count(), string.Join(", ", pendingMigrations));
                await dbContext.Database.MigrateAsync();
            }
            else
            {
                app.Logger.LogInformation("No pending migrations to apply. Database is up to date.");
            }
            await dbContext.Database.EnsureCreatedAsync();
        }
        catch (Exception ex)
        {
            app.Logger.LogError(ex, "An error occurred while applying database migrations.");
            throw new InvalidOperationException("Database initialization failed. See inner exception for details.", ex);
        }
    }

    public static async Task SeedDataAsync(this DbContext dbContext, IWebHostEnvironment env)
    {
        ArgumentNullException.ThrowIfNull(dbContext);
        ArgumentNullException.ThrowIfNull(env);

        if (env.IsDevelopment())
        {
            await GenerateHabits(dbContext);
            await GenerateTags(dbContext);
        }
    }

    private static async Task GenerateHabits(DbContext dbContext)
    {
        bool containsData = await dbContext.Set<Habit>().AnyAsync();
        if (!containsData)
        {
            List<Habit> sampleHabits =
            [
                new Habit
                {
                    Id = "h_01962fcb-87d3-715a-9c4f-6a0a3c6817a0",
                    Name = "Morning Meditation",
                    Description = "Meditate for 10 minutes every morning",
                    HabitType = HabitType.Measurable,
                    Frequency = new Frequency { Type = FrequencyType.Daily, TimePerPeriod = 1 },
                    Target = new Target { Value = 10, Unit = "minutes" },
                    Status = HabitStatus.Ongoing,
                    CreatedAtUtc = DateTime.UtcNow
                },

                new Habit
                {
                    Id = $"h_{Guid.CreateVersion7()}",
                    Name = "Drink Water",
                    Description = "Drink 8 glasses of water daily",
                    HabitType = HabitType.Measurable,
                    Frequency = new Frequency { Type = FrequencyType.Daily, TimePerPeriod = 1 },
                    Target = new Target { Value = 8, Unit = "glasses" },
                    Status = HabitStatus.Ongoing,
                    CreatedAtUtc = DateTime.UtcNow
                },

                new Habit
                {
                    Id = $"h_{Guid.CreateVersion7()}",
                    Name = "Evening Walk",
                    Description = "Walk for 30 minutes every evening",
                    HabitType = HabitType.Measurable,
                    Frequency = new Frequency { Type = FrequencyType.Daily, TimePerPeriod = 1 },
                    Target = new Target { Value = 30, Unit = "minutes" },
                    Status = HabitStatus.Ongoing,
                    CreatedAtUtc = DateTime.UtcNow
                },

                new Habit
                {
                    Id = $"h_{Guid.CreateVersion7()}",
                    Name = "Read a Book",
                    Description = "Read 20 pages of a book daily",
                    HabitType = HabitType.Measurable,
                    Frequency = new Frequency { Type = FrequencyType.Daily, TimePerPeriod = 1 },
                    Target = new Target { Value = 20, Unit = "pages" },
                    Status = HabitStatus.Ongoing,
                    CreatedAtUtc = DateTime.UtcNow
                },

                new Habit
                {
                    Id = $"h_{Guid.CreateVersion7()}",
                    Name = "Weekly Jogging",
                    Description = "Jog for 5 kilometers every week",
                    HabitType = HabitType.Measurable,
                    Frequency = new Frequency { Type = FrequencyType.Weekly, TimePerPeriod = 1 },
                    Target = new Target { Value = 5, Unit = "kilometers" },
                    Status = HabitStatus.Ongoing,
                    CreatedAtUtc = DateTime.UtcNow
                },

                new Habit
                {
                    Id = $"h_{Guid.CreateVersion7()}",
                    Name = "Daily Exercise",
                    Description = "Exercise for 30 minutes daily",
                    HabitType = HabitType.Measurable,
                    Frequency = new Frequency { Type = FrequencyType.Daily, TimePerPeriod = 1 },
                    Target = new Target { Value = 30, Unit = "minutes" },
                    Status = HabitStatus.Ongoing,
                    CreatedAtUtc = DateTime.UtcNow
                },

                new Habit
                {
                    Id = $"h_{Guid.CreateVersion7()}",
                    Name = "Read Books",
                    Description = "Read 10 pages of a book",
                    HabitType = HabitType.Measurable,
                    Frequency = new Frequency { Type = FrequencyType.Daily, TimePerPeriod = 1 },
                    Target = new Target { Value = 10, Unit = "pages" },
                    Status = HabitStatus.Ongoing,
                    CreatedAtUtc = DateTime.UtcNow
                }
            ];

            for (int i = 8; i <= 20; i++)
            {
                sampleHabits.Add(new Habit
                {
                    Id = $"h_{Guid.CreateVersion7()}",
                    Name = $"Sample Habit {i}",
                    Description = $"Description for Sample Habit {i}",
                    HabitType = HabitType.Measurable,
                    Frequency = new Frequency { Type = FrequencyType.Daily, TimePerPeriod = 1 },
                    Target = new Target { Value = i * 5, Unit = "units" },
                    Status = HabitStatus.Ongoing,
                    CreatedAtUtc = DateTime.UtcNow
                });
            }

            await dbContext.Set<Habit>().AddRangeAsync(sampleHabits);
            await dbContext.SaveChangesAsync();
        }
    }

    private static async Task GenerateTags(DbContext dbContext)
    {
        bool containsTags = await dbContext.Set<Tag>().AnyAsync();
        if (!containsTags)
        {
            List<Tag> sampleTags =
            [
                new Tag { Id = $"t_{Guid.CreateVersion7()}", Name = "Health", Description = "Habits related to physical and mental well-being." },
                new Tag { Id = $"t_{Guid.CreateVersion7()}", Name = "Productivity", Description = "Habits that improve efficiency and output." },
                new Tag { Id = $"t_{Guid.CreateVersion7()}", Name = "Fitness", Description = "Habits focused on exercise and staying fit." },
                new Tag { Id = $"t_{Guid.CreateVersion7()}", Name = "Learning", Description = "Habits for acquiring new knowledge or skills." },
                new Tag { Id = $"t_{Guid.CreateVersion7()}", Name = "Mindfulness", Description = "Habits that promote mindfulness and meditation." },
                new Tag { Id = $"t_{Guid.CreateVersion7()}", Name = "Nutrition", Description = "Habits related to healthy eating and diet." },
                new Tag { Id = $"t_{Guid.CreateVersion7()}", Name = "Creativity", Description = "Habits that foster creative thinking and activities." },
                new Tag { Id = $"t_{Guid.CreateVersion7()}", Name = "Finance", Description = "Habits for managing money and financial growth." },
                new Tag { Id = $"t_{Guid.CreateVersion7()}", Name = "Social", Description = "Habits that improve social interactions and relationships." },
                new Tag { Id = $"t_{Guid.CreateVersion7()}", Name = "Self-care", Description = "Habits for personal care and relaxation." }
            ];

            await dbContext.Set<Tag>().AddRangeAsync(sampleTags);
            await dbContext.SaveChangesAsync();
        }
    }
}
