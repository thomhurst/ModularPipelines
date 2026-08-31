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
        File.Delete(fullArchivePath);

        var directories = new List<string>();
        foreach (var directory in Directory.EnumerateDirectories(
                     sourceDirectory,
                     "*",
                     SearchOption.AllDirectories))
        {
            cancellationToken.ThrowIfCancellationRequested();
            directories.Add(directory);
        }

        var files = new List<string>();
        foreach (var file in Directory.EnumerateFiles(
                     sourceDirectory,
                     "*",
                     SearchOption.AllDirectories))
        {
            cancellationToken.ThrowIfCancellationRequested();
            files.Add(file);
        }

        using var archive = ZipFile.Open(fullArchivePath, ZipArchiveMode.Create);
        foreach (var directory in directories)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var entryName = Path.GetRelativePath(sourceDirectory, directory)
                .Replace(Path.DirectorySeparatorChar, '/')
                .TrimEnd('/') + "/";
            archive.CreateEntry(entryName, compressionLevel);
        }

        foreach (var file in files)
        {
            cancellationToken.ThrowIfCancellationRequested();
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

    internal static StringComparison GetArchivePathComparison() =>
        OperatingSystem.IsWindows()
            ? StringComparison.OrdinalIgnoreCase
            : StringComparison.Ordinal;

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
            using var archive = new ZipArchive(stream, ZipArchiveMode.Read);
            await ExtractDirectoryArchiveAsync(archive, destinationPath, cancellationToken);
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

    internal static async Task ExtractDirectoryArchiveAsync(
        ZipArchive archive,
        string destinationPath,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var destinationDirectory = Path.GetFullPath(destinationPath);
        var destinationPrefix = Path.EndsInDirectorySeparator(destinationDirectory)
            ? destinationDirectory
            : destinationDirectory + Path.DirectorySeparatorChar;
        var pathComparison = GetArchivePathComparison();
        CreateDirectoryWithoutLinks(destinationDirectory, destinationDirectory);

        foreach (var entry in archive.Entries)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var entryPath = Path.GetFullPath(Path.Combine(destinationDirectory, entry.FullName));
            if (!entryPath.StartsWith(destinationPrefix, pathComparison)
                && !string.Equals(entryPath, destinationDirectory, pathComparison))
            {
                throw new IOException($"Extracting '{entry.FullName}' would leave the destination directory.");
            }

            if (string.IsNullOrEmpty(entry.Name))
            {
                CreateDirectoryWithoutLinks(destinationDirectory, entryPath);
                continue;
            }

            var entryDirectory = Path.GetDirectoryName(entryPath);
            if (!string.IsNullOrEmpty(entryDirectory))
            {
                CreateDirectoryWithoutLinks(destinationDirectory, entryDirectory);
            }

            EnsurePathContainsNoLinks(destinationDirectory, entryPath);

            await using (var entryStream = entry.Open())
            await using (var destinationStream = new FileStream(
                             entryPath,
                             new FileStreamOptions
                             {
                                 Access = FileAccess.Write,
                                 Mode = FileMode.Create,
                                 Options = FileOptions.Asynchronous | FileOptions.SequentialScan,
                             }))
            {
                await entryStream.CopyToAsync(destinationStream, cancellationToken);
            }

            File.SetLastWriteTime(entryPath, entry.LastWriteTime.DateTime);
        }
    }

    private static void CreateDirectoryWithoutLinks(string destinationDirectory, string path)
    {
        EnsurePathContainsNoLinks(destinationDirectory, path);
        Directory.CreateDirectory(path);
        EnsurePathContainsNoLinks(destinationDirectory, path);
    }

    private static void EnsurePathContainsNoLinks(string destinationDirectory, string path)
    {
        var currentPath = destinationDirectory;
        try
        {
            EnsurePathIsNotLink(currentPath);
        }
        catch (Exception exception) when (exception is FileNotFoundException or DirectoryNotFoundException)
        {
            return;
        }

        var relativePath = Path.GetRelativePath(destinationDirectory, path);
        if (relativePath == ".")
        {
            return;
        }

        foreach (var segment in relativePath.Split(
                     [Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar],
                     StringSplitOptions.RemoveEmptyEntries))
        {
            currentPath = Path.Combine(currentPath, segment);
            try
            {
                EnsurePathIsNotLink(currentPath);
            }
            catch (Exception exception) when (exception is FileNotFoundException or DirectoryNotFoundException)
            {
                return;
            }
        }
    }

    private static void EnsurePathIsNotLink(string path)
    {
        if ((File.GetAttributes(path) & FileAttributes.ReparsePoint) != 0)
        {
            throw new IOException($"Extracting through linked path '{path}' is not allowed.");
        }
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
