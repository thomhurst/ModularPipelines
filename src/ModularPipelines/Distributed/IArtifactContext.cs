namespace ModularPipelines.Distributed;

/// <summary>
/// Module-facing API for artifact publishing and downloading.
/// Access via <c>context.Artifacts()</c> extension method.
/// </summary>
public interface IArtifactContext
{
    /// <summary>
    /// Publishes a file as a named artifact.
    /// </summary>
    Task<ArtifactReference> PublishFileAsync(string artifactName, string filePath, CancellationToken cancellationToken);

    /// <summary>
    /// Publishes a directory as a named artifact (compressed as a zip archive).
    /// </summary>
    Task<ArtifactReference> PublishDirectoryAsync(string artifactName, string directoryPath, CancellationToken cancellationToken);

    /// <summary>
    /// Downloads a named artifact from a specific producer module to a local path.
    /// </summary>
    /// <returns>The local path where the artifact was downloaded.</returns>
    Task<string> DownloadAsync(string producerModuleTypeName, string artifactName, string destinationPath, CancellationToken cancellationToken);
}
