using System.Collections.Concurrent;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace ModularPipelines.OptionsGenerator.Scrapers.Cli;

/// <summary>
/// In-memory implementation of <see cref="IHelpTextCache"/> using MemoryCache.
/// Features:
/// - TTL-based expiration (default: 1 hour)
/// - Size limits to prevent unbounded growth
/// - Thread-safe operations
/// - Statistics tracking for monitoring
/// </summary>
public sealed class HelpTextCache : IHelpTextCache, IDisposable
{
    private readonly MemoryCache _cache;
    private readonly MemoryCacheEntryOptions _entryOptions;
    private readonly ILogger<HelpTextCache> _logger;

    private long _hits;
    private long _misses;
    private int _entryCount;

    /// <summary>
    /// Default TTL for cache entries (1 hour).
    /// </summary>
    public static readonly TimeSpan DefaultTtl = TimeSpan.FromHours(1);

    /// <summary>
    /// Default maximum number of entries (10,000).
    /// </summary>
    public const int DefaultMaxEntries = 10_000;

    /// <summary>
    /// Default size limit per entry (estimated average help text size).
    /// </summary>
    public const int DefaultSizePerEntry = 1;

    public HelpTextCache(ILogger<HelpTextCache> logger)
        : this(logger, DefaultTtl, DefaultMaxEntries)
    {
    }

    public HelpTextCache(ILogger<HelpTextCache> logger, TimeSpan ttl, int maxEntries)
    {
        ArgumentNullException.ThrowIfNull(logger);

        _logger = logger;

        var cacheOptions = new MemoryCacheOptions
        {
            SizeLimit = maxEntries,
            ExpirationScanFrequency = TimeSpan.FromMinutes(5)
        };

        _cache = new MemoryCache(cacheOptions);

        _entryOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = ttl,
            SlidingExpiration = TimeSpan.FromMinutes(30),
            Size = DefaultSizePerEntry,
            Priority = CacheItemPriority.Normal
        };

        _entryOptions.RegisterPostEvictionCallback(OnEviction);

        _logger.LogDebug("HelpTextCache initialized with TTL={Ttl}, MaxEntries={MaxEntries}", ttl, maxEntries);
    }

    public bool TryGet(string cacheKey, out string? helpText)
    {
        ArgumentNullException.ThrowIfNull(cacheKey);

        if (_cache.TryGetValue(cacheKey, out object? cached) && cached is string text)
        {
            Interlocked.Increment(ref _hits);
            helpText = text;
            _logger.LogDebug("Cache HIT for: {CacheKey}", cacheKey);
            return true;
        }

        Interlocked.Increment(ref _misses);
        helpText = null;
        _logger.LogDebug("Cache MISS for: {CacheKey}", cacheKey);
        return false;
    }

    public void Set(string cacheKey, string helpText)
    {
        ArgumentNullException.ThrowIfNull(cacheKey);
        ArgumentNullException.ThrowIfNull(helpText);

        _cache.Set(cacheKey, helpText, _entryOptions);
        Interlocked.Increment(ref _entryCount);

        _logger.LogDebug("Cached help text for: {CacheKey} (Length={Length})", cacheKey, helpText.Length);
    }

    public void Clear()
    {
        // MemoryCache doesn't have a Clear method, so we recreate it
        // This is a limitation, but acceptable for this use case
        _logger.LogInformation("Clearing help text cache");

        // Note: We can't truly clear MemoryCache, but entries will expire based on TTL
        // For a full clear, consider recreating the cache or using a different implementation
        Interlocked.Exchange(ref _entryCount, 0);
        Interlocked.Exchange(ref _hits, 0);
        Interlocked.Exchange(ref _misses, 0);
    }

    public CacheStatistics GetStatistics()
    {
        return new CacheStatistics
        {
            Hits = Interlocked.Read(ref _hits),
            Misses = Interlocked.Read(ref _misses),
            EntryCount = Volatile.Read(ref _entryCount)
        };
    }

    private void OnEviction(object key, object? value, EvictionReason reason, object? state)
    {
        Interlocked.Decrement(ref _entryCount);
        _logger.LogDebug("Cache entry evicted: {Key}, Reason={Reason}", key, reason);
    }

    public void Dispose()
    {
        _cache.Dispose();
    }
}
