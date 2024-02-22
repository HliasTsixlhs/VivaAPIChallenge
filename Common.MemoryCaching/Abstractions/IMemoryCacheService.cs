namespace Common.MemoryCaching.Abstractions;

public interface IMemoryCacheService
{
    T Get<T>(string key);
    void Set<T>(string key, T value, TimeSpan cacheDuration);
    bool Remove(string key);
}