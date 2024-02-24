namespace Common.MemoryCaching.Abstractions;

public interface IMemoryCacheService
{
    /// <summary>
    /// Retrieves a cached item by its key. Returns the item if it exists, or default value otherwise.
    /// </summary>
    /// <typeparam name="T">The type of item to retrieve.</typeparam>
    /// <param name="key">The key associated with the cached item.</param>
    /// <returns>The cached item or default value.</returns>
    T Get<T>(string key);

    /// <summary>
    /// Stores an item in the cache with a specified key and cache duration.
    /// </summary>
    /// <typeparam name="T">The type of item to store.</typeparam>
    /// <param name="key">The key to store the item under.</param>
    /// <param name="value">The item to store in the cache.</param>
    /// <param name="cacheDuration">The duration for which the item should be cached.</param>
    void Set<T>(string key, T value, TimeSpan cacheDuration);

    /// <summary>
    /// Removes an item from the cache by its key.
    /// </summary>
    /// <param name="key">The key of the item to remove from the cache.</param>
    /// <returns>True if the item was successfully removed; otherwise, false.</returns>
    bool Remove(string key);
}