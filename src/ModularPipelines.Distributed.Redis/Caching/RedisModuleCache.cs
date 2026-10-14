using System.Globalization;
using ModularPipelines.Caching;
using Respire;

namespace ModularPipelines.Distributed.Redis.Caching;

/// <summary>
/// Stores shareable, chunked module cache entries in Redis.
/// Cache keys are independent of distributed pipeline run identifiers. Every key of one entry
/// carries the fingerprint as a Redis Cluster hash tag, so the publishing transaction and
/// cleanup deletes stay within one slot.
/// </summary>
internal sealed class RedisModuleCache : IModuleCacheStore
{
    private static readonly TimeSpan MinimumProvisionalExpiration = TimeSpan.FromHours(1);

    // Removes the metadata only while it still names the failed generation, so a newer entry survives.
    private static readonly RespireScript DeleteMetadataIfUnchangedScript = RespireScript.Create("""
        if redis.call('GET', KEYS[1]) == ARGV[1] then
            return redis.call('UNLINK', KEYS[1])
        end
        return 0
        """);
    private readonly RedisConnectionProvider _connections;
    private readonly string _keyPrefix;
    private readonly int _chunkSize;
    private readonly long _maximumCacheEntryBytes;
    private readonly TimeSpan _expiration;
    private readonly TimeSpan _provisionalExpiration;

    public RedisModuleCache(
        RedisConnectionProvider connections,
        RedisOptions redisOptions,
        ModuleCacheOptions cacheOptions)
    {
        ArgumentNullException.ThrowIfNull(connections);
        ArgumentNullException.ThrowIfNull(redisOptions);
        ArgumentNullException.ThrowIfNull(cacheOptions);
        ArgumentException.ThrowIfNullOrWhiteSpace(redisOptions.KeyPrefix);

        _connections = connections;
        _keyPrefix = $"{redisOptions.KeyPrefix}:module-cache:v2";
        _chunkSize = redisOptions.ChunkSizeBytes;
        _maximumCacheEntryBytes = cacheOptions.MaxCacheEntryBytes;
        _expiration = redisOptions.TimeToLive;
        _provisionalExpiration = _expiration > MinimumProvisionalExpiration
            ? _expiration
            : MinimumProvisionalExpiration;

        if (_chunkSize <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(redisOptions),
                "RedisOptions.ChunkSizeBytes must be positive.");
        }

        if (_expiration <= TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(
                nameof(redisOptions),
                "RedisOptions.TimeToLive must be positive.");
        }

