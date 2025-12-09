namespace ModularPipelines.OptionsGenerator.Scrapers.Cli;

/// <summary>
/// Interface for caching CLI help text output.
/// Abstracted to allow different caching strategies (in-memory, disk, etc.)
/// and proper resource management (TTL, eviction policies).
/// </summary>
public interface IHelpTextCache
{
    /// <summary>
    /// Tries to get cached help text for the given command.
    /// </summary>
    /// <param name="cacheKey">The cache key (typically the full command path).</param>
    /// <param name="helpText">The cached help text if found.</param>
    /// <returns>True if the help text was found in cache, false otherwise.</returns>
    bool TryGet(string cacheKey, out string? helpText);

    /// <summary>
    /// Stores help text in the cache.
    /// </summary>
    /// <param name="cacheKey">The cache key (typically the full command path).</param>
    /// <param name="helpText">The help text to cache.</param>
    void Set(string cacheKey, string helpText);

    /// <summary>
    /// Clears all cached entries.
    /// </summary>
    void Clear();

    /// <summary>
    /// Gets cache statistics for monitoring.
    /// </summary>
    CacheStatistics GetStatistics();
}

/// <summary>
/// Cache statistics for monitoring and diagnostics.
/// </summary>
public record CacheStatistics
{
    /// <summary>
    /// Number of cache hits.
    /// </summary>
    public long Hits { get; init; }

    /// <summary>
    /// Number of cache misses.
    /// </summary>
    public long Misses { get; init; }

    /// <summary>
    /// Current number of entries in the cache.
    /// </summary>
    public int EntryCount { get; init; }

    /// <summary>
    /// Hit ratio (0.0 to 1.0).
    /// </summary>
    public double HitRatio => Hits + Misses > 0 ? (double)Hits / (Hits + Misses) : 0;
}
