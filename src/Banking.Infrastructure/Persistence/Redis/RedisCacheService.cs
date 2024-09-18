using Microsoft.Extensions.Caching.Distributed;

namespace Banking.Infrastructure.Persistence.Redis;

public class RedisCacheService
{
    private readonly IDistributedCache _redis;

    public RedisCacheService(IDistributedCache redis)
    {
        _redis = redis;
    }

    public async Task<T> GetOrCreateAsync<T>(
            string key,
            Func<Task<T>> createItem,
            TimeSpan? absoluteExpireTime = null,
            TimeSpan? unusedExpireTime = null)
    {
        var value = await _redis.GetStringAsync(key);

        if (value != null)
        {
            return Deserialize<T>(value!);
        }
        else
        {
            var newItem = await createItem();

            var serializedItem = Serialize(newItem);

            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = absoluteExpireTime ?? TimeSpan.FromMinutes(30),
                SlidingExpiration = unusedExpireTime,
            };

            await _redis.SetStringAsync(key, serializedItem, options);

            return newItem;
        }
    }

    public async Task RemoveAsync(string key)
    {
        await _redis.RemoveAsync(key);
    }

    private string Serialize<T>(T obj)
    {
        return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
    }

    private T Deserialize<T>(string json)
    {
        return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json)!;
    }
}