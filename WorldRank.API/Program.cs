using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using NLog.Extensions.Logging;
using WorldRank.Application;
using WorldRank.Infrastructure;
using WorldRank.Infrastructure.Persistence;
using WorldRank.Infrastructure.Persistence.Context;

var builder = WebApplication.CreateBuilder(args);

// Logging via NLog (same nlog.config layout as the Console app).
builder.Logging.ClearProviders();
builder.Logging.AddNLog("nlog.config");

// Compose the layers. AddApplication registers the strategies, the application services
// (behind IPlayerService / IWalletService) and their caching logic; AddInfrastructureDatabase
// registers the EF Core repositories, the DbContext, the in-memory cache and the ICache port.
builder.Services.AddApplication();
builder.Services.AddInfrastructureDatabase(DbConnection.ConnectionString);

// Accept/emit enums (e.g. Currency) as their string names, not numbers.
builder.Services.AddControllers()
	.AddJsonOptions(options =>
		options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

// Swagger / OpenAPI — interactive API docs at /swagger.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Apply any pending EF Core migrations so the schema is up to date.
using (var scope = app.Services.CreateScope())
{
	var dbContext = scope.ServiceProvider.GetRequiredService<WorldRankDBContext>();
	dbContext.Database.Migrate();
}

// Serve the Swagger JSON and UI in Development.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
	app.MapGet("/", () => Results.Redirect("/swagger")); // root → Swagger UI
}

app.MapControllers();

app.Run();
