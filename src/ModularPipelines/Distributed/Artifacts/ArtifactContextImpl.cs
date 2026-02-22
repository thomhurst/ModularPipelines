using System.IO.Compression;
using Microsoft.Extensions.Options;

namespace ModularPipelines.Distributed.Artifacts;

/// <summary>
/// Implementation of <see cref="IArtifactContext"/> wrapping <see cref="IDistributedArtifactStore"/>
/// with convenience methods for file and directory operations.
/// </summary>
internal class ArtifactContextImpl : IArtifactContext
{
    private readonly IDistributedArtifactStore _store;
    private readonly ArtifactOptions _options;
    private readonly string _currentModuleTypeName;

    public ArtifactContextImpl(
        IDistributedArtifactStore store,
        IOptions<ArtifactOptions> options,
        string currentModuleTypeName)
    {
        _store = store;
        _options = options.Value;
        _currentModuleTypeName = currentModuleTypeName;
    }

    public async Task<ArtifactReference> PublishFileAsync(string artifactName, string filePath, CancellationToken cancellationToken)
    {
        var descriptor = new ArtifactDescriptor(
            Name: artifactName,
            ModuleTypeName: _currentModuleTypeName,
            ContentType: "application/octet-stream");

        await using var stream = File.OpenRead(filePath);
        return await _store.UploadAsync(descriptor, stream, cancellationToken);
    }

    public async Task<ArtifactReference> PublishDirectoryAsync(string artifactName, string directoryPath, CancellationToken cancellationToken)
    {
        var descriptor = new ArtifactDescriptor(
            Name: artifactName,
            ModuleTypeName: _currentModuleTypeName,
            ContentType: "application/zip");

        using var ms = new MemoryStream();
        ZipFile.CreateFromDirectory(directoryPath, ms, _options.CompressionLevel, includeBaseDirectory: false);
        ms.Position = 0;
        return await _store.UploadAsync(descriptor, ms, cancellationToken);
    }

    public async Task<string> DownloadAsync(string producerModuleTypeName, string artifactName, string destinationPath, CancellationToken cancellationToken)
    {
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

        Directory.CreateDirectory(Path.GetDirectoryName(destinationPath)!);
        await using var fileStream = File.Create(destinationPath);
        await stream.CopyToAsync(fileStream, cancellationToken);
        return destinationPath;
    }
}
