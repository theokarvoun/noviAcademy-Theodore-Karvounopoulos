using Microsoft.EntityFrameworkCore;
using NLog.Extensions.Logging;
using System.Data.Common;
using System.Text.Json.Serialization;
using WorldRank;
using WorldRank.Application.Interfaces;
using WorldRank.Application.Services;
using WorldRank.Application.Strategies;
using WorldRank.Infrastructure;
using WorldRank.Infrastructure.Persistence.Context;
using WorldRank.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Logging via NLog (same nlog.config layout as the Console app).
builder.Logging.ClearProviders();
builder.Logging.AddNLog("nlog.config");

// One AppDbContext per request (scoped) — the EF Core repositories depend on it.
builder.Services.AddDbContext<WorldRankDBContext>(
            options => options.UseSqlServer("Server=localhost;Database=WorldRank;Integrated Security=true;TrustServerCertificate=true"),
            contextLifetime: ServiceLifetime.Singleton,
            optionsLifetime: ServiceLifetime.Singleton);

builder.Services.AddSingleton<IPlayerRepository, DBPlayerRepository>();
builder.Services.AddSingleton<IWalletRepository, DBWalletRepository>();

// Single-instance in-memory cache (Day 6). Redis would replace this behind a load balancer.
builder.Services.AddMemoryCache();

// The services own the caching (read-through + write-through) and reach the DB via the repositories.
builder.Services.AddSingleton<IFundsStrategy, AddFundsStrategy>();
builder.Services.AddSingleton<IFundsStrategy, SubtractFundsStrategy>();
builder.Services.AddSingleton<IFundsStrategy, ForceSubtractFundsStrategy>();

// Application services that drive the menu use-cases.
builder.Services.AddSingleton<PlayerService>();
builder.Services.AddSingleton<WalletService>();

// Accept/emit enums (e.g. Currency) as their string names, not numbers.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

// Swagger / OpenAPI — interactive API docs at /swagger.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Serve the Swagger JSON and UI in Development.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapGet("/", () => Results.Redirect("/swagger")); // root → Swagger UI
}

app.MapControllers();

app.Run();