using System.Text.Json;
using ModularPipelines.Distributed.Redis.Coordination;
using StackExchange.Redis;

namespace ModularPipelines.Distributed.Redis.Artifacts;

/// <summary>
/// Redis-based implementation of <see cref="IDistributedArtifactStore"/>.
/// Artifacts that fit in one chunk are stored under a single key; larger artifacts are chunked.
/// All keys are isolated by run identifier and expire via TTL.
/// </summary>
internal sealed class RedisDistributedArtifactStore : IDistributedArtifactStore
{
    private readonly IDatabase _database;
    private readonly RedisKeyBuilder _keys;
    private readonly TimeSpan _timeToLive;
    private readonly int _chunkSize;

    public RedisDistributedArtifactStore(
        IDatabase database,
        RedisKeyBuilder keys,
        RedisOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        if (options.ChunkSizeBytes <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(options),
                $"{nameof(RedisOptions)}.{nameof(RedisOptions.ChunkSizeBytes)} must be positive.");
        }

        _database = database;
        _keys = keys;
        _timeToLive = options.TimeToLive;
        _chunkSize = options.ChunkSizeBytes;
    }

    public async Task<ArtifactReference> UploadAsync(ArtifactDescriptor descriptor, Stream data, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(descriptor);
        ArgumentNullException.ThrowIfNull(data);

        var artifactId = Guid.NewGuid().ToString("N");
        var buffer = new byte[_chunkSize];
        var bytesRead = await ReadFullBufferAsync(data, buffer, cancellationToken).ConfigureAwait(false);
        var totalBytes = 0L;

        if (bytesRead < buffer.Length)
        {
            // Fits in one value, including empty artifacts: always write the key so an empty
            // artifact round-trips instead of looking missing.
            await _database.StringSetAsync(
                    _keys.ArtifactData(artifactId),
                    new ReadOnlyMemory<byte>(buffer, 0, bytesRead),
                    _timeToLive)
                .WaitAsync(cancellationToken)
                .ConfigureAwait(false);
            totalBytes = bytesRead;
        }
        else
        {
            var chunkIndex = 0;
            while (bytesRead > 0)
            {
                await _database.StringSetAsync(
                        _keys.ArtifactChunk(artifactId, chunkIndex),
                        new ReadOnlyMemory<byte>(buffer, 0, bytesRead),
                        _timeToLive)
                    .WaitAsync(cancellationToken)
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
        await _database.StringSetAsync(_keys.ArtifactMeta(artifactId), metaJson, _timeToLive)
            .WaitAsync(cancellationToken)
            .ConfigureAwait(false);

        var indexKey = _keys.ArtifactIndex(descriptor.ModuleId);
        await _database.SetAddAsync(indexKey, artifactId)
            .WaitAsync(cancellationToken)
            .ConfigureAwait(false);
        await _database.KeyExpireAsync(indexKey, _timeToLive)
            .WaitAsync(cancellationToken)
            .ConfigureAwait(false);

        return reference;
    }

    public async Task<Stream> DownloadAsync(ArtifactReference reference, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(reference);
        cancellationToken.ThrowIfCancellationRequested();

        var data = await _database.StringGetAsync(_keys.ArtifactData(reference.ArtifactId))
            .WaitAsync(cancellationToken)
            .ConfigureAwait(false);

        if (!data.IsNull)
        {
            var bytes = (byte[]?) data ?? [];
            ThrowIfSizeMismatch(reference, bytes.Length);
            return new MemoryStream(bytes, writable: false);
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
                var chunk = await _database.StringGetAsync(_keys.ArtifactChunk(reference.ArtifactId, chunkIndex))
                    .WaitAsync(cancellationToken)
                    .ConfigureAwait(false);
                if (chunk.IsNull)
                {
                    break;
                }

                var bytes = (byte[]) chunk!;
                await stream.WriteAsync(bytes, cancellationToken).ConfigureAwait(false);
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
        var members = await _database.SetMembersAsync(_keys.ArtifactIndex(moduleId))
            .WaitAsync(cancellationToken)
            .ConfigureAwait(false);

        if (members.Length == 0)
        {
            return [];
        }

        var references = new List<ArtifactReference>(members.Length);
        foreach (var member in members)
        {
            var metaJson = await _database.StringGetAsync(_keys.ArtifactMeta(member.ToString()))
                .WaitAsync(cancellationToken)
                .ConfigureAwait(false);
            if (!metaJson.IsNull)
            {
                var reference = JsonSerializer.Deserialize<ArtifactReference>(metaJson.ToString());
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

        await _database.KeyDeleteAsync(_keys.ArtifactMeta(reference.ArtifactId))
            .WaitAsync(cancellationToken)
            .ConfigureAwait(false);
        await _database.KeyDeleteAsync(_keys.ArtifactData(reference.ArtifactId))
            .WaitAsync(cancellationToken)
            .ConfigureAwait(false);

        var chunkIndex = 0;
        while (true)
        {
            var deleted = await _database.KeyDeleteAsync(_keys.ArtifactChunk(reference.ArtifactId, chunkIndex))
                .WaitAsync(cancellationToken)
                .ConfigureAwait(false);
            if (!deleted)
            {
                break;
            }

            chunkIndex++;
        }

        await _database.SetRemoveAsync(_keys.ArtifactIndex(reference.ModuleId), reference.ArtifactId)
            .WaitAsync(cancellationToken)
            .ConfigureAwait(false);
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
