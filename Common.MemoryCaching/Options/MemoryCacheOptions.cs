namespace Common.MemoryCaching.Options;

public class MemoryCacheOptions
{
    public int SizeLimit { get; set; } // Sets a size limit for the cache, 0 for no limit

    public TimeSpan ExpirationScanFrequency { get; set; } // Frequency to scan for expired items
    // Add other caching options as needed
}