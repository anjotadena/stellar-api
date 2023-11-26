using API.Extensions;
using API.Middleware;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddApplicationServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseMiddleware<ExceptionMiddleware>();

app.UseStatusCodePagesWithReExecute("/errors/{0}");

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.UseStaticFiles();

app.MapControllers();

using var scope = app.Services.CreateScope();

var services = scope.ServiceProvider;
var context = services.GetRequiredService<StellarDbContext>();
var logger = services.GetRequiredService<ILogger<Program>>();

try
{
    Console.WriteLine("Initiating database migration...");
    await context.Database.MigrateAsync();
    Console.WriteLine("Database migration completed successfully!");
    Console.WriteLine("Commencing data seeding...");
    await StellarDbContextSeed.SeedAsync(context);
    Console.WriteLine("Data seeding completed successfully!");
}
catch (Exception ex)
{
    logger.LogError(ex, "An error occurred during database migration.");
}

app.Run();
