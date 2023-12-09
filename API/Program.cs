using API.Extensions;
using API.Middleware;
using Core.Entities.Identity;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseMiddleware<ExceptionMiddleware>();

app.UseStatusCodePagesWithReExecute("/errors/{0}");

app.UseSwagger();
app.UseSwaggerUI();

app.UseStaticFiles();

app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using var scope = app.Services.CreateScope();

var services = scope.ServiceProvider;
var context = services.GetRequiredService<StellarDbContext>();
var identityContext = services.GetRequiredService<AppIdentityDbContext>();
var userManager = services.GetRequiredService<UserManager<AppUser>>();
var logger = services.GetRequiredService<ILogger<Program>>();

try
{
    Console.WriteLine("Initiating database migration...");
    await context.Database.MigrateAsync();
    Console.WriteLine("Database migration completed successfully!");

    Console.WriteLine("Initiating database identity migration...");
    await identityContext.Database.MigrateAsync();
    Console.WriteLine("Database identity migration completed successfully!");

    Console.WriteLine("Commencing data seeding...");
    await StellarDbContextSeed.SeedAsync(context);
    Console.WriteLine("Data seeding completed successfully!");

    Console.WriteLine("Commencing identity seeding...");
    await AppIdentityDbContextSeed.SeedUsersAsync(userManager);
    Console.WriteLine("Data identity seeding completed successfully!");
}
catch (Exception ex)
{
    logger.LogError(ex, "An error occurred during database migration.");
}

app.Run();
