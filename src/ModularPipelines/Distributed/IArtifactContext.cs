using ModularPipelines.FileSystem;
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
#pragma warning disable RS0026 // Path overloads intentionally share optional cancellation tokens.
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
    /// <param name="producerModuleId">The stable identifier of the module that produced the artifact.</param>
    /// <param name="artifactName">The artifact name.</param>
    /// <param name="destinationPath">The local destination path.</param>
    /// <param name="cancellationToken">A token that cancels the operation.</param>
    /// <returns>The local path where the artifact was downloaded.</returns>
    Task<string> DownloadAsync(
        ModuleId producerModuleId,
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

    /// <summary>
    /// Publishes a file through its file-system provider.
    /// </summary>
    /// <param name="artifactName">The artifact name.</param>
    /// <param name="filePath">The resolved path and file-system provider to use.</param>
    /// <param name="cancellationToken">A token that cancels the operation.</param>
    /// <returns>The published artifact reference.</returns>
    Task<ArtifactReference> PublishFileAsync(
        string artifactName,
        FilePath filePath,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Publishes a directory through its file-system provider.
    /// </summary>
    /// <param name="artifactName">The artifact name.</param>
    /// <param name="directoryPath">The resolved path and file-system provider to use.</param>
    /// <param name="cancellationToken">A token that cancels the operation.</param>
    /// <returns>The published artifact reference.</returns>
    Task<ArtifactReference> PublishDirectoryAsync(
        string artifactName,
        FolderPath directoryPath,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Downloads a file artifact to the supplied typed path.
    /// </summary>
    /// <param name="producerModuleId">The producer module identifier.</param>
    /// <param name="artifactName">The artifact name.</param>
    /// <param name="destinationPath">The resolved path and file-system provider to use.</param>
    /// <param name="cancellationToken">A token that cancels the operation.</param>
    /// <returns>The supplied destination path after the download completes.</returns>
    /// <exception cref="InvalidOperationException">The artifact kind does not match the destination path type.</exception>
    Task<FilePath> DownloadAsync(
        ModuleId producerModuleId,
        string artifactName,
        FilePath destinationPath,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Downloads a directory artifact to the supplied typed path.
    /// </summary>
    /// <param name="producerModuleId">The producer module identifier.</param>
    /// <param name="artifactName">The artifact name.</param>
    /// <param name="destinationPath">The resolved path and file-system provider to use.</param>
    /// <param name="cancellationToken">A token that cancels the operation.</param>
    /// <returns>The supplied destination path after the download completes.</returns>
    /// <exception cref="InvalidOperationException">The artifact kind does not match the destination path type.</exception>
    Task<FolderPath> DownloadAsync(
        ModuleId producerModuleId,
        string artifactName,
        FolderPath destinationPath,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Downloads a file artifact to the supplied typed path.
    /// </summary>
    /// <typeparam name="TProducerModule">The module that produced the artifact.</typeparam>
    /// <param name="artifactName">The artifact name.</param>
    /// <param name="destinationPath">The resolved path and file-system provider to use.</param>
    /// <param name="cancellationToken">A token that cancels the operation.</param>
    /// <returns>The supplied destination path after the download completes.</returns>
    /// <exception cref="InvalidOperationException">The artifact kind does not match the destination path type.</exception>
    Task<FilePath> DownloadAsync<TProducerModule>(
        string artifactName,
        FilePath destinationPath,
        CancellationToken cancellationToken = default)
        where TProducerModule : IModule;

    /// <summary>
    /// Downloads a directory artifact to the supplied typed path.
    /// </summary>
    /// <typeparam name="TProducerModule">The module that produced the artifact.</typeparam>
    /// <param name="artifactName">The artifact name.</param>
    /// <param name="destinationPath">The resolved path and file-system provider to use.</param>
    /// <param name="cancellationToken">A token that cancels the operation.</param>
    /// <returns>The supplied destination path after the download completes.</returns>
    /// <exception cref="InvalidOperationException">The artifact kind does not match the destination path type.</exception>
    Task<FolderPath> DownloadAsync<TProducerModule>(
        string artifactName,
        FolderPath destinationPath,
        CancellationToken cancellationToken = default)
        where TProducerModule : IModule;
#pragma warning restore RS0026
}
