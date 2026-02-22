using System.Text.Json;
using ModularPipelines.Distributed.Redis.Configuration;
using ModularPipelines.Distributed.Redis.Coordination;
using StackExchange.Redis;

namespace ModularPipelines.Distributed.Redis.Artifacts;

/// <summary>
/// Redis-based implementation of <see cref="IDistributedArtifactStore"/>.
/// Small artifacts are stored as binary strings; large artifacts are chunked.
/// All keys are isolated by run identifier and expire via TTL.
/// </summary>
internal sealed class RedisDistributedArtifactStore : IDistributedArtifactStore
{
    private readonly IDatabase _database;
    private readonly RedisKeyBuilder _keys;
    private readonly TimeSpan _keyExpiration;
    private readonly int _chunkSize;
    private readonly long _maxSingleUpload;

    public RedisDistributedArtifactStore(
        IDatabase database,
        RedisKeyBuilder keys,
        ArtifactOptions options)
    {
        _database = database;
        _keys = keys;
        _keyExpiration = TimeSpan.FromSeconds(options.TimeToLiveSeconds);
        _chunkSize = options.ChunkSizeBytes;
        _maxSingleUpload = options.MaxSingleUploadBytes;
    }

    public async Task<ArtifactReference> UploadAsync(ArtifactDescriptor descriptor, Stream data, CancellationToken cancellationToken)
    {
        var artifactId = Guid.NewGuid().ToString("N");

        using var ms = new MemoryStream();
        await data.CopyToAsync(ms, cancellationToken);
        var bytes = ms.ToArray();

        if (bytes.Length <= _maxSingleUpload)
        {
            // Single key storage
            var dataKey = _keys.ArtifactData(artifactId);
            await _database.StringSetAsync(dataKey, bytes, _keyExpiration);
        }
        else
        {
            // Chunked storage
            var chunkCount = (int)Math.Ceiling((double)bytes.Length / _chunkSize);
            for (var i = 0; i < chunkCount; i++)
            {
                var offset = i * _chunkSize;
                var length = Math.Min(_chunkSize, bytes.Length - offset);
                var chunk = new byte[length];
                Buffer.BlockCopy(bytes, offset, chunk, 0, length);

                var chunkKey = _keys.ArtifactChunk(artifactId, i);
                await _database.StringSetAsync(chunkKey, chunk, _keyExpiration);
            }
        }

        var reference = new ArtifactReference(
            ArtifactId: artifactId,
            Name: descriptor.Name,
            ModuleTypeName: descriptor.ModuleTypeName,
            SizeBytes: bytes.Length,
            ContentType: descriptor.ContentType,
            UploadedAt: DateTimeOffset.UtcNow);

        // Store metadata
        var metaKey = _keys.ArtifactMeta(artifactId);
        var metaJson = JsonSerializer.Serialize(reference);
        await _database.StringSetAsync(metaKey, metaJson, _keyExpiration);

        // Add to module artifact index
        var indexKey = _keys.ArtifactIndex(descriptor.ModuleTypeName);
        await _database.SetAddAsync(indexKey, artifactId);
        await _database.KeyExpireAsync(indexKey, _keyExpiration);

        return reference;
    }

    public async Task<Stream> DownloadAsync(ArtifactReference reference, CancellationToken cancellationToken)
    {
        // Try single key first
        var dataKey = _keys.ArtifactData(reference.ArtifactId);
        var data = await _database.StringGetAsync(dataKey);

        if (!data.IsNull)
        {
            return new MemoryStream((byte[])data!);
        }

        // Try chunked
        var ms = new MemoryStream();
        var chunkIndex = 0;
        while (true)
        {
            var chunkKey = _keys.ArtifactChunk(reference.ArtifactId, chunkIndex);
            var chunk = await _database.StringGetAsync(chunkKey);
            if (chunk.IsNull)
            {
                break;
            }

            var bytes = (byte[])chunk!;
            ms.Write(bytes, 0, bytes.Length);
            chunkIndex++;
        }

        if (ms.Length == 0)
        {
            throw new InvalidOperationException($"Artifact '{reference.ArtifactId}' not found in Redis.");
        }

        ms.Position = 0;
        return ms;
    }

    public async Task<IReadOnlyList<ArtifactReference>> ListArtifactsAsync(string moduleTypeName, CancellationToken cancellationToken)
    {
        var indexKey = _keys.ArtifactIndex(moduleTypeName);
        var members = await _database.SetMembersAsync(indexKey);

        if (members.Length == 0)
        {
            return [];
        }

        var references = new List<ArtifactReference>(members.Length);
        foreach (var member in members)
        {
            var metaKey = _keys.ArtifactMeta(member.ToString());
            var metaJson = await _database.StringGetAsync(metaKey);
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
        // Delete metadata
        await _database.KeyDeleteAsync(_keys.ArtifactMeta(reference.ArtifactId));

        // Delete data (single or chunked)
        await _database.KeyDeleteAsync(_keys.ArtifactData(reference.ArtifactId));

        var chunkIndex = 0;
        while (true)
        {
            var deleted = await _database.KeyDeleteAsync(_keys.ArtifactChunk(reference.ArtifactId, chunkIndex));
            if (!deleted)
            {
                break;
            }

            chunkIndex++;
        }

        // Remove from module index
        var indexKey = _keys.ArtifactIndex(reference.ModuleTypeName);
        await _database.SetRemoveAsync(indexKey, reference.ArtifactId);
    }
}
