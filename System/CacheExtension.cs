using Microsoft.Extensions.Caching.Distributed;
using Ndknitor.System;
namespace Ndknitor.System;
public static class CacheExtension
{
    public static T Get<T>(this IDistributedCache cache, string key)
    {
        return cache.GetString(key).ToClass<T>();
    }
    public static async Task<T> GetAsync<T>(this IDistributedCache cache, string key)
    {
        return (await cache.GetStringAsync(key)).ToClass<T>();
    }
    public static void Set<T>(this IDistributedCache cache, string key, T value, int exprieSec)
    {
        cache.SetString(key, value.ToJson(),
        new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(exprieSec)
        });
    }
    public static async Task SetAsync<T>(this IDistributedCache cache, string key, T value, int exprieSec)
    {
        await cache.SetStringAsync(key, value.ToJson(), new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(exprieSec)
        });
    }
}