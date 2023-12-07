using System.Text.Json;
using Core.Entities;
using Core.Interfaces;
using StackExchange.Redis;

namespace Infrastructure.Repositories;
public class BasketRepository : IBasketRepository
{
    private readonly IDatabase _redisDb;

    public BasketRepository(IConnectionMultiplexer redis)
    {
        _redisDb = redis.GetDatabase();
    }

    public async Task<bool> DeleteBasketAsync(string basketId)
    {
        return await _redisDb.KeyDeleteAsync(basketId);
    }

    public async Task<CustomerBasket?> GetBasketAsync(string basketId)
    {
        var data = await _redisDb.StringGetAsync(basketId);

        return data.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CustomerBasket>(data);
    }

    public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket)
    {
        var created = await _redisDb.StringSetAsync(basket.Id, JsonSerializer.Serialize(basket), TimeSpan.FromDays(30));

        if (!created) {
            return null;
        }

        return await GetBasketAsync(basket.Id);
    }
}