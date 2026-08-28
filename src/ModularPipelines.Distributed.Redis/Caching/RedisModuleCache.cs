using System.Globalization;
using ModularPipelines.Caching;
using ModularPipelines.Distributed.Redis.Configuration;
using StackExchange.Redis;

namespace ModularPipelines.Distributed.Redis.Caching;

/// <summary>
/// Stores shareable, chunked module cache entries in Redis.
/// Cache keys are independent of distributed pipeline run identifiers.
/// </summary>
public sealed class RedisModuleCache : IModuleCacheStore
{
    private static readonly TimeSpan MinimumProvisionalExpiration = TimeSpan.FromHours(1);
    private readonly IDatabase _database;
    private readonly string _keyPrefix;
    private readonly int _chunkSize;
    private readonly long _maximumCacheEntryBytes;
    private readonly TimeSpan _expiration;
    private readonly TimeSpan _provisionalExpiration;

    /// <summary>
    /// Initialises a new instance of the <see cref="RedisModuleCache"/> class.
    /// Initializes a new instance of the <see cref="RedisModuleCache"/> class.
    /// </summary>
    public RedisModuleCache(
        IConnectionMultiplexer connection,
        RedisDistributedOptions redisOptions,
        ArtifactOptions artifactOptions)
        : this(connection, redisOptions, artifactOptions, new ModuleCacheOptions())
    {
    }

    internal RedisModuleCache(
        IConnectionMultiplexer connection,
        RedisDistributedOptions redisOptions,
        ArtifactOptions artifactOptions,
        ModuleCacheOptions cacheOptions)
    {
        ArgumentNullException.ThrowIfNull(connection);
        ArgumentNullException.ThrowIfNull(redisOptions);
        ArgumentNullException.ThrowIfNull(artifactOptions);
        ArgumentNullException.ThrowIfNull(cacheOptions);
        ArgumentException.ThrowIfNullOrWhiteSpace(redisOptions.KeyPrefix);

        _database = connection.GetDatabase();
        _keyPrefix = $"{redisOptions.KeyPrefix}:module-cache:v1";
        _chunkSize = artifactOptions.ChunkSizeBytes;
        _maximumCacheEntryBytes = cacheOptions.MaximumCacheEntryBytes;
        _expiration = artifactOptions.TimeToLive;
        _provisionalExpiration = _expiration > MinimumProvisionalExpiration
            ? _expiration
            : MinimumProvisionalExpiration;

        if (_chunkSize <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(artifactOptions),
                "ArtifactOptions.ChunkSizeBytes must be positive.");
        }

        if (_expiration <= TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(
                nameof(artifactOptions),
                "ArtifactOptions.TimeToLive must be positive.");
        }

        if (_maximumCacheEntryBytes <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(cacheOptions),
                "ModuleCacheOptions.MaximumCacheEntryBytes must be positive.");
        }
    }

    /// <inheritdoc />
    public async Task<Stream?> OpenReadAsync(string fingerprint, CancellationToken cancellationToken)
    {
        ModuleCacheFingerprint.Validate(fingerprint);
        cancellationToken.ThrowIfCancellationRequested();

        var metadata = await _database.StringGetAsync(MetadataKey(fingerprint))
            .WaitAsync(cancellationToken)
            .ConfigureAwait(false);
        if (metadata.IsNull)
        {
            return null;
        }

        var (generation, chunkCount, expectedLength) = ParseMetadata(metadata.ToString());
        if (expectedLength > _maximumCacheEntryBytes)
        {
            throw CreateEntryLimitException();
        }

        var expectedChunkCount = expectedLength / _chunkSize;
        if (expectedLength % _chunkSize != 0)
        {
            expectedChunkCount++;
        }

        if (chunkCount != expectedChunkCount)
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
            for (var chunkIndex = 0; chunkIndex < chunkCount; chunkIndex++)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var chunk = await _database.StringGetAsync(ChunkKey(fingerprint, generation, chunkIndex))
                    .WaitAsync(cancellationToken)
                    .ConfigureAwait(false);
                if (chunk.IsNull)
                {
                    throw new InvalidDataException(
                        $"Redis module cache entry '{fingerprint}' is missing chunk {chunkIndex}.");
                }

                var chunkBytes = (byte[]) chunk!;
                if (stream.Length > _maximumCacheEntryBytes - chunkBytes.Length)
                {
                    throw CreateEntryLimitException();
                }

                await stream.WriteAsync(chunkBytes, cancellationToken).ConfigureAwait(false);
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

        var buffer = new byte[_chunkSize];
        var generation = Guid.NewGuid().ToString("N");
        var chunkCount = 0;
        var totalLength = 0L;
        var chunkKeys = new List<RedisKey>();
        try
        {
            while (true)
            {
                var length = await ReadFullBufferAsync(content, buffer, cancellationToken).ConfigureAwait(false);
                if (length == 0)
                {
                    break;
                }

                cancellationToken.ThrowIfCancellationRequested();
                var chunkKey = ChunkKey(fingerprint, generation, chunkCount);
                await _database.StringSetAsync(
                        chunkKey,
                        new ReadOnlyMemory<byte>(buffer, 0, length),
                        _provisionalExpiration)
                    .WaitAsync(cancellationToken)
                    .ConfigureAwait(false);
                chunkKeys.Add(chunkKey);
                chunkCount++;
                totalLength += length;
            }

            var metadata = string.Create(
                CultureInfo.InvariantCulture,
                $"{generation}:{chunkCount}:{totalLength}");
            var transaction = _database.CreateTransaction();
            var expirationTasks = chunkKeys
                .Select(chunkKey => transaction.KeyExpireAsync(chunkKey, _expiration))
                .ToArray();
            var metadataTask = transaction.StringSetAsync(
                MetadataKey(fingerprint),
                metadata,
                _expiration);
            cancellationToken.ThrowIfCancellationRequested();
            var transactionCommitted = await transaction.ExecuteAsync().ConfigureAwait(false);
            if (!transactionCommitted)
            {
                throw new IOException(
                    $"Redis could not publish module cache entry '{fingerprint}'.");
            }

            var expirationResults = await Task.WhenAll(expirationTasks).ConfigureAwait(false);
            var metadataWritten = await metadataTask.ConfigureAwait(false);
            if (expirationResults.Any(success => !success) || !metadataWritten)
            {
                throw new IOException(
                    $"Redis could not expire module cache entry '{fingerprint}'.");
            }
        }
        catch
        {
            if (chunkKeys.Count > 0)
            {
                await _database.KeyDeleteAsync([.. chunkKeys]).ConfigureAwait(false);
            }

            throw;
        }
    }

    private static async Task<int> ReadFullBufferAsync(
        Stream stream,
        byte[] buffer,
        CancellationToken cancellationToken)
    {
        var totalRead = 0;
        while (totalRead < buffer.Length)
        {
            var read = await stream.ReadAsync(buffer.AsMemory(totalRead), cancellationToken).ConfigureAwait(false);
            if (read == 0)
            {
                break;
            }

            totalRead += read;
        }

        return totalRead;
    }

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

    private string MetadataKey(string fingerprint) =>
        $"{_keyPrefix}:{fingerprint.ToLowerInvariant()}:metadata";

    private string ChunkKey(string fingerprint, string generation, int chunkIndex) =>
        $"{_keyPrefix}:{fingerprint.ToLowerInvariant()}:entry:{generation}:chunk:{chunkIndex}";

    private InvalidDataException CreateEntryLimitException() =>
        new($"Redis module cache entry exceeded the configured limit of {_maximumCacheEntryBytes:N0} bytes.");
}
