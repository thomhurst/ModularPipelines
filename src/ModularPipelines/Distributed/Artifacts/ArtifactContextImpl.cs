using System.IO.Compression;
using ModularPipelines.Logging;
using ModularPipelines.Modules;

namespace ModularPipelines.Distributed.Artifacts;

/// <summary>
/// Implementation of <see cref="IArtifactContext"/> wrapping <see cref="IDistributedArtifactStore"/>
/// with convenience methods for file and directory operations.
/// </summary>
internal class ArtifactContextImpl : IArtifactContext, IModuleScopedArtifactContext
{
    private readonly IDistributedArtifactStore _store;
    private readonly ArtifactOptions _options;
    private readonly string? _moduleTypeName;

    public ArtifactContextImpl(
        IDistributedArtifactStore store,
        ArtifactOptions options)
    {
        _store = store;
        _options = options;
    }

    private ArtifactContextImpl(
        IDistributedArtifactStore store,
        ArtifactOptions options,
        string moduleTypeName)
        : this(store, options)
    {
        _moduleTypeName = moduleTypeName;
    }

    public IArtifactContext ForModule(Type moduleType)
        => new ArtifactContextImpl(_store, _options, moduleType.FullName ?? moduleType.Name);

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
            await CreateDirectoryArchiveAsync(
                directoryPath,
                temporaryArchivePath,
                _options.CompressionLevel,
                cancellationToken);
            await using var stream = File.OpenRead(temporaryArchivePath);
            return await _store.UploadAsync(descriptor, stream, cancellationToken);
        }
        finally
        {
            File.Delete(temporaryArchivePath);
        }
    }

    internal static async Task CreateDirectoryArchiveAsync(
        string directoryPath,
        string archivePath,
        CompressionLevel compressionLevel,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var sourceDirectory = Path.GetFullPath(directoryPath);
        var fullArchivePath = Path.GetFullPath(archivePath);
        var pathComparison = OperatingSystem.IsWindows()
            ? StringComparison.OrdinalIgnoreCase
            : StringComparison.Ordinal;

        using var archive = ZipFile.Open(fullArchivePath, ZipArchiveMode.Create);
        var resolvedArchivePath = ResolveSymbolicLinks(fullArchivePath);
        foreach (var directory in Directory.EnumerateDirectories(
                     sourceDirectory,
                     "*",
                     SearchOption.AllDirectories))
        {
            cancellationToken.ThrowIfCancellationRequested();
            var entryName = Path.GetRelativePath(sourceDirectory, directory)
                .Replace(Path.DirectorySeparatorChar, '/')
                .TrimEnd('/') + "/";
            archive.CreateEntry(entryName, compressionLevel);
        }

        foreach (var file in Directory.EnumerateFiles(
                     sourceDirectory,
                     "*",
                     SearchOption.AllDirectories))
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (string.Equals(Path.GetFullPath(file), fullArchivePath, pathComparison)
                || string.Equals(ResolveSymbolicLinks(file), resolvedArchivePath, pathComparison))
            {
                continue;
            }

            var entryName = Path.GetRelativePath(sourceDirectory, file)
                .Replace(Path.DirectorySeparatorChar, '/');
            var entry = archive.CreateEntry(entryName, compressionLevel);
            entry.LastWriteTime = File.GetLastWriteTime(file);
            await using var sourceStream = new FileStream(
                file,
                new FileStreamOptions
                {
                    Access = FileAccess.Read,
                    Mode = FileMode.Open,
                    Options = FileOptions.Asynchronous | FileOptions.SequentialScan,
                });
            await using var entryStream = entry.Open();
            await sourceStream.CopyToAsync(entryStream, cancellationToken);
        }
    }

    private static string ResolveSymbolicLinks(string path)
    {
        var fullPath = Path.GetFullPath(path);
        var root = Path.GetPathRoot(fullPath)
                   ?? throw new ArgumentException("Path must have a root.", nameof(path));
        var resolvedPath = root;

        foreach (var segment in fullPath[root.Length..].Split(
                     [Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar],
                     StringSplitOptions.RemoveEmptyEntries))
        {
            resolvedPath = Path.Combine(resolvedPath, segment);
            FileSystemInfo entry = Directory.Exists(resolvedPath)
                ? new DirectoryInfo(resolvedPath)
                : new FileInfo(resolvedPath);
            resolvedPath = entry.ResolveLinkTarget(returnFinalTarget: true)?.FullName
                           ?? resolvedPath;
        }

        return Path.GetFullPath(resolvedPath);
    }

    public async Task<string> DownloadAsync(string producerModuleTypeName, string artifactName, string destinationPath, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var artifacts = await _store.ListArtifactsAsync(producerModuleTypeName, cancellationToken);
        var artifact = artifacts
            .Where(a => a.Name == artifactName)
            .OrderByDescending(static a => a.UploadedAt)
            .FirstOrDefault()
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

    private string GetCurrentModuleTypeName()
        => _moduleTypeName
           ?? ModuleLogger.CurrentModuleType.Value?.FullName
           ?? throw new InvalidOperationException(
               "Artifacts can only be published while a module is executing.");
}

internal interface IModuleScopedArtifactContext
{
    IArtifactContext ForModule(Type moduleType);
}
