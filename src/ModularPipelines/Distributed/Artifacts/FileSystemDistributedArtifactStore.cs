using System.Collections.Concurrent;

namespace ModularPipelines.Distributed.Artifacts;

/// <summary>
/// Temporary filesystem-backed artifact store for single-process execution.
/// </summary>
internal sealed class FileSystemDistributedArtifactStore : IDistributedArtifactStore, IDisposable
{
    private readonly ConcurrentDictionary<string, (ArtifactReference Reference, string Path)> _artifacts = new();
    private readonly ConcurrentDictionary<string, List<string>> _moduleIndex = new();
    private readonly Lazy<string> _root = new(
        static () => Directory.CreateTempSubdirectory("modular-pipelines-artifacts-").FullName,
        LazyThreadSafetyMode.ExecutionAndPublication);

    public async Task<ArtifactReference> UploadAsync(
        ArtifactDescriptor descriptor,
        Stream data,
        CancellationToken cancellationToken)
    {
        var artifactId = Guid.NewGuid().ToString("N");
        var path = Path.Combine(_root.Value, artifactId);
        try
        {
            await using (var file = new FileStream(
                             path,
                             FileMode.CreateNew,
                             FileAccess.Write,
                             FileShare.None,
                             bufferSize: 81920,
                             FileOptions.Asynchronous | FileOptions.SequentialScan))
            {
                await data.CopyToAsync(file, cancellationToken).ConfigureAwait(false);
            }

            var reference = new ArtifactReference(
                ArtifactId: artifactId,
                Name: descriptor.Name,
                ModuleTypeName: descriptor.ModuleTypeName,
                SizeBytes: new FileInfo(path).Length,
                ContentType: descriptor.ContentType,
                UploadedAt: DateTimeOffset.UtcNow);
            _artifacts[artifactId] = (reference, path);
            _moduleIndex.AddOrUpdate(
                descriptor.ModuleTypeName,
                _ => [artifactId],
                (_, artifactIds) =>
                {
                    lock (artifactIds)
                    {
                        artifactIds.Add(artifactId);
                    }

                    return artifactIds;
                });
            return reference;
        }
        catch
        {
            File.Delete(path);
            throw;
        }
    }

    public Task<Stream> DownloadAsync(
        ArtifactReference reference,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (!_artifacts.TryGetValue(reference.ArtifactId, out var entry))
        {
            throw new InvalidOperationException($"Artifact '{reference.ArtifactId}' not found.");
        }

        Stream stream = new FileStream(
            entry.Path,
            FileMode.Open,
            FileAccess.Read,
            FileShare.Read,
            bufferSize: 81920,
            FileOptions.Asynchronous | FileOptions.SequentialScan);
        return Task.FromResult(stream);
    }

    public Task<IReadOnlyList<ArtifactReference>> ListArtifactsAsync(
        string moduleTypeName,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (!_moduleIndex.TryGetValue(moduleTypeName, out var artifactIds))
        {
            return Task.FromResult<IReadOnlyList<ArtifactReference>>([]);
        }

        List<ArtifactReference> references;
        lock (artifactIds)
        {
            references = [];
            foreach (var artifactId in artifactIds)
            {
                if (_artifacts.TryGetValue(artifactId, out var entry))
                {
                    references.Add(entry.Reference);
                }
            }
        }

        return Task.FromResult<IReadOnlyList<ArtifactReference>>(references);
    }

    public Task DeleteAsync(
        ArtifactReference reference,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (_artifacts.TryRemove(reference.ArtifactId, out var entry))
        {
            File.Delete(entry.Path);
        }

        if (_moduleIndex.TryGetValue(reference.ModuleTypeName, out var artifactIds))
        {
            lock (artifactIds)
            {
                artifactIds.Remove(reference.ArtifactId);
            }
        }

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        if (!_root.IsValueCreated)
        {
            return;
        }

        try
        {
            Directory.Delete(_root.Value, recursive: true);
        }
        catch (Exception exception) when (exception is IOException or UnauthorizedAccessException)
        {
            // Best-effort cleanup must not fail pipeline disposal.
        }
    }
}
