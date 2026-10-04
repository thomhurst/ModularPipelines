using System.IO.Compression;
using ModularPipelines.FileSystem;
using ModularPipelines.Logging;
using ModularPipelines.Modules;

namespace ModularPipelines.Distributed.Artifacts;

/// <summary>
/// Implementation of <see cref="IArtifactContext"/> wrapping <see cref="IDistributedArtifactStore"/>
/// with convenience methods for file and directory operations.
/// </summary>
internal class ArtifactContextImpl(
    IDistributedArtifactStore store,
    DistributedOptions options,
    AcceptedArtifactRegistry? acceptedArtifacts = null) : IArtifactContext, IModuleScopedArtifactContext
{
    private readonly IDistributedArtifactStore _store = store;
    private readonly DistributedOptions _options = options;
    private readonly AcceptedArtifactRegistry? _acceptedArtifacts = acceptedArtifacts;
    private readonly ModuleId? _moduleId;

    private ArtifactContextImpl(
        IDistributedArtifactStore store,
        DistributedOptions options,
        AcceptedArtifactRegistry? acceptedArtifacts,
        ModuleId moduleId)
        : this(store, options, acceptedArtifacts)
    {
        _moduleId = moduleId;
    }

    public IArtifactContext ForModule(Type moduleType)
        => new ArtifactContextImpl(_store, _options, _acceptedArtifacts, ModuleId.FromType(moduleType));

    public Task<ArtifactReference> PublishFileAsync(string artifactName, string filePath, CancellationToken cancellationToken)
        => PublishFileAsync(artifactName, filePath, SystemFileSystemProvider.Instance, cancellationToken);

    public Task<ArtifactReference> PublishFileAsync(string artifactName, FilePath filePath, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filePath);
        return PublishFileAsync(artifactName, filePath.Path, filePath.Provider, cancellationToken);
    }

    private async Task<ArtifactReference> PublishFileAsync(string artifactName, string filePath, IFileSystemProvider provider, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var descriptor = new ArtifactDescriptor
        {
            Name = artifactName,
            ModuleId = GetCurrentModuleId(),
            ContentType = "application/octet-stream",
        };

        var stream = provider.OpenRead(filePath);
        await using var streamLifetime = stream.ConfigureAwait(false);
        return await _store.UploadAsync(descriptor, stream, cancellationToken).ConfigureAwait(false);
    }

    public Task<ArtifactReference> PublishDirectoryAsync(string artifactName, string directoryPath, CancellationToken cancellationToken)
        => PublishDirectoryAsync(artifactName, directoryPath, SystemFileSystemProvider.Instance, cancellationToken);

    public Task<ArtifactReference> PublishDirectoryAsync(string artifactName, FolderPath directoryPath, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(directoryPath);
        return PublishDirectoryAsync(artifactName, directoryPath.Path, directoryPath.Provider, cancellationToken);
    }

    private async Task<ArtifactReference> PublishDirectoryAsync(string artifactName, string directoryPath, IFileSystemProvider provider, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var descriptor = new ArtifactDescriptor
        {
            Name = artifactName,
            ModuleId = GetCurrentModuleId(),
            ContentType = "application/zip",
        };

        var temporaryArchivePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.zip");
        try
        {
            await CreateDirectoryArchiveAsync(
                directoryPath,
                temporaryArchivePath,
                _options.ArtifactCompressionLevel,
                cancellationToken,
                provider).ConfigureAwait(false);
            var stream = File.OpenRead(temporaryArchivePath);
            await using var streamLifetime = stream.ConfigureAwait(false);
            return await _store.UploadAsync(descriptor, stream, cancellationToken).ConfigureAwait(false);
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
        CancellationToken cancellationToken,
        IFileSystemProvider? provider = null)
    {
        provider ??= SystemFileSystemProvider.Instance;
        cancellationToken.ThrowIfCancellationRequested();
        var sourceDirectory = provider is SystemFileSystemProvider ? Path.GetFullPath(directoryPath) : directoryPath;
        var fullArchivePath = Path.GetFullPath(archivePath);
        File.Delete(fullArchivePath);

        var directories = new List<string>();
        foreach (var directory in provider.EnumerateDirectories(
                     sourceDirectory,
                     "*",
                     SearchOption.AllDirectories))
        {
            cancellationToken.ThrowIfCancellationRequested();
            directories.Add(directory);
        }

        var files = new List<string>();
        foreach (var file in provider.EnumerateFiles(
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
            var entryName = GetArchiveEntryName(provider, sourceDirectory, directory).TrimEnd('/') + "/";
            archive.CreateEntry(entryName, compressionLevel);
        }

        foreach (var file in files)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var entryName = GetArchiveEntryName(provider, sourceDirectory, file);
            var entry = archive.CreateEntry(entryName, compressionLevel);
            entry.LastWriteTime = provider.GetLastWriteTimeUtc(file).ToLocalTime();
            if (provider is SystemFileSystemProvider && !OperatingSystem.IsWindows())
            {
                // Keep the regular-file type so mode 000 is distinct from absent Unix metadata.
                entry.ExternalAttributes = (0x8000 | (int) File.GetUnixFileMode(file)) << 16;
            }

            var sourceStream = provider is SystemFileSystemProvider ? new FileStream(
                file,
                new FileStreamOptions
                {
                    Access = FileAccess.Read,
                    Mode = FileMode.Open,
                    Options = FileOptions.Asynchronous | FileOptions.SequentialScan,
                }) : provider.OpenRead(file);
            await using var sourceStreamLifetime = sourceStream.ConfigureAwait(false);
            var entryStream = entry.Open();
            await using var entryStreamLifetime = entryStream.ConfigureAwait(false);
            await sourceStream.CopyToAsync(entryStream, cancellationToken).ConfigureAwait(false);
        }
    }

    private static string GetArchiveEntryName(IFileSystemProvider provider, string directory, string path)
    {
        var relativePath = provider.GetRelativePath(directory, path);
        // Only the declared separator separates directories; other characters belong to the file name.
        return relativePath.Replace(provider.DirectorySeparatorChar, '/');
    }

    internal static StringComparison GetArchivePathComparison(IFileSystemProvider? provider = null) =>
        OperatingSystem.IsWindows() && (provider is null or SystemFileSystemProvider)
            ? StringComparison.OrdinalIgnoreCase
            : StringComparison.Ordinal;

    public Task<string> DownloadAsync(ModuleId producerModuleId, string artifactName, string destinationPath, CancellationToken cancellationToken)
        => DownloadAsync(producerModuleId, artifactName, destinationPath, SystemFileSystemProvider.Instance, null, cancellationToken);

    private async Task<string> DownloadAsync(ModuleId producerModuleId, string artifactName, string destinationPath, IFileSystemProvider provider, bool? directory, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var artifact = await ArtifactLifecycleManager.ResolveArtifactAsync(
                _store,
                _acceptedArtifacts,
                producerModuleId,
                artifactName,
                cancellationToken)
            .ConfigureAwait(false)
            ?? throw new InvalidOperationException(
                $"Artifact '{artifactName}' from module '{producerModuleId}' not found.");

        if (directory.HasValue && directory.Value != (artifact.ContentType == "application/zip"))
        {
            throw new InvalidOperationException($"Artifact '{artifactName}' does not match the destination path type.");
        }

        var stream = await _store.DownloadAsync(artifact, cancellationToken).ConfigureAwait(false);
        await using var streamLifetime = stream.ConfigureAwait(false);

        if (artifact.ContentType == "application/zip")
        {
            using var archive = new ZipArchive(stream, ZipArchiveMode.Read);
            await ExtractDirectoryArchiveAsync(archive, destinationPath, cancellationToken, provider).ConfigureAwait(false);
            return destinationPath;
        }

        var destinationDirectory = Path.GetDirectoryName(destinationPath);
        if (!string.IsNullOrEmpty(destinationDirectory))
        {
            provider.CreateDirectory(destinationDirectory);
        }

        var fileStream = provider.Create(destinationPath);
        await using var fileStreamLifetime = fileStream.ConfigureAwait(false);
        await stream.CopyToAsync(fileStream, cancellationToken).ConfigureAwait(false);
        return destinationPath;
    }

    internal static async Task ExtractDirectoryArchiveAsync(
        ZipArchive archive,
        string destinationPath,
        CancellationToken cancellationToken,
        IFileSystemProvider? provider = null)
    {
        provider ??= SystemFileSystemProvider.Instance;
        cancellationToken.ThrowIfCancellationRequested();
        var destinationDirectory = provider is SystemFileSystemProvider ? Path.GetFullPath(destinationPath) : destinationPath;
        var destinationPrefix = Path.EndsInDirectorySeparator(destinationDirectory)
            ? destinationDirectory
            : destinationDirectory + Path.DirectorySeparatorChar;
        var pathComparison = GetArchivePathComparison(provider);
        CreateDirectoryWithoutLinks(provider, destinationDirectory, destinationDirectory);

        foreach (var entry in archive.Entries)
        {
            cancellationToken.ThrowIfCancellationRequested();
            // Normalize provider separators before host canonicalization and containment checks.
            // A slash-based Unix provider can retain a literal backslash in an entry name.
            var entryName = entry.FullName.Replace(provider.DirectorySeparatorChar, '/');
            var isDirectory = entryName.EndsWith('/');
            var entryPath = Path.GetFullPath(Path.Combine(destinationDirectory, entryName));
            // A root directory entry needs no work; every other entry must be below it.
            if (isDirectory && string.Equals(entryPath, destinationDirectory, pathComparison))
            {
                continue;
            }

            if (!entryPath.StartsWith(destinationPrefix, pathComparison))
            {
                throw new IOException($"Extracting '{entry.FullName}' would leave the destination directory.");
            }

            if (isDirectory)
            {
                CreateDirectoryWithoutLinks(provider, destinationDirectory, entryPath);
                continue;
            }

            await ExtractArchiveFileAsync(
                entry, provider, destinationDirectory, destinationPrefix, entryPath, pathComparison, cancellationToken)
                .ConfigureAwait(false);
        }
    }

    private static async Task ExtractArchiveFileAsync(
        ZipArchiveEntry entry,
        IFileSystemProvider provider,
        string destinationDirectory,
        string destinationPrefix,
        string entryPath,
        StringComparison pathComparison,
        CancellationToken cancellationToken)
    {
        var entryDirectory = Path.GetDirectoryName(entryPath);
        if (!string.IsNullOrEmpty(entryDirectory))
        {
            CreateDirectoryWithoutLinks(provider, destinationDirectory, entryDirectory);
        }

        EnsurePathContainsNoLinks(provider, destinationDirectory, entryPath);

        var fileOptions = new FileStreamOptions
        {
            Access = FileAccess.Write,
            Mode = FileMode.CreateNew,
            Options = FileOptions.Asynchronous | FileOptions.SequentialScan,
        };
        if (provider is SystemFileSystemProvider && !OperatingSystem.IsWindows())
        {
            // Apply ordinary permissions at creation so the OS enforces the umask.
            // Never propagate setuid, setgid, or sticky bits from ZIPs.
            var unixAttributes = (entry.ExternalAttributes >> 16) & 0xFFFF;
            if (unixAttributes != 0)
            {
                var permissionBits = unixAttributes & 0x1FF;
                fileOptions.UnixCreateMode = (UnixFileMode) permissionBits;
            }
        }

        // A new sibling file receives the archive mode through the OS umask even
        // when replacing an existing destination. Cancellation leaves that file intact.
        var temporaryPath = Path.Combine(entryDirectory!, $".modularpipelines-extract-{Guid.NewGuid():N}.tmp");
        if (provider is SystemFileSystemProvider)
        {
            temporaryPath = Path.GetFullPath(temporaryPath);
        }
        if (!temporaryPath.StartsWith(destinationPrefix, pathComparison))
        {
            throw new IOException("The archive temporary file would leave the destination directory.");
        }

        var destinationStream = provider is SystemFileSystemProvider
            ? new FileStream(temporaryPath, fileOptions)
            : provider.Open(temporaryPath, FileMode.CreateNew, FileAccess.Write);
        try
        {
            await using (destinationStream.ConfigureAwait(false))
            {
                var entryStream = entry.Open();
                await using (entryStream.ConfigureAwait(false))
                {
                    await entryStream.CopyToAsync(destinationStream, cancellationToken).ConfigureAwait(false);
                }
            }

            cancellationToken.ThrowIfCancellationRequested();
            provider.SetLastWriteTimeUtc(temporaryPath, entry.LastWriteTime.DateTime.ToUniversalTime());
            EnsurePathContainsNoLinks(provider, destinationDirectory, entryPath);
            provider.MoveFile(temporaryPath, entryPath, overwrite: true);
        }
        finally
        {
            provider.DeleteFile(temporaryPath);
        }
    }

    private static void CreateDirectoryWithoutLinks(IFileSystemProvider provider, string destinationDirectory, string path)
    {
        EnsurePathContainsNoLinks(provider, destinationDirectory, path);
        provider.CreateDirectory(path);
        EnsurePathContainsNoLinks(provider, destinationDirectory, path);
    }

    private static void EnsurePathContainsNoLinks(IFileSystemProvider provider, string destinationDirectory, string path)
    {
        var currentPath = destinationDirectory;
        try
        {
            EnsurePathIsNotLink(provider, currentPath);
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
                EnsurePathIsNotLink(provider, currentPath);
            }
            catch (Exception exception) when (exception is FileNotFoundException or DirectoryNotFoundException)
            {
                return;
            }
        }
    }

    private static void EnsurePathIsNotLink(IFileSystemProvider provider, string path)
    {
        // File.GetAttributes throws for missing paths, unlike FileSystemInfo.Attributes.
        var attributes = provider is SystemFileSystemProvider ? File.GetAttributes(path) : provider.GetAttributes(path);
        if ((attributes & FileAttributes.ReparsePoint) != 0)
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
            ModuleId.FromType(typeof(TProducerModule)),
            artifactName,
            destinationPath,
            cancellationToken);

    public async Task<FilePath> DownloadAsync(ModuleId producerModuleId, string artifactName, FilePath destinationPath, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(destinationPath);
        await DownloadAsync(producerModuleId, artifactName, destinationPath.Path, destinationPath.Provider, false, cancellationToken).ConfigureAwait(false);
        return destinationPath;
    }

    public Task<FilePath> DownloadAsync<TProducerModule>(string artifactName, FilePath destinationPath, CancellationToken cancellationToken = default)
        where TProducerModule : IModule
        => DownloadAsync(ModuleId.FromType(typeof(TProducerModule)), artifactName, destinationPath, cancellationToken);

    public async Task<FolderPath> DownloadAsync(ModuleId producerModuleId, string artifactName, FolderPath destinationPath, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(destinationPath);
        await DownloadAsync(producerModuleId, artifactName, destinationPath.Path, destinationPath.Provider, true, cancellationToken).ConfigureAwait(false);
        return destinationPath;
    }

    public Task<FolderPath> DownloadAsync<TProducerModule>(string artifactName, FolderPath destinationPath, CancellationToken cancellationToken = default)
        where TProducerModule : IModule
        => DownloadAsync(ModuleId.FromType(typeof(TProducerModule)), artifactName, destinationPath, cancellationToken);

    private ModuleId GetCurrentModuleId()
        => _moduleId
           ?? (AmbientModuleOutputContext.Current?.ModuleType is { } moduleType
               ? (ModuleId?) ModuleId.FromType(moduleType)
               : null)
           ?? throw new InvalidOperationException(
               "Artifacts can only be published while a module is executing.");
}

internal interface IModuleScopedArtifactContext
{
    IArtifactContext ForModule(Type moduleType);
}
