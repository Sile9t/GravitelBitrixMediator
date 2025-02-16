using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Services
{
    public class RedisCacheService
    {
        private readonly IDistributedCache _cache;

        public RedisCacheService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<T?> GetCachedData<T>(string key)
        {
            var jsonData = await _cache.GetStringAsync(key);

            if (jsonData is null)
                return default(T);

            return JsonSerializer.Deserialize<T>(jsonData);
        }

        public async Task SetCacheData<T>(string key, T data, TimeSpan cacheDuration)
        {
            var options = new DistributedCacheEntryOptions
            {
                SlidingExpiration = cacheDuration
            };

            var jsonData = JsonSerializer.Serialize<T>(data);
            await _cache.SetStringAsync(key, jsonData, options);
        }
    }
}
