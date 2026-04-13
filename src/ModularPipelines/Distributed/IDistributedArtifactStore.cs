namespace ModularPipelines.Distributed;

/// <summary>
/// Defines the storage layer for distributed artifact sharing.
/// Implement this interface to provide a custom artifact transport (Redis, S3, Azure Blob, etc.).
/// All operations are scoped to a run identifier for isolation between concurrent pipeline runs.
/// </summary>
public interface IDistributedArtifactStore
{
    /// <summary>
    /// Uploads an artifact from a stream.
    /// </summary>
    Task<ArtifactReference> UploadAsync(ArtifactDescriptor descriptor, Stream data, CancellationToken cancellationToken);

    /// <summary>
    /// Downloads an artifact as a stream.
    /// </summary>
    Task<Stream> DownloadAsync(ArtifactReference reference, CancellationToken cancellationToken);

    /// <summary>
    /// Lists all artifacts produced by a specific module.
    /// </summary>
    Task<IReadOnlyList<ArtifactReference>> ListArtifactsAsync(string moduleTypeName, CancellationToken cancellationToken);

    /// <summary>
    /// Deletes an artifact.
    /// </summary>
    Task DeleteAsync(ArtifactReference reference, CancellationToken cancellationToken);
}
