using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

builder.Services.AddDbContext<StellarDbContext>(option => 
{
    option.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

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
