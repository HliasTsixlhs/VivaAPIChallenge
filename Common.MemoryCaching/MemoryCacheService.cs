using Common.MemoryCaching.Abstractions;
using Microsoft.Extensions.Caching.Memory;

namespace Common.MemoryCaching;

public class MemoryCacheService : IMemoryCacheService
{
    private readonly IMemoryCache _memoryCache;

    public MemoryCacheService(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public T Get<T>(string key)
    {
        _memoryCache.TryGetValue(key, out T value);
        return value;
    }

    public void Set<T>(string key, T value, TimeSpan cacheDuration)
    {
        _memoryCache.Set(key, value, new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = cacheDuration
        });
    }

    public bool Remove(string key)
    {
        _memoryCache.Remove(key);
        return true;
    }
}