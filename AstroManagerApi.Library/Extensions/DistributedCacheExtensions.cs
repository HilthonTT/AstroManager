using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace AstroManagerApi.Library.Extensions;
public static class DistributedCacheExtensions
{
    public static async Task SetRecordAsync<T>(
        this IDistributedCache cache,
        string recordId,
        T data,
        TimeSpan? absoluteExpireTime = null,
        TimeSpan? unusedExpireTime = null)
    {
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = absoluteExpireTime ?? TimeSpan.FromMinutes(30),
            SlidingExpiration = unusedExpireTime
        };

        string jsonData = JsonSerializer.Serialize(data);
        await cache.SetStringAsync(recordId, jsonData, options);
    }

    public static async Task<T> GetRecordAsync<T>(this IDistributedCache cache, string recordId)
    {
        string jsonData = await cache.GetStringAsync(recordId);

        if (string.IsNullOrWhiteSpace(jsonData))
        {
            return default;
        }

        return JsonSerializer.Deserialize<T>(jsonData);
    }
}
