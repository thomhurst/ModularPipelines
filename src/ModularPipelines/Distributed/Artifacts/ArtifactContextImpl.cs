using System.IO.Compression;
using ModularPipelines.Logging;
using ModularPipelines.Modules;

namespace ModularPipelines.Distributed.Artifacts;

/// <summary>
/// Implementation of <see cref="IArtifactContext"/> wrapping <see cref="IDistributedArtifactStore"/>
/// with convenience methods for file and directory operations.
/// </summary>
internal class ArtifactContextImpl : IArtifactContext
{
    private readonly IDistributedArtifactStore _store;
    private readonly ArtifactOptions _options;

    public ArtifactContextImpl(
        IDistributedArtifactStore store,
        ArtifactOptions options)
    {
        _store = store;
        _options = options;
    }

    public async Task<ArtifactReference> PublishFileAsync(string artifactName, string filePath, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var descriptor = new ArtifactDescriptor(
            Name: artifactName,
            ModuleTypeName: GetCurrentModuleTypeName(),
            ContentType: "application/octet-stream");

        await using var stream = File.OpenRead(filePath);
        return await _store.UploadAsync(descriptor, stream, cancellationToken);
    }

    public async Task<ArtifactReference> PublishDirectoryAsync(string artifactName, string directoryPath, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var descriptor = new ArtifactDescriptor(
            Name: artifactName,
            ModuleTypeName: GetCurrentModuleTypeName(),
            ContentType: "application/zip");

        var temporaryArchivePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.zip");
        try
        {
            ZipFile.CreateFromDirectory(
                directoryPath,
                temporaryArchivePath,
                _options.CompressionLevel,
                includeBaseDirectory: false);
            await using var stream = File.OpenRead(temporaryArchivePath);
            return await _store.UploadAsync(descriptor, stream, cancellationToken);
        }
        finally
        {
            File.Delete(temporaryArchivePath);
        }
    }

    public async Task<string> DownloadAsync(string producerModuleTypeName, string artifactName, string destinationPath, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var artifacts = await _store.ListArtifactsAsync(producerModuleTypeName, cancellationToken);
        var artifact = artifacts.FirstOrDefault(a => a.Name == artifactName)
            ?? throw new InvalidOperationException(
                $"Artifact '{artifactName}' from module '{producerModuleTypeName}' not found.");

        await using var stream = await _store.DownloadAsync(artifact, cancellationToken);

        if (artifact.ContentType == "application/zip")
        {
            Directory.CreateDirectory(destinationPath);
            using var archive = new ZipArchive(stream, ZipArchiveMode.Read);
            archive.ExtractToDirectory(destinationPath, overwriteFiles: true);
            return destinationPath;
        }

        var destinationDirectory = Path.GetDirectoryName(destinationPath);
        if (!string.IsNullOrEmpty(destinationDirectory))
        {
            Directory.CreateDirectory(destinationDirectory);
        }

        await using var fileStream = File.Create(destinationPath);
        await stream.CopyToAsync(fileStream, cancellationToken);
        return destinationPath;
    }

    public Task<string> DownloadAsync<TProducerModule>(
        string artifactName,
        string destinationPath,
        CancellationToken cancellationToken = default)
        where TProducerModule : IModule
        => DownloadAsync(
            typeof(TProducerModule).FullName!,
            artifactName,
            destinationPath,
            cancellationToken);

    private static string GetCurrentModuleTypeName()
        => ModuleLogger.CurrentModuleType.Value?.FullName
           ?? throw new InvalidOperationException(
               "Artifacts can only be published while a module is executing.");
}
