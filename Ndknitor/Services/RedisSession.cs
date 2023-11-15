using System.Security.Claims;
using StackExchange.Redis;
using Ndknitor.System;
using System.Text;
using Microsoft.Extensions.Options;

namespace Ndknitor.Services.Web;

public static class RedisSessionServiceCollectionExtensions
{
    public static void AddRedisSession(this IServiceCollection services, Action<RedisSessionOptions> configureOptions = null)
    {
        services.AddSingleton<RedisSession>();
        services.AddSingleton<GlobalRedisSession>();
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

public class RedisSession
{
    private readonly HttpContext context;
    private readonly IDatabase redis;
    private readonly string prefix;

    public RedisSession(IHttpContextAccessor accessor, IDatabase redis, IOptions<RedisSessionOptions> options)
    {
        this.redis = redis;
        context = accessor.HttpContext;
        prefix = $"{nameof(RedisSession)}{context.User.FindFirstValue(ClaimTypes.NameIdentifier)}";
        Timeout = options.Value.Timeout ?? TimeSpan.FromMinutes(30); // Set a default timeout if not provided
    }

    public TimeSpan Timeout { get; }

    public T Get<T>(string key)
    {
        key = prefix + key;
        var value = redis.StringGet(key);
        if (value.HasValue)
        {
            return Encoding.UTF8.GetBytes(value).ToBsonClass<T>();
        }
        return default;
    }

    public async Task<T> GetAsync<T>(string key)
    {
        key = prefix + key;
        var value = await redis.StringGetAsync(key);
        if (value.HasValue)
        {
            return Encoding.UTF8.GetBytes(value).ToBsonClass<T>();
        }
        return default;
    }

    public bool Set(string key, object value, TimeSpan? expiry = null)
    {
        key = prefix + key;
        return redis.StringSet(key, value.ToBson(), expiry ?? Timeout);
    }

    public async Task<bool> SetAsync(string key, object value, TimeSpan? expiry = null)
    {
        key = prefix + key;
        return await redis.StringSetAsync(key, value.ToBson(), expiry ?? Timeout);
    }

    public bool Exist(string key)
    {
        key = prefix + key;
        return redis.KeyExists(key);
    }

    public async Task<bool> ExistAsync(string key)
    {
        key = prefix + key;
        return await redis.KeyExistsAsync(key);
    }
    public bool Remove(string key)
    {
        key = prefix + key;
        return redis.KeyDelete(key);
    }

    public async Task<bool> RemoveAsync(string key)
    {
        key = prefix + key;
        return await redis.KeyDeleteAsync(key);
    }
    public IEnumerable<string> GetAllKeys()
    {
        var keys = redis.Execute("KEYS", $"{prefix}*"); // Prefix with your identifier
        return keys.ToDictionary().Keys;
    }

    public async Task<IEnumerable<string>> GetAllKeysAsync()
    {
        var keys = await redis.ExecuteAsync("KEYS", $"{prefix}*"); // Prefix with your identifier
        return keys.ToDictionary().Keys;
    }
}

public class GlobalRedisSession
{
    private readonly IDatabase redis;
    public GlobalRedisSession(IHttpContextAccessor accessor, IDatabase redis, IOptions<RedisSessionOptions> options)
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
}