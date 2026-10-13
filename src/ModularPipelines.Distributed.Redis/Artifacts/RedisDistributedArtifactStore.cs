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

        var buffer = new byte[_chunkSize];
        var bytesRead = await ReadFullBufferAsync(data, buffer, cancellationToken).ConfigureAwait(false);
        var totalBytes = 0L;

        if (bytesRead < buffer.Length)
        {
            // Fits in one value, including empty artifacts: always write the key so an empty
            // artifact round-trips instead of looking missing.
            await SetValueAsync(
                    _keys.ArtifactData(artifactId),
                    new ReadOnlyMemory<byte>(buffer, 0, bytesRead),
                    cancellationToken)
                .ConfigureAwait(false);
            totalBytes = bytesRead;
        }
        else
        {
            var chunkIndex = 0;
            while (bytesRead > 0)
            {
                await SetValueAsync(
                        _keys.ArtifactChunk(artifactId, chunkIndex),
                        new ReadOnlyMemory<byte>(buffer, 0, bytesRead),
                        cancellationToken)
                    .ConfigureAwait(false);
                totalBytes += bytesRead;
                chunkIndex++;

                if (bytesRead < buffer.Length)
                {
                    break;
                }

                bytesRead = await ReadFullBufferAsync(data, buffer, cancellationToken).ConfigureAwait(false);
            }
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
        _ = transaction.Sets.Add(indexKey, [artifactId]);
        _ = transaction.Expire(indexKey, _timeToLive);
        _ = transaction.Sets.Add(_keys.ArtifactModules, [moduleId.Value]);
        _ = transaction.Expire(_keys.ArtifactModules, _timeToLive);
        await transaction.CommitAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task<Stream> DownloadAsync(ArtifactReference reference, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(reference);
        cancellationToken.ThrowIfCancellationRequested();

        var data = await GetValueAsync(_keys.ArtifactData(reference.ArtifactId), cancellationToken)
            .ConfigureAwait(false);

        if (data is not null)
        {
            ThrowIfSizeMismatch(reference, data.Length);
            return new MemoryStream(data, writable: false);
        }

        // ZIP consumers require seeking. Disk backing bounds memory and supports
        // artifacts larger than MemoryStream's 2 GB capacity.
        var temporaryPath = Path.Combine(Path.GetTempPath(), $"modularpipelines-artifact-{Guid.NewGuid():N}.tmp");
        var stream = new FileStream(temporaryPath, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.None,
            64 * 1024, FileOptions.Asynchronous | FileOptions.DeleteOnClose);
        try
        {
            var chunkIndex = 0;
            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var chunk = await GetValueAsync(_keys.ArtifactChunk(reference.ArtifactId, chunkIndex), cancellationToken)
                    .ConfigureAwait(false);
                if (chunk is null)
                {
                    break;
                }

                await stream.WriteAsync(chunk, cancellationToken).ConfigureAwait(false);
                chunkIndex++;
            }

            if (chunkIndex == 0)
            {
                throw new InvalidOperationException($"Artifact '{reference.ArtifactId}' not found in Redis.");
            }

            ThrowIfSizeMismatch(reference, stream.Length);
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
        var members = await _redis.Sets.MembersAsync(_keys.ArtifactIndex(moduleId), cancellationToken)
            .ConfigureAwait(false);

        if (members.Length == 0)
        {
            return [];
        }

        var references = new List<ArtifactReference>(members.Length);
        foreach (var member in members)
        {
            var metaJson = await _redis.GetStringAsync(_keys.ArtifactMeta(member), cancellationToken)
                .ConfigureAwait(false);
            if (metaJson is not null)
            {
                var reference = JsonSerializer.Deserialize<ArtifactReference>(metaJson);
                if (reference is not null)
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

        var moduleIds = await _redis.Sets.MembersAsync(_keys.ArtifactModules, cancellationToken)
            .ConfigureAwait(false);
        foreach (var moduleId in moduleIds)
        {
            var indexKey = _keys.ArtifactIndex(new ModuleId(moduleId));
            var artifactIds = await _redis.Sets.MembersAsync(indexKey, cancellationToken)
                .ConfigureAwait(false);
            foreach (var artifactId in artifactIds)
            {
                var sizeBytes = await GetRecordedSizeAsync(artifactId, cancellationToken).ConfigureAwait(false);
                await DeleteArtifactKeysAsync(artifactId, sizeBytes, cancellationToken).ConfigureAwait(false);
            }

            await _redis.DeleteAsync([indexKey], cancellationToken)
                .ConfigureAwait(false);
        }

        await _redis.DeleteAsync([_keys.ArtifactModules], cancellationToken)
            .ConfigureAwait(false);
    }

    // A partial upload has no metadata yet, so its size is unknown.
    private async Task<long?> GetRecordedSizeAsync(string artifactId, CancellationToken cancellationToken)
    {
        var metaJson = await _redis.GetStringAsync(_keys.ArtifactMeta(artifactId), cancellationToken)
            .ConfigureAwait(false);
        if (string.IsNullOrEmpty(metaJson))
        {
            return null;
        }

        try
        {
            return JsonSerializer.Deserialize<ArtifactReference>(metaJson)?.SizeBytes;
        }
        catch (JsonException)
        {
            return null;
        }
    }

    private async Task DeleteArtifactKeysAsync(string artifactId, long? sizeBytes, CancellationToken cancellationToken)
    {
        await _redis.DeleteAsync([_keys.ArtifactMeta(artifactId), _keys.ArtifactData(artifactId)], cancellationToken)
            .ConfigureAwait(false);

        // Chunks share the run's hash tag, so delete them in batches: one round trip per batch keeps large
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
            var deleted = await _redis.DeleteAsync(chunkKeys, cancellationToken)
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

    private ValueTask<bool> SetValueAsync(string key, ReadOnlyMemory<byte> value, CancellationToken cancellationToken)
    {
        // Respire borrows binary values until the command completes, and an abandoned attempt can still be
        // sent after the upload reuses its buffer for the next chunk. Every attempt writes this copy.
        var stableValue = value.ToArray();
        return _transientRetry.ExecuteAsync(
            async token => await _redis.SetAsync(key, (RespireValue) stableValue, _timeToLive, cancellationToken: token).ConfigureAwait(false),
            cancellationToken);
    }

    private static void ThrowIfSizeMismatch(ArtifactReference reference, long actualBytes)
    {
        if (actualBytes != reference.SizeBytes)
        {
            throw new InvalidOperationException(
                $"Artifact '{reference.ArtifactId}' size mismatch: expected {reference.SizeBytes} bytes but got {actualBytes} bytes. " +
                "One or more chunks may have expired or been evicted.");
        }
    }

    private static async Task<int> ReadFullBufferAsync(Stream stream, byte[] buffer, CancellationToken cancellationToken)
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
}
