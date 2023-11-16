using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Ndknitor.System;
using StackExchange.Redis;

public static class RedisStoreServiceCollectionExtensions
{
    public static void AddRedisStore(this IServiceCollection services, Action<RedisSessionOptions> configureOptions = null)
    {
        services.AddSingleton<IRedisStore, RedisStore>();
        if (configureOptions != null)
        {
            services.Configure(configureOptions);
        }
    }
}

public class RedisSessionOptions
{
    public TimeSpan? Timeout { get; set; }
}

public interface IRedisStore
{
    public TimeSpan Timeout { get; }
    public T Get<T>(string key);
    public Task<T> GetAsync<T>(string key);
    public bool Set(string key, object value, TimeSpan? expiry = null);
    public Task<bool> SetAsync(string key, object value, TimeSpan? expiry = null);
    public bool Exist(string key);
    public Task<bool> ExistAsync(string key);
    public bool Remove(string key);
    public Task<bool> RemoveAsync(string key);
    public IEnumerable<string> GetAllKeys();
    public Task<IEnumerable<string>> GetAllKeysAsync();
}

public class RedisStore : IRedisStore
{
    private readonly IDatabase redis;
    public RedisStore(IHttpContextAccessor accessor, IDatabase redis, IOptions<RedisSessionOptions> options)
    {
        this.redis = redis;
        Timeout = options.Value.Timeout ?? TimeSpan.FromMinutes(30); // Set a default timeout if not provided
    }

    public TimeSpan Timeout { get; }

    public T Get<T>(string key)
    {
        var value = redis.StringGet(key);
        if (value.HasValue)
        {
            return Encoding.UTF8.GetBytes(value).ToBsonClass<T>();
        }
        return default;
    }

    public async Task<T> GetAsync<T>(string key)
    {
        var value = await redis.StringGetAsync(key);
        if (value.HasValue)
        {
            return Encoding.UTF8.GetBytes(value).ToBsonClass<T>();
        }
        return default;
    }

    public bool Set(string key, object value, TimeSpan? expiry = null)
    {
        return redis.StringSet(key, value.ToBson(), expiry ?? Timeout);
    }

    public async Task<bool> SetAsync(string key, object value, TimeSpan? expiry = null)
    {
        return await redis.StringSetAsync(key, value.ToBson(), expiry ?? Timeout);
    }

    public bool Exist(string key)
    {
        return redis.KeyExists(key);
    }

    public async Task<bool> ExistAsync(string key)
    {
        return await redis.KeyExistsAsync(key);
    }
    public bool Remove(string key)
    {
        return redis.KeyDelete(key);
    }

    public async Task<bool> RemoveAsync(string key)
    {
        return await redis.KeyDeleteAsync(key);
    }

    public IEnumerable<string> GetAllKeys()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<string>> GetAllKeysAsync()
    {
        throw new NotImplementedException();
    }
}