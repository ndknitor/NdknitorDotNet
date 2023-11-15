using System.Security.Claims;
using StackExchange.Redis;

namespace Ndknitor.Services.Web;
public class RedisSession
{
    private readonly HttpContext context;
    private readonly IDatabase redis;
    private readonly string prefix;

    public RedisSession(HttpContextAccessor accessor, IDatabase redis)
    {
        this.redis = redis;
        context = accessor.HttpContext;
        prefix = $"{nameof(RedisSession)}{context.User.FindFirstValue(ClaimTypes.NameIdentifier)}";
    }
    public T Get<T>(string key)
    {
        return default;
    }
    public T Set<T>(string key)
    {
        return default;
    }
}