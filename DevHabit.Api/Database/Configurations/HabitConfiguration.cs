using DevHabit.Api.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace DevHabit.Api.Database.Configurations;

public sealed class HabitConfiguration : IEntityTypeConfiguration<Habit>
{
    public void Configure(EntityTypeBuilder<Habit> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasMaxLength(500);
        builder.Property(x => x.Name).HasMaxLength(100);
        builder.Property(x => x.Description).HasMaxLength(500);
        builder.OwnsOne(h => h.Frequency);
        builder.OwnsOne(h => h.Target, targetBuilder => targetBuilder.Property(t => t.Unit).HasMaxLength(100));
        builder.OwnsOne(h => h.MileStone);
    }
}
