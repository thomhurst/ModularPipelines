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

        var stream = OpenSourceFile(provider, filePath);
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
            var stream = OpenSourceFile(SystemFileSystemProvider.Instance, temporaryArchivePath);
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
        IFileSystemProvider? provider = null,
        string? trustedRoot = null)
    {
        provider ??= SystemFileSystemProvider.Instance;
        cancellationToken.ThrowIfCancellationRequested();
        var sourceDirectory = provider is SystemFileSystemProvider ? Path.GetFullPath(directoryPath) : directoryPath;
        var fullArchivePath = Path.GetFullPath(archivePath);
        EnsureSourcePathContainsNoLinks(provider, sourceDirectory, trustedRoot);
        EnsureSourcePathContainsNoLinks(SystemFileSystemProvider.Instance, fullArchivePath);
        using (var archiveDirectory = BoundArtifactDirectory.Open(Path.GetDirectoryName(fullArchivePath)!, create: false))
        {
            archiveDirectory.Delete(Path.GetFileName(fullArchivePath));
        }

        await CreatePathArchiveAsync(
            fullArchivePath, sourceDirectory,
            EnumerateDirectoryPaths(sourceDirectory, cancellationToken, provider, trustedRoot),
            compressionLevel, cancellationToken, provider, trustedRoot).ConfigureAwait(false);
    }

    internal static IEnumerable<(string Path, bool IsDirectory)> EnumerateDirectoryPaths(
        string directoryPath,
        CancellationToken cancellationToken,
        IFileSystemProvider? provider = null,
        string? trustedRoot = null,
        Func<string, bool>? includeFile = null)
    {
        provider ??= SystemFileSystemProvider.Instance;
        cancellationToken.ThrowIfCancellationRequested();
        EnsureSourcePathContainsNoLinks(provider, directoryPath, trustedRoot);
        var pending = new Stack<string>();
        pending.Push(directoryPath);
        while (pending.TryPop(out var current))
        {
            cancellationToken.ThrowIfCancellationRequested();
            EnsureSourcePathContainsNoLinks(provider, current, trustedRoot);
            // Check each entry before descending so links, including cycles, are never followed.
            foreach (var directory in provider.EnumerateDirectories(current, "*", SearchOption.TopDirectoryOnly))
            {
                cancellationToken.ThrowIfCancellationRequested();
                EnsurePathIsNotLink(provider, directory);
                yield return (directory, true);
                pending.Push(directory);
            }

            foreach (var file in provider.EnumerateFiles(current, "*", SearchOption.TopDirectoryOnly))
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (includeFile is not null && !includeFile(file))
                {
                    continue;
                }

                EnsurePathIsNotLink(provider, file);
                yield return (file, false);
            }
        }
    }

    internal static async Task CreatePathArchiveAsync(
        string archivePath,
        string sourceDirectory,
        IEnumerable<(string Path, bool IsDirectory)> paths,
        CompressionLevel compressionLevel,
        CancellationToken cancellationToken,
        IFileSystemProvider? provider = null,
        string? trustedRoot = null)
    {
        provider ??= SystemFileSystemProvider.Instance;
        // Complete enumeration before opening the archive or reading any source file.
        var entries = paths.DistinctBy(entry => entry.Path,
            GetArchivePathComparison(provider) == StringComparison.OrdinalIgnoreCase
                ? StringComparer.OrdinalIgnoreCase
                : StringComparer.Ordinal).ToList();
        cancellationToken.ThrowIfCancellationRequested();
        var archivedEntries = new HashSet<string>(StringComparer.Ordinal);
        archivePath = Path.GetFullPath(archivePath);
        using var archiveDirectory = BoundArtifactDirectory.Open(Path.GetDirectoryName(archivePath)!, create: false);
        var archiveStream = archiveDirectory.CreateNew(Path.GetFileName(archivePath));
        await using var archiveStreamLifetime = archiveStream.ConfigureAwait(false);
        using var archive = new ZipArchive(archiveStream, ZipArchiveMode.Create, leaveOpen: true);
        foreach (var (path, isDirectory) in entries)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var entryName = GetArchiveEntryName(provider, sourceDirectory, path);
            if (isDirectory)
            {
                EnsureSourcePathContainsNoLinks(provider, path, trustedRoot);
                entryName = entryName.TrimEnd('/') + "/";
                if (archivedEntries.Add(entryName))
                {
                    archive.CreateEntry(entryName, compressionLevel);
                }

                continue;
            }

            if (!archivedEntries.Add(entryName))
            {
                continue;
            }

            var entry = archive.CreateEntry(entryName, compressionLevel);
            var sourceStream = OpenSourceFile(provider, path, trustedRoot);
            await using var sourceStreamLifetime = sourceStream.ConfigureAwait(false);
            if (sourceStream is FileStream fileStream && provider is SystemFileSystemProvider)
            {
                entry.LastWriteTime = File.GetLastWriteTimeUtc(fileStream.SafeFileHandle).ToLocalTime();
                if (!OperatingSystem.IsWindows())
                {
                    // Keep the regular-file type, including for a mode of 000.
                    entry.ExternalAttributes = (0x8000 | (int) File.GetUnixFileMode(fileStream.SafeFileHandle)) << 16;
                }
            }
            else
            {
                entry.LastWriteTime = provider.GetLastWriteTimeUtc(path).ToLocalTime();
            }
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

        await RestoreFileAsync(stream, destinationPath, cancellationToken, provider).ConfigureAwait(false);
        return destinationPath;
    }

    internal static async Task RestoreFileAsync(
        Stream stream,
        string destinationPath,
        CancellationToken cancellationToken,
        IFileSystemProvider? provider = null,
        string? trustedRoot = null)
    {
        provider ??= SystemFileSystemProvider.Instance;
        cancellationToken.ThrowIfCancellationRequested();
        if (provider is SystemFileSystemProvider)
        {
            await RestoreSystemFileAsync(stream, destinationPath, trustedRoot, null, null, cancellationToken).ConfigureAwait(false);
            return;
        }

        var destinationDirectory = GetProviderParentPath(provider, destinationPath);
        EnsurePathContainsNoLinks(provider, destinationPath, trustedRoot);
        if (!string.IsNullOrEmpty(destinationDirectory))
        {
            CreateDirectoryWithoutLinks(provider, destinationDirectory, trustedRoot);
        }

        var temporaryName = $".modularpipelines-download-{Guid.NewGuid():N}.tmp";
        var temporaryPath = string.IsNullOrEmpty(destinationDirectory)
            ? temporaryName
            : AppendProviderPath(provider, destinationDirectory, temporaryName);
        var destinationStream = provider.Open(temporaryPath, FileMode.CreateNew, FileAccess.Write);
        try
        {
            await using (destinationStream.ConfigureAwait(false))
            {
                await stream.CopyToAsync(destinationStream, cancellationToken).ConfigureAwait(false);
            }

            cancellationToken.ThrowIfCancellationRequested();
            EnsurePathContainsNoLinks(provider, destinationPath, trustedRoot);
            provider.MoveFile(temporaryPath, destinationPath, overwrite: true);
        }
        finally
        {
            DeleteTemporaryFile(provider, temporaryPath);
        }
    }

    private static async Task RestoreSystemFileAsync(
        Stream stream, string destinationPath, string? trustedRoot,
        DateTime? lastWriteTime, UnixFileMode? mode, CancellationToken cancellationToken)
    {
        destinationPath = Path.GetFullPath(destinationPath);
        EnsureSourcePathContainsNoLinks(SystemFileSystemProvider.Instance, destinationPath, trustedRoot);
        using var directory = BoundArtifactDirectory.Open(Path.GetDirectoryName(destinationPath)!, create: true, trustedRoot);
        var temporaryName = $".modularpipelines-download-{Guid.NewGuid():N}.tmp";
        var output = directory.CreateNew(temporaryName, mode);
        try
        {
            await using (output.ConfigureAwait(false))
            {
                await stream.CopyToAsync(output, cancellationToken).ConfigureAwait(false);
                if (lastWriteTime is { } timestamp)
                {
                    await output.FlushAsync(cancellationToken).ConfigureAwait(false);
                    File.SetLastWriteTimeUtc(output.SafeFileHandle, timestamp);
                }
            }

            cancellationToken.ThrowIfCancellationRequested();
            directory.Move(temporaryName, Path.GetFileName(destinationPath));
        }
        finally
        {
            try
            {
                directory.Delete(temporaryName);
            }
            catch (Exception exception) when (exception is IOException or UnauthorizedAccessException)
            {
                // Best-effort cleanup must not replace the original outcome.
            }
        }
    }

    internal static Stream OpenSourceFile(IFileSystemProvider provider, string path, string? trustedRoot = null)
    {
        EnsureSourcePathContainsNoLinks(provider, path, trustedRoot);
        if (provider is not SystemFileSystemProvider)
        {
            // Providers expose pathname operations only; their concurrency guarantees apply.
            return provider.OpenRead(path);
        }

        path = Path.GetFullPath(path);
        using var directory = BoundArtifactDirectory.Open(Path.GetDirectoryName(path)!, create: false, trustedRoot);
        return directory.OpenRead(Path.GetFileName(path));
    }

    internal static async Task ExtractDirectoryArchiveAsync(
        ZipArchive archive,
        string destinationPath,
        CancellationToken cancellationToken,
        IFileSystemProvider? provider = null,
        string? trustedRoot = null)
    {
        provider ??= SystemFileSystemProvider.Instance;
        cancellationToken.ThrowIfCancellationRequested();
        var destinationDirectory = provider is SystemFileSystemProvider ? Path.GetFullPath(destinationPath) : destinationPath;
        var destinationPrefix = destinationDirectory.EndsWith(provider.DirectorySeparatorChar)
            ? destinationDirectory
            : destinationDirectory + provider.DirectorySeparatorChar;
        var pathComparison = GetArchivePathComparison(provider);
        CreateDirectoryWithoutLinks(provider, destinationDirectory, trustedRoot);

        foreach (var entry in archive.Entries)
        {
            cancellationToken.ThrowIfCancellationRequested();
            // ZIP uses '/', but legacy archives may also use the provider's separator.
            // Custom-provider segments must never be interpreted using the host's path rules.
            var entryName = entry.FullName.Replace(provider.DirectorySeparatorChar, '/');
            var isDirectory = entryName.EndsWith('/');
            var entryPath = provider is SystemFileSystemProvider
                ? Path.GetFullPath(Path.Combine(destinationDirectory, entryName))
                : GetProviderArchivePath(provider, destinationDirectory, entryName);
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
                CreateDirectoryWithoutLinks(provider, entryPath, trustedRoot);
                continue;
            }

            await ExtractArchiveFileAsync(
                entry, provider, destinationDirectory, destinationPrefix, entryPath, pathComparison, trustedRoot, cancellationToken)
                .ConfigureAwait(false);
        }
    }

    private static string GetProviderArchivePath(IFileSystemProvider provider, string root, string entryName)
    {
        if (entryName.StartsWith('/') ||
            (provider.DirectorySeparatorChar == '\\' && entryName.Length >= 2 && entryName[1] == ':'))
        {
            throw new IOException($"Archive entry '{entryName}' must be relative to the destination directory.");
        }

        var segments = new List<string>();
        foreach (var segment in entryName.Split('/', StringSplitOptions.RemoveEmptyEntries))
        {
            if (segment == ".")
            {
                continue;
            }

            if (segment == "..")
            {
                if (segments.Count == 0)
                {
                    throw new IOException($"Extracting '{entryName}' would leave the destination directory.");
                }

                segments.RemoveAt(segments.Count - 1);
            }
            else
            {
                segments.Add(segment);
            }
        }

        return segments.Count == 0 ? root : AppendProviderPath(provider, root, string.Join(provider.DirectorySeparatorChar, segments));
    }

    private static string? GetProviderParentPath(IFileSystemProvider provider, string path)
    {
        var separatorIndex = path.LastIndexOf(provider.DirectorySeparatorChar);
        // Typed paths retain host-compatible absolute roots. Preserve drive/UNC roots,
        // and use host parsing only when no provider-relative segments are present.
        if (provider is SystemFileSystemProvider || separatorIndex < (Path.GetPathRoot(path)?.Length ?? 0))
        {
            return Path.GetDirectoryName(path);
        }

        return path[..separatorIndex];
    }

    private static string AppendProviderPath(IFileSystemProvider provider, string directory, string relativePath) =>
        directory.EndsWith(provider.DirectorySeparatorChar)
            ? directory + relativePath
            : directory + provider.DirectorySeparatorChar + relativePath;

    private static async Task ExtractArchiveFileAsync(
        ZipArchiveEntry entry,
        IFileSystemProvider provider,
        string destinationDirectory,
        string destinationPrefix,
        string entryPath,
        StringComparison pathComparison,
        string? trustedRoot,
        CancellationToken cancellationToken)
    {
        if (provider is SystemFileSystemProvider)
        {
            var attributes = (entry.ExternalAttributes >> 16) & 0xFFFF;
            var permissionBits = attributes & 0x1FF;
            UnixFileMode? mode = !OperatingSystem.IsWindows() && attributes != 0 ? (UnixFileMode) permissionBits : null;
            var input = entry.Open();
            await using (input.ConfigureAwait(false))
            {
                await RestoreSystemFileAsync(input, entryPath, trustedRoot,
                    entry.LastWriteTime.UtcDateTime, mode, cancellationToken).ConfigureAwait(false);
            }
            return;
        }

        var entryDirectory = GetProviderParentPath(provider, entryPath);
        // A direct child's parent omits the final separator. Keep the supplied custom root
        // intact so link checks and temporary files stay within that root.
        if (provider is not SystemFileSystemProvider && entryDirectory?.Length < destinationDirectory.Length)
        {
            entryDirectory = destinationDirectory;
        }

        if (!string.IsNullOrEmpty(entryDirectory))
        {
            CreateDirectoryWithoutLinks(provider, entryDirectory, trustedRoot);
        }

        EnsurePathContainsNoLinks(provider, entryPath, trustedRoot);

        var temporaryPath = AppendProviderPath(provider, entryDirectory!, $".modularpipelines-extract-{Guid.NewGuid():N}.tmp");
        if (!temporaryPath.StartsWith(destinationPrefix, pathComparison))
        {
            throw new IOException("The archive temporary file would leave the destination directory.");
        }

        var destinationStream = provider.Open(temporaryPath, FileMode.CreateNew, FileAccess.Write);
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
            EnsurePathContainsNoLinks(provider, entryPath, trustedRoot);
            provider.MoveFile(temporaryPath, entryPath, overwrite: true);
        }
        finally
        {
            DeleteTemporaryFile(provider, temporaryPath);
        }
    }

    private static void DeleteTemporaryFile(IFileSystemProvider provider, string path)
    {
        try
        {
            provider.DeleteFile(path);
        }
        catch (Exception exception) when (exception is IOException or UnauthorizedAccessException)
        {
            // Best-effort cleanup must not replace the download or extraction outcome.
        }
    }

    private static void CreateDirectoryWithoutLinks(IFileSystemProvider provider, string path, string? trustedRoot)
    {
        EnsurePathContainsNoLinks(provider, path, trustedRoot);
        if (provider is SystemFileSystemProvider)
        {
            using var directory = BoundArtifactDirectory.Open(path, create: true, trustedRoot);
        }
        else
        {
            provider.CreateDirectory(path);
            EnsurePathContainsNoLinks(provider, path, trustedRoot);
        }
    }

    internal static void EnsureSourcePathContainsNoLinks(IFileSystemProvider provider, string path, string? trustedRoot = null)
        => EnsurePathContainsNoLinks(provider, path, trustedRoot);

    private static void EnsurePathContainsNoLinks(IFileSystemProvider provider, string path, string? trustedRoot)
    {
        if (provider is not SystemFileSystemProvider)
        {
            EnsureProviderPathContainsNoLinks(provider, path);
            return;
        }

        path = Path.GetFullPath(path);
        var validationRoot = GetTrustedValidationRoot(path, trustedRoot);
        var destinationDirectory = validationRoot ?? Path.GetPathRoot(path)!;

        var currentPath = destinationDirectory;
        if (validationRoot is null && !EnsureExistingPathIsNotLink(provider, currentPath))
        {
            return;
        }

        var relativePath = Path.GetRelativePath(destinationDirectory, path);
        if (relativePath is "" or ".")
        {
            return;
        }

        foreach (var segment in relativePath.Split(provider.DirectorySeparatorChar, StringSplitOptions.RemoveEmptyEntries))
        {
            currentPath = AppendProviderPath(provider, currentPath, segment);
            if (!EnsureExistingPathIsNotLink(provider, currentPath))
            {
                return;
            }
        }
    }

    private static void EnsureProviderPathContainsNoLinks(IFileSystemProvider provider, string path)
    {
        var ancestors = new Stack<string>();
        for (var currentPath = path; !string.IsNullOrEmpty(currentPath); currentPath = GetProviderParentPath(provider, currentPath))
        {
            ancestors.Push(currentPath);
        }

        // Custom providers have no trusted host root. Check parents first so a missing
        // destination cannot hide an existing link higher in the provider's tree.
        while (ancestors.TryPop(out var currentPath))
        {
            if (!EnsureExistingPathIsNotLink(provider, currentPath))
            {
                return;
            }
        }
    }

    private static bool EnsureExistingPathIsNotLink(IFileSystemProvider provider, string path)
    {
        try
        {
            EnsurePathIsNotLink(provider, path);
            return true;
        }
        catch (Exception exception) when (exception is FileNotFoundException or DirectoryNotFoundException)
        {
            return false;
        }
    }

    internal static string? GetTrustedValidationRoot(string path, string? configuredRoot)
    {
        string? trustedRoot = null;
        // The host trusts its configured workspace, process directory and temporary directory.
        // Ancestors may be system aliases or junctions; all segments below the root are checked.
        foreach (var root in new[] { configuredRoot, Directory.GetCurrentDirectory(), Path.GetTempPath() })
        {
            if (root is null)
            {
                continue;
            }

            var fullRoot = Path.TrimEndingDirectorySeparator(Path.GetFullPath(root));
            var prefix = Path.EndsInDirectorySeparator(fullRoot) ? fullRoot : fullRoot + Path.DirectorySeparatorChar;
            if ((trustedRoot is null || fullRoot.Length > trustedRoot.Length)
                && (path.Equals(fullRoot, GetArchivePathComparison())
                    || path.StartsWith(prefix, GetArchivePathComparison())))
            {
                trustedRoot = fullRoot;
            }
        }

        return trustedRoot;
    }

    private static void EnsurePathIsNotLink(IFileSystemProvider provider, string path)
    {
        // File.GetAttributes throws for missing paths, unlike FileSystemInfo.Attributes.
        var attributes = provider is SystemFileSystemProvider ? File.GetAttributes(path) : provider.GetAttributes(path);
        if ((attributes & FileAttributes.ReparsePoint) != 0)
        {
            throw new IOException($"Artifact operations through linked path '{path}' are not allowed.");
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
