using API.Errors;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace API.Extensions;

public static class ApplicationServicesExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        
        services.AddScoped<IBasketRepository, BasketRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped(typeof(ISpecification<>), typeof(BaseSpecification<>));
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        services.AddDbContext<StellarDbContext>(option =>
        {
            option.UseSqlite(configuration.GetConnectionString("DefaultConnection"));
        });

        services.AddSingleton<IConnectionMultiplexer>(c =>
        {
            var options = ConfigurationOptions.Parse(configuration.GetConnectionString("Redis"));

            return ConnectionMultiplexer.Connect(options);
        });

        services.Configure<ApiBehaviorOptions>(options =>
        {
            // Handle our validation errors
            options.InvalidModelStateResponseFactory = actionContext =>
            {
                var errors = actionContext
                                        .ModelState
                                        .Where(e => e.Value.Errors.Count > 0)
                                        .SelectMany(x => x.Value.Errors)
                                        .Select(x => x.ErrorMessage)
                                        .ToArray();

                var errorResponse = new ApiValidationErrorResponse { Errors = errors };

                return new BadRequestObjectResult(errorResponse);
            };
        });

        services.AddCors(option =>
        {
            option.AddPolicy("CorsPolicy", policy =>
            {
                policy
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .WithOrigins("https://localhost:4200");
            });
        });

        return services;
    }
}