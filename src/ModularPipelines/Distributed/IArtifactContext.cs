using ModularPipelines.Modules;

namespace ModularPipelines.Distributed;

/// <summary>
/// Module-facing API for artifact publishing and downloading.
/// Access via <c>context.Artifacts</c>.
/// </summary>
public interface IArtifactContext
{
    /// <summary>
    /// Publishes a file as a named artifact.
    /// </summary>
    /// <param name="artifactName">The artifact name.</param>
    /// <param name="filePath">The file to publish.</param>
    /// <param name="cancellationToken">A token that cancels the operation.</param>
    /// <returns>The published artifact reference.</returns>
    Task<ArtifactReference> PublishFileAsync(
        string artifactName,
        string filePath,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Publishes a directory as a named artifact (compressed as a zip archive).
    /// </summary>
    /// <param name="artifactName">The artifact name.</param>
    /// <param name="directoryPath">The directory to publish.</param>
    /// <param name="cancellationToken">A token that cancels the operation.</param>
    /// <returns>The published artifact reference.</returns>
    Task<ArtifactReference> PublishDirectoryAsync(
        string artifactName,
        string directoryPath,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Downloads a named artifact from a specific producer module to a local path.
    /// </summary>
    /// <param name="producerModuleTypeName">The full name of the module that produced the artifact.</param>
    /// <param name="artifactName">The artifact name.</param>
    /// <param name="destinationPath">The local destination path.</param>
    /// <param name="cancellationToken">A token that cancels the operation.</param>
    /// <returns>The local path where the artifact was downloaded.</returns>
    Task<string> DownloadAsync(
        string producerModuleTypeName,
        string artifactName,
        string destinationPath,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Downloads a named artifact from a producer module to a local path.
    /// </summary>
    /// <typeparam name="TProducerModule">The module that produced the artifact.</typeparam>
    /// <param name="artifactName">The artifact name.</param>
    /// <param name="destinationPath">The local destination path.</param>
    /// <param name="cancellationToken">A token that cancels the operation.</param>
    /// <returns>The local path where the artifact was downloaded.</returns>
    Task<string> DownloadAsync<TProducerModule>(
        string artifactName,
        string destinationPath,
        CancellationToken cancellationToken = default)
        where TProducerModule : IModule;
}