        if (_maximumCacheEntryBytes <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(cacheOptions),
                "ModuleCacheOptions.MaxCacheEntryBytes must be positive.");
        }
    }

    /// <inheritdoc />
    public async Task<Stream?> OpenReadAsync(string fingerprint, CancellationToken cancellationToken)
    {
        ModuleCacheFingerprint.Validate(fingerprint);
        cancellationToken.ThrowIfCancellationRequested();

        var redis = await _connections.GetConnectionAsync(cancellationToken).ConfigureAwait(false);
        var metadata = await redis.GetStringAsync(MetadataKey(fingerprint), cancellationToken)
            .ConfigureAwait(false);
        if (metadata is null)
        {
            return null;
        }

        var (generation, chunkCount, expectedLength) = ParseMetadata(metadata);
        if (expectedLength > _maximumCacheEntryBytes)
        {
            throw CreateEntryLimitException();
        }

        // Entries keep the chunking they were written with, so validate against the stored length
        // rather than the current ChunkSizeBytes: every chunk holds at least one byte.
        if (expectedLength == 0 ? chunkCount != 0 : chunkCount < 1 || chunkCount > expectedLength)
        {
            throw new InvalidDataException("Redis module cache metadata has an inconsistent chunk count.");
        }

        var temporary = Path.GetTempFileName();
        var stream = new FileStream(
            temporary,
            FileMode.Create,
            FileAccess.ReadWrite,
            FileShare.None,
            64 * 1024,
            FileOptions.Asynchronous | FileOptions.DeleteOnClose);
        try
        {
            var restored = await RedisChunkTransfer.ReadAsync(
                    async (chunkIndex, token) => await redis.Strings
                        .GetLeaseAsync(ChunkKey(fingerprint, generation, chunkIndex), token)
                        .ConfigureAwait(false),
                    stream,
                    chunkCount,
                    chunkLength => stream.Length > _maximumCacheEntryBytes - chunkLength ? CreateEntryLimitException() : null,
                    cancellationToken)
                .ConfigureAwait(false);
            if (restored < chunkCount)
            {
                throw new InvalidDataException(
                    $"Redis module cache entry '{fingerprint}' is missing chunk {restored}.");
            }

            if (stream.Length != expectedLength)
            {
                throw new InvalidDataException(
                    $"Redis module cache entry '{fingerprint}' expected {expectedLength} bytes but restored {stream.Length}.");
            }

            stream.Position = 0;
            return stream;
        }
        catch
        {
            await stream.DisposeAsync().ConfigureAwait(false);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task WriteAsync(string fingerprint, Stream content, CancellationToken cancellationToken)
    {
        ModuleCacheFingerprint.Validate(fingerprint);
        ArgumentNullException.ThrowIfNull(content);

        var redis = await _connections.GetConnectionAsync(cancellationToken).ConfigureAwait(false);
        var generation = Guid.NewGuid().ToString("N");
        var totalLength = 0L;
        var startedChunks = 0;
        string? publishedMetadata = null;
        try
        {
            // Each chunk has its own buffer, which Respire borrows until its write completes, so several writes
            // can be in flight. The RespireValue conversion stores the raw bytes; the generic overload would
            // serialize them as JSON. The count of started writes tells cleanup which chunk keys may exist.
            var chunkCount = await RedisChunkTransfer.WriteAsync(
                    RedisChunkTransfer.ReadChunksAsync(content, _chunkSize, first: null, cancellationToken),
                    async (chunkIndex, chunk, token) =>
                    {
                        var chunkKey = ChunkKey(fingerprint, generation, chunkIndex);
                        Interlocked.Increment(ref startedChunks);
                        await redis.SetAsync(chunkKey, (RespireValue) chunk, _provisionalExpiration, cancellationToken: token)
                            .ConfigureAwait(false);
                        Interlocked.Add(ref totalLength, chunk.Length);
                    },
                    cancellationToken)
                .ConfigureAwait(false);

            var metadata = string.Create(
                CultureInfo.InvariantCulture,
                $"{generation}:{chunkCount}:{totalLength}");
            var transaction = redis.CreateTransaction();
            await using var transactionLifetime = transaction.ConfigureAwait(false);
            var expirations = ChunkKeys(fingerprint, generation, chunkCount)
                .Select(chunkKey => transaction.Expire(chunkKey, _expiration))
                .ToArray();
            var metadataWrite = transaction.Set(
                MetadataKey(fingerprint),
                (RespireValue) metadata,
                _expiration);
            cancellationToken.ThrowIfCancellationRequested();

            // Once EXEC is sent Redis may commit it, so wait for the outcome instead of abandoning it on
            // cancellation, and remember that the metadata may now name this generation.
            publishedMetadata = metadata;
            await transaction.CommitAsync(CancellationToken.None).ConfigureAwait(false);

            if (expirations.Any(expiration => !expiration.Result) || !metadataWrite.Result)
            {
                throw new IOException(
                    $"Redis could not expire module cache entry '{fingerprint}'.");
            }
        }
        catch
        {
            await DiscardWriteAsync(
                    redis,
                    fingerprint,
                    publishedMetadata,
                    ChunkKeys(fingerprint, generation, Volatile.Read(ref startedChunks)))
                .ConfigureAwait(false);
            throw;
        }
    }

    // Best effort: the write's own failure is what the caller needs, so a failed cleanup is left to expiry.
    private async Task DiscardWriteAsync(
        IRespireClient redis,
        string fingerprint,
        string? publishedMetadata,
        RespireKey[] chunkKeys)
    {
        try
        {
            // Remove committed metadata before its chunks, so readers never see an entry with missing chunks. If
            // that fails, the chunks stay: the metadata may still name them, and they expire with it.
            if (publishedMetadata is not null)
            {
                await redis.Scripts.ExecuteIntegerAsync(
                        DeleteMetadataIfUnchangedScript,
                        [MetadataKey(fingerprint)],
                        [publishedMetadata],
                        CancellationToken.None)
                    .ConfigureAwait(false);
            }

            if (chunkKeys.Length > 0)
            {
                await redis.Keys.UnlinkAsync(chunkKeys, CancellationToken.None).ConfigureAwait(false);
            }
        }
        catch (Exception)
        {
            // Provisional chunk expiry and the entry's TTL remove whatever is left.
        }
    }

    /// <inheritdoc />
    public async Task<bool> ExistsAsync(string fingerprint, CancellationToken cancellationToken)
    {
        ModuleCacheFingerprint.Validate(fingerprint);
        var redis = await _connections.GetConnectionAsync(cancellationToken).ConfigureAwait(false);
        return await redis.ExistsAsync(MetadataKey(fingerprint), cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task DeleteAsync(string fingerprint, CancellationToken cancellationToken)
    {
        ModuleCacheFingerprint.Validate(fingerprint);
        var redis = await _connections.GetConnectionAsync(cancellationToken).ConfigureAwait(false);

        var metadataKey = MetadataKey(fingerprint);
        var metadata = await redis.GetStringAsync(metadataKey, cancellationToken)
            .ConfigureAwait(false);
        if (metadata is null)
        {
            return;
        }

        // Delete the metadata first so readers never observe an entry with missing chunks.
        await redis.Keys.UnlinkAsync([metadataKey], cancellationToken).ConfigureAwait(false);
        var (generation, chunkCount, _) = ParseMetadata(metadata);
        if (chunkCount > 0)
        {
            await redis.Keys.UnlinkAsync(ChunkKeys(fingerprint, generation, chunkCount), cancellationToken)
                .ConfigureAwait(false);
        }
    }

    private RespireKey[] ChunkKeys(string fingerprint, string generation, int chunkCount) =>
        [.. Enumerable.Range(0, chunkCount).Select(chunkIndex => (RespireKey) ChunkKey(fingerprint, generation, chunkIndex))];

    private static (string Generation, int ChunkCount, long Length) ParseMetadata(string metadata)
    {
        var generationSeparator = metadata.IndexOf(':', StringComparison.Ordinal);
        var countSeparator = generationSeparator < 0
            ? -1
            : metadata.IndexOf(':', generationSeparator + 1);
        var generation = generationSeparator < 0 ? string.Empty : metadata[..generationSeparator];
        if (generation.Length != 32
            || generation.Any(character => !Uri.IsHexDigit(character))
            || countSeparator <= generationSeparator + 1
            || !int.TryParse(
                metadata.AsSpan(generationSeparator + 1, countSeparator - generationSeparator - 1),
                NumberStyles.None,
                CultureInfo.InvariantCulture,
                out var chunks)
            || !long.TryParse(metadata.AsSpan(countSeparator + 1), NumberStyles.None, CultureInfo.InvariantCulture, out var length)
            || chunks < 0
            || length < 0)
        {
            throw new InvalidDataException("Redis module cache metadata is invalid.");
        }

        return (generation, chunks, length);
    }

    // The braces form a Redis Cluster hash tag: an entry's metadata and chunks share one slot.
    private string MetadataKey(string fingerprint) =>
        $"{_keyPrefix}:{{{fingerprint.ToLowerInvariant()}}}:metadata";

    private string ChunkKey(string fingerprint, string generation, int chunkIndex) =>
        $"{_keyPrefix}:{{{fingerprint.ToLowerInvariant()}}}:entry:{generation}:chunk:{chunkIndex}";

    private InvalidDataException CreateEntryLimitException() =>
        new($"Redis module cache entry exceeded the configured limit of {_maximumCacheEntryBytes:N0} bytes.");
}
