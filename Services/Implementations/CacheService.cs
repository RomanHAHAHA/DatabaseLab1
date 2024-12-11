using DatabaseLab1.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Concurrent;

namespace DatabaseLab1.Services.Implementations;

public class CacheService(
    IMemoryCache memoryCache,
    ILogger<CacheService> logger) : ICacheService
{
    private static readonly ConcurrentDictionary<string, bool> CacheKeys = new();

    private readonly IMemoryCache _memoryCache = memoryCache;
    private readonly ILogger<CacheService> _logger = logger;

    public Task<string> SetAsync<T>(T data) where T : class
    {
        var cacheKey = Guid.NewGuid().ToString();
        _memoryCache.Set(cacheKey, data);

        CacheKeys.TryAdd(cacheKey, false);
        _logger.LogInformation($"Cache [{cacheKey}] set with no expiration.");

        LogAllKeys();

        return Task.FromResult(cacheKey);
    }

    public Task<List<T>> GetAllAsync<T>() where T : class
    {
        var allValues = new List<T>();

        foreach (var key in CacheKeys.Keys)
        {
            if (_memoryCache.TryGetValue(key, out T? cachedValue) &&
                cachedValue is not null)
            {
                allValues.Add(cachedValue);
            }
        }

        return Task.FromResult(allValues);
    }

    public Task RemoveAsync(string key)
    {
        _memoryCache.Remove(key);
        CacheKeys.TryRemove(key, out bool _);

        _logger.LogInformation($"Cache [{key}] removed.");
        LogAllKeys();

        return Task.CompletedTask;
    }

    private void LogAllKeys()
    {
        if (CacheKeys.IsEmpty)
        {
            _logger.LogInformation("No cache keys present in memory.");
        }
        else
        {
            _logger.LogInformation("Current memory cache keys:");
            foreach (var key in CacheKeys.Keys)
            {
                Console.WriteLine($"\t- {key}");
            }
        }
    }
}
