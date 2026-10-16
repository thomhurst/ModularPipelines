using System.Text.Json;
using Kevlar;
using ModularPipelines.Distributed.Redis.Coordination;
using Respire;

namespace ModularPipelines.Distributed.Redis.Artifacts;

/// <summary>
/// Redis-based implementation of <see cref="IDistributedArtifactStore"/>.
/// Artifacts that fit in one chunk are stored under a single key; larger artifacts are chunked.
/// All keys are isolated by run identifier and expire via TTL, or are deleted when the run is cleaned up.
/// Data and chunk reads and writes are idempotent, so they are retried after transient timeouts and
/// connection failures.
/// </summary>
internal sealed class RedisDistributedArtifactStore : IDistributedArtifactStore, IDistributedRunCleanup
{
    private const int ChunkDeleteBatchSize = 256;
    private const int MetadataReadBatchSize = 250;

    private readonly IRespireClient _redis;
    private readonly RedisKeyBuilder _keys;
    private readonly TimeSpan _timeToLive;
    private readonly int _chunkSize;
    private readonly Shield _transientRetry;

    public RedisDistributedArtifactStore(
        IRespireClient redis,
        RedisKeyBuilder keys,
        RedisOptions options,
        TimeSpan? retryBaseDelay = null)
    {
        ArgumentNullException.ThrowIfNull(options);
        if (options.ChunkSizeBytes <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(options),
                $"{nameof(RedisOptions)}.{nameof(RedisOptions.ChunkSizeBytes)} must be positive.");
        }

