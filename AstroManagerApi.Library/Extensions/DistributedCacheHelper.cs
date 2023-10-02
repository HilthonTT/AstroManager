using AstroManagerApi.Library.Extensions.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace AstroManagerApi.Library.Extensions;
public class DistributedCacheHelper : IDistributedCacheHelper
{
    private readonly IDistributedCache _redisCache;
    private readonly IMemoryCache _cache;
    private readonly ILogger<DistributedCacheHelper> _logger;

    public DistributedCacheHelper(
        IDistributedCache redisCache,
        IMemoryCache cache,
        ILogger<DistributedCacheHelper> logger)
    {
        _redisCache = redisCache;
        _cache = cache;
        _logger = logger;
    }

    public async Task SetRecordAsync<T>(
        string recordId,
        T data,
        TimeSpan? absoluteExpireTime = null,
        TimeSpan? unusedExpireTime = null)
    {
        try
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = absoluteExpireTime ?? TimeSpan.FromMinutes(30),
                SlidingExpiration = unusedExpireTime
            };

            string jsonData = JsonSerializer.Serialize(data);
            await _redisCache.SetStringAsync(recordId, jsonData, options);
        }
        catch (Exception ex)
        {
            _logger.LogError("Redis cache failed: {0}", ex.Message);
            _cache.Set(recordId, data, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = absoluteExpireTime ?? TimeSpan.FromMinutes(30),
                SlidingExpiration = unusedExpireTime
            });
        }
    }

    public async Task<T> GetRecordAsync<T>(string recordId)
    {
        try
        {
            await _redisCache.RemoveAsync(recordId);

            string jsonData = await _redisCache.GetStringAsync(recordId);

            if (string.IsNullOrWhiteSpace(jsonData))
            {
                return default;
            }

            return JsonSerializer.Deserialize<T>(jsonData);
        }
        catch (Exception ex)
        {
            _logger.LogError("Redis cache failed: {0}", ex.Message);
            return _cache.Get<T>(recordId);
        }
    }

    public async Task RemoveAsync(string recordId)
    {
        try
        {
            await _redisCache.RemoveAsync(recordId);
        }
        catch (Exception ex)
        {
            _logger.LogError("Redis cache failed: {0}", ex.Message);
            _cache.Remove(recordId);
        }
    }
}
