using System.Collections.Concurrent;

namespace ModularPipelines.Distributed.Artifacts;

/// <summary>
/// In-memory artifact store for single-process testing.
/// Not suitable for multi-process distributed execution.
/// </summary>
internal class InMemoryDistributedArtifactStore : IDistributedArtifactStore
{
    private readonly ConcurrentDictionary<string, (ArtifactReference Reference, byte[] Data)> _artifacts = new();
    private readonly ConcurrentDictionary<string, List<string>> _moduleIndex = new();

    public async Task<ArtifactReference> UploadAsync(ArtifactDescriptor descriptor, Stream data, CancellationToken cancellationToken)
    {
        using var ms = new MemoryStream();
        await data.CopyToAsync(ms, cancellationToken);
        var bytes = ms.ToArray();

        var artifactId = Guid.NewGuid().ToString("N");
        var reference = new ArtifactReference(
            ArtifactId: artifactId,
            Name: descriptor.Name,
            ModuleTypeName: descriptor.ModuleTypeName,
            SizeBytes: bytes.Length,
            ContentType: descriptor.ContentType,
            UploadedAt: DateTimeOffset.UtcNow);

        _artifacts[artifactId] = (reference, bytes);

        _moduleIndex.AddOrUpdate(
            descriptor.ModuleTypeName,
            _ => [artifactId],
            (_, list) =>
            {
                lock (list)
                {
                    list.Add(artifactId);
                }

                return list;
            });

        return reference;
    }

    public Task<Stream> DownloadAsync(ArtifactReference reference, CancellationToken cancellationToken)
    {
        if (!_artifacts.TryGetValue(reference.ArtifactId, out var entry))
        {
            throw new InvalidOperationException($"Artifact '{reference.ArtifactId}' not found.");
        }

        Stream stream = new MemoryStream(entry.Data);
        return Task.FromResult(stream);
    }

    public Task<IReadOnlyList<ArtifactReference>> ListArtifactsAsync(string moduleTypeName, CancellationToken cancellationToken)
    {
        if (!_moduleIndex.TryGetValue(moduleTypeName, out var artifactIds))
        {
            return Task.FromResult<IReadOnlyList<ArtifactReference>>([]);
        }

        List<ArtifactReference> references;
        lock (artifactIds)
        {
            references = artifactIds
                .Where(id => _artifacts.ContainsKey(id))
                .Select(id => _artifacts[id].Reference)
                .ToList();
        }

        return Task.FromResult<IReadOnlyList<ArtifactReference>>(references);
    }

    public Task DeleteAsync(ArtifactReference reference, CancellationToken cancellationToken)
    {
        _artifacts.TryRemove(reference.ArtifactId, out _);

        if (_moduleIndex.TryGetValue(reference.ModuleTypeName, out var artifactIds))
        {
            lock (artifactIds)
            {
                artifactIds.Remove(reference.ArtifactId);
            }
        }

        return Task.CompletedTask;
    }
}
