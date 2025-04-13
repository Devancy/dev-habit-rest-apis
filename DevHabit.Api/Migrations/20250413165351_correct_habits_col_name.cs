﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevHabit.Api.Migrations
{
    /// <inheritdoc />
    public partial class correct_habits_col_name : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "frequency_frequency_type",
                schema: "dev_habit",
                table: "habits",
                newName: "frequency_type");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "frequency_type",
                schema: "dev_habit",
                table: "habits",
                newName: "frequency_frequency_type");
        }
    }
}