        _redis = redis;
        _keys = keys;
        _timeToLive = options.TimeToLive;
        _chunkSize = options.ChunkSizeBytes;
        _transientRetry = RedisTransientRetry.Create(retryBaseDelay ?? RedisTransientRetry.DefaultBaseDelay);
    }

    public async Task<ArtifactReference> UploadAsync(ArtifactDescriptor descriptor, Stream data, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(descriptor);
        ArgumentNullException.ThrowIfNull(data);

        var artifactId = Guid.NewGuid().ToString("N");

        // Index before writing content, so run cleanup also finds an upload that fails part way. Listing skips
        // an indexed artifact until its metadata is written.
        await IndexArtifactAsync(descriptor.ModuleId, artifactId, cancellationToken).ConfigureAwait(false);

        var first = await RedisChunkTransfer.ReadChunkAsync(data, _chunkSize, cancellationToken).ConfigureAwait(false);
        var totalBytes = 0L;

        if (first.Length < _chunkSize)
        {
            // Fits in one value, including empty artifacts: always write the key so an empty
            // artifact round-trips instead of looking missing.
            await SetValueAsync(_keys.ArtifactData(artifactId), first, cancellationToken).ConfigureAwait(false);
            totalBytes = first.Length;
        }
        else
        {
            // Each chunk gets its own buffer, so several writes can be in flight while the next chunk is read.
            await RedisChunkTransfer.WriteAsync(
                    RedisChunkTransfer.ReadChunksAsync(data, _chunkSize, first, cancellationToken),
                    async (chunkIndex, chunk, token) =>
                    {
                        await SetValueAsync(_keys.ArtifactChunk(artifactId, chunkIndex), chunk, token).ConfigureAwait(false);
                        Interlocked.Add(ref totalBytes, chunk.Length);
                    },
                    cancellationToken)
                .ConfigureAwait(false);
        }

        var reference = new ArtifactReference
        {
            ArtifactId = artifactId,
            Name = descriptor.Name,
            ModuleId = descriptor.ModuleId,
            SizeBytes = totalBytes,
            ContentType = descriptor.ContentType,
            UploadedAt = DateTimeOffset.UtcNow,
        };

        var metaJson = JsonSerializer.Serialize(reference);
        await _redis.SetAsync(_keys.ArtifactMeta(artifactId), (RespireValue) metaJson, _timeToLive, cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        // Index again: reading a slow source can outlast the TTL of the index written before the content.
        await IndexArtifactAsync(descriptor.ModuleId, artifactId, cancellationToken).ConfigureAwait(false);

        return reference;
    }

    // Adds the artifact to its module's index and the module to the run's module set, each with the TTL, in one
    // transaction, so no index key can be left without an expiry or outside run cleanup's reach. Both keys share
    // the run's hash tag, so the transaction stays in one Redis Cluster slot.
    private async Task IndexArtifactAsync(ModuleId moduleId, string artifactId, CancellationToken cancellationToken)
    {
        var indexKey = _keys.ArtifactIndex(moduleId);
        var transaction = _redis.CreateTransaction();
        await using var transactionLifetime = transaction.ConfigureAwait(false);
        var indexed = transaction.Sets.Add(indexKey, [artifactId]);
        var indexExpiry = transaction.Expire(indexKey, _timeToLive);
        var moduleIndexed = transaction.Sets.Add(_keys.ArtifactModules, [moduleId.Value]);
        var moduleExpiry = transaction.Expire(_keys.ArtifactModules, _timeToLive);
        await transaction.CommitAsync(cancellationToken).ConfigureAwait(false);

        // EXEC succeeds even when a queued command fails, for example on a wrong key type or an ACL denial.
        // Reading each result rethrows that command's error, so an unindexed upload is never reported as stored.
        _ = indexed.Result;
        _ = moduleIndexed.Result;
        if (!indexExpiry.Result || !moduleExpiry.Result)
        {
            throw new IOException($"Redis could not set the expiry of the artifact index for '{artifactId}'.");
        }
    }

    public async Task<Stream> DownloadAsync(ArtifactReference reference, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(reference);
        cancellationToken.ThrowIfCancellationRequested();

        var evictionsBefore = await RedisMemoryDiagnostics.ReadEvictionSnapshotAsync(_redis, cancellationToken).ConfigureAwait(false);

        var data = await GetValueAsync(_keys.ArtifactData(reference.ArtifactId), cancellationToken)
            .ConfigureAwait(false);

        if (data is not null)
        {
            await ThrowIfSizeMismatchAsync(reference, data.Length, evictionsBefore, cancellationToken).ConfigureAwait(false);
            return new MemoryStream(data, writable: false);
        }

        // ZIP consumers require seeking. Disk backing bounds memory and supports
        // artifacts larger than MemoryStream's 2 GB capacity.
        var temporaryPath = Path.Combine(Path.GetTempPath(), $"modularpipelines-artifact-{Guid.NewGuid():N}.tmp");
        var stream = new FileStream(temporaryPath, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.None,
            64 * 1024, FileOptions.Asynchronous | FileOptions.DeleteOnClose);
        try
        {
            // Chunks are written from zero, so the first missing chunk ends the artifact; the size check below
            // catches chunks that expired or were evicted.
            var chunkCount = await RedisChunkTransfer.ReadAsync(
                    (chunkIndex, token) => GetLeaseAsync(_keys.ArtifactChunk(reference.ArtifactId, chunkIndex), token),
                    stream,
                    int.MaxValue,
                    reject: null,
                    cancellationToken)
                .ConfigureAwait(false);

            if (chunkCount == 0)
            {
                throw new InvalidOperationException($"Artifact '{reference.ArtifactId}' not found in Redis."
                    + await RedisMemoryDiagnostics.DescribeEvictionsAsync(_redis, evictionsBefore, cancellationToken).ConfigureAwait(false));
            }

            await ThrowIfSizeMismatchAsync(reference, stream.Length, evictionsBefore, cancellationToken).ConfigureAwait(false);
            stream.Position = 0;
            return stream;
        }
        catch
        {
            await stream.DisposeAsync().ConfigureAwait(false);
            throw;
        }
    }

    public async Task<IReadOnlyList<ArtifactReference>> ListArtifactsAsync(ModuleId moduleId, CancellationToken cancellationToken)
    {
        var members = await ReadSetAsync(_keys.ArtifactIndex(moduleId), cancellationToken).ConfigureAwait(false);
        var references = new List<ArtifactReference>(members.Count);
        foreach (var batch in members.Chunk(MetadataReadBatchSize))
        {
            // Every metadata key shares the run's hash tag, so MGET also works on Redis Cluster.
            RespireKey[] metadataKeys = [.. batch.Select(member => (RespireKey) _keys.ArtifactMeta(member))];
            var metadata = await _redis.Strings.GetManyAsync(metadataKeys, cancellationToken).ConfigureAwait(false);
            foreach (var metaJson in metadata)
            {
                // Skip unreadable metadata like missing metadata, so one bad entry cannot hide the rest.
                if (TryParseReference(metaJson) is { } reference)
                {
                    references.Add(reference);
                }
            }
        }

        return references;
    }

    public async Task DeleteAsync(ArtifactReference reference, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(reference);

        await DeleteArtifactKeysAsync(reference.ArtifactId, reference.SizeBytes, cancellationToken).ConfigureAwait(false);
        await _redis.Sets.RemoveAsync(_keys.ArtifactIndex(reference.ModuleId), [reference.ArtifactId], cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Deletes every artifact uploaded in the run, by any process, and the indexes that list them.
    /// </summary>
    public async Task CleanupRunAsync(DistributedRunCleanupContext context, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(context);

        var moduleIds = await ReadSetAsync(_keys.ArtifactModules, cancellationToken).ConfigureAwait(false);
        foreach (var moduleId in moduleIds)
        {
            var indexKey = _keys.ArtifactIndex(new ModuleId(moduleId));
            var artifactIds = await ReadSetAsync(indexKey, cancellationToken).ConfigureAwait(false);
            foreach (var artifactId in artifactIds)
            {
                var sizeBytes = await GetRecordedSizeAsync(artifactId, cancellationToken).ConfigureAwait(false);
                await DeleteArtifactKeysAsync(artifactId, sizeBytes, cancellationToken).ConfigureAwait(false);
            }

            await _redis.Keys.UnlinkAsync([indexKey], cancellationToken)
                .ConfigureAwait(false);
        }

        await _redis.Keys.UnlinkAsync([_keys.ArtifactModules], cancellationToken)
            .ConfigureAwait(false);
    }

    private async Task<HashSet<string>> ReadSetAsync(string key, CancellationToken cancellationToken)
    {
        var members = new HashSet<string>(StringComparer.Ordinal);
        await foreach (var member in _redis.Sets.ScanAsync(key, cancellationToken: cancellationToken).ConfigureAwait(false))
        {
            // Cursor scans may return the same member more than once.
            members.Add(member);
        }

        return members;
    }

    // A partial upload has no metadata yet, so its size is unknown.
    private async Task<long?> GetRecordedSizeAsync(string artifactId, CancellationToken cancellationToken)
    {
        var metaJson = await _redis.GetStringAsync(_keys.ArtifactMeta(artifactId), cancellationToken)
            .ConfigureAwait(false);
        return TryParseReference(metaJson)?.SizeBytes;
    }

    private static ArtifactReference? TryParseReference(string? metaJson)
    {
        if (string.IsNullOrEmpty(metaJson))
        {
            return null;
        }

        try
        {
            return JsonSerializer.Deserialize<ArtifactReference>(metaJson);
        }
        catch (JsonException)
        {
            return null;
        }
    }

    private async Task DeleteArtifactKeysAsync(string artifactId, long? sizeBytes, CancellationToken cancellationToken)
    {
        // UNLINK frees large values on a background thread instead of blocking the server.
        await _redis.Keys.UnlinkAsync([_keys.ArtifactMeta(artifactId), _keys.ArtifactData(artifactId)], cancellationToken)
            .ConfigureAwait(false);

        // Chunks share the run's hash tag, so unlink them in batches: one round trip per batch keeps large
        // artifacts within the timeout. A recorded size gives the chunk count, so evicted early chunks cannot end
        // the loop before later ones; without one, chunks written in order from zero end at the first short batch.
        var recordedChunks = sizeBytes is { } size ? (size + _chunkSize - 1) / _chunkSize : 0;
        for (var firstChunk = 0; ; firstChunk += ChunkDeleteBatchSize)
        {
            RespireKey[] chunkKeys =
            [
                .. Enumerable.Range(firstChunk, ChunkDeleteBatchSize)
                    .Select(chunkIndex => (RespireKey) _keys.ArtifactChunk(artifactId, chunkIndex)),
            ];
            var deleted = await _redis.Keys.UnlinkAsync(chunkKeys, cancellationToken)
                .ConfigureAwait(false);
            if (deleted < ChunkDeleteBatchSize && firstChunk + ChunkDeleteBatchSize >= recordedChunks)
            {
                break;
            }
        }
    }

    private ValueTask<byte[]?> GetValueAsync(string key, CancellationToken cancellationToken) =>
        _transientRetry.ExecuteAsync(
            async token => await _redis.GetBytesAsync(key, token).ConfigureAwait(false),
            cancellationToken);

    // Large chunks are read into pooled memory instead of a new array per chunk.
    private async Task<RespireLease> GetLeaseAsync(string key, CancellationToken cancellationToken) =>
        await _transientRetry.ExecuteAsync(
                async token => await _redis.Strings.GetLeaseAsync(key, token).ConfigureAwait(false),
                cancellationToken)
            .ConfigureAwait(false);

    // Respire borrows binary values until the command completes, so the caller hands over a buffer that is
    // never reused; every retry writes the same bytes.
    private ValueTask<bool> SetValueAsync(string key, byte[] value, CancellationToken cancellationToken) =>
        _transientRetry.ExecuteAsync(
            async token => await _redis.SetAsync(key, (RespireValue) value, _timeToLive, cancellationToken: token).ConfigureAwait(false),
            cancellationToken);

    private async Task ThrowIfSizeMismatchAsync(
        ArtifactReference reference,
        long actualBytes,
        RedisMemoryDiagnostics.EvictionSnapshot? evictionsBefore,
        CancellationToken cancellationToken)
    {
        if (actualBytes != reference.SizeBytes)
        {
            throw new InvalidOperationException(
                $"Artifact '{reference.ArtifactId}' size mismatch: expected {reference.SizeBytes} bytes but got {actualBytes} bytes. " +
                "One or more chunks may have expired or been evicted."
                + await RedisMemoryDiagnostics.DescribeEvictionsAsync(_redis, evictionsBefore, cancellationToken).ConfigureAwait(false));
        }
    }
}
