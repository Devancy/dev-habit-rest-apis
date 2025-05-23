using DevHabit.Api.Database;
using DevHabit.Api.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options => options.RespectBrowserAcceptHeader = true)
    .AddNewtonsoftJson(options => options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter()))
    .AddXmlSerializerFormatters();
builder.Services.AddOpenApi();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options
        .UseNpgsql(
            builder.Configuration.GetConnectionString("Database"),
            npgsqlOptions => npgsqlOptions
                .MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Application))
        .UseSnakeCaseNamingConvention()
        .UseAsyncSeeding(async (context, _, _) => await context.SeedDataAsync(builder.Environment)));

builder.Services.AddOpenTelemetry()
    .ConfigureResource(r => r.AddService(builder.Environment.ApplicationName))
    .WithTracing(tracing => tracing
        .AddHttpClientInstrumentation()
        .AddAspNetCoreInstrumentation()
        .AddNpgsql())
    .WithMetrics(metrics => metrics
        .AddHttpClientInstrumentation()
        .AddAspNetCoreInstrumentation()
        .AddRuntimeInstrumentation())
    .UseOtlpExporter();

builder.Logging.AddOpenTelemetry(options =>
{
    options.IncludeScopes = true;
    options.IncludeFormattedMessage = true;
});

WebApplication? app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    await app.ApplyMigrationAsync();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
