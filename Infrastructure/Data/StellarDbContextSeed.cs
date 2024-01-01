using System.Text.Json;
using Core.Entities;
using Core.Entities.OrderAggregate;

namespace Infrastructure.Data;

public class StellarDbContextSeed
{
    public static async Task SeedAsync(StellarDbContext context)
    {
        if (!context.ProductBrands.Any())
        {
            var brands = GetEntityData<List<ProductBrand>>("../Infrastructure/Data/SeedData/brands.json");

            context.ProductBrands.AddRange(brands);
        }

        if (!context.ProductTypes.Any())
        {
            var types = GetEntityData<List<ProductType>>("../Infrastructure/Data/SeedData/types.json");

            context.ProductTypes.AddRange(types);
        }

        if (!context.Products.Any())
        {
            var products = GetEntityData<List<Product>>("../Infrastructure/Data/SeedData/products.json");

            context.Products.AddRange(products);
        }

        if (!context.DeliveryMethods.Any())
        {
            var deliveries = GetEntityData<List<DeliveryMethod>>("../Infrastructure/Data/SeedData/delivery.json");

            context.DeliveryMethods.AddRange(deliveries);
        }

        if (context.ChangeTracker.HasChanges())
        {
            await context.SaveChangesAsync();
        }
    }

    private static T GetEntityData<T>(string path)
    {
        var data = File.ReadAllText(path);

        if (data is null) {
            return default;
        }

        return JsonSerializer.Deserialize<T>(data);
    }
}
