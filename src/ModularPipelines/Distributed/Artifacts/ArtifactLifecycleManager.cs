using System.Collections.Concurrent;
using System.IO.Compression;
using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Caching;

namespace ModularPipelines.Distributed.Artifacts;

/// <summary>
/// Manages automatic upload/download of artifacts based on <see cref="ProducesArtifactAttribute"/>
/// and <see cref="ConsumesArtifactAttribute"/> declarations.
/// </summary>
internal class ArtifactLifecycleManager
{
    private readonly IDistributedArtifactStore _store;
    private readonly ArtifactOptions _options;
    private readonly ILogger<ArtifactLifecycleManager> _logger;
    private readonly string _workingDirectory;

    /// <summary>
    /// Tracks completed and in-flight restores keyed by "{producerType}:{artifactName}:{normalizedRestorePath}".
    /// Multiple modules consuming the same artifact to the same path share a single download.
    /// </summary>
    private readonly ConcurrentDictionary<string, Lazy<Task>> _completedRestores = new();

    public ArtifactLifecycleManager(
        IDistributedArtifactStore store,
        IOptions<ArtifactOptions> options,
        ILogger<ArtifactLifecycleManager> logger)
        : this(store, options, logger, Directory.GetCurrentDirectory())
    {
    }

    public ArtifactLifecycleManager(
        IDistributedArtifactStore store,
        IOptions<ArtifactOptions> options,
        ILogger<ArtifactLifecycleManager> logger,
        IOptions<ModuleCacheOptions> cacheOptions)
        : this(store, options, logger, cacheOptions.Value.WorkingDirectory)
    {
    }

    internal ArtifactLifecycleManager(
        IDistributedArtifactStore store,
        IOptions<ArtifactOptions> options,
        ILogger<ArtifactLifecycleManager> logger,
        string workingDirectory)
    {
        _store = store;
        _options = options.Value;
        _logger = logger;
        _workingDirectory = Path.GetFullPath(workingDirectory);
    }

    /// <summary>
    /// Scans a module type for <see cref="ProducesArtifactAttribute"/> and uploads matching artifacts.
    /// </summary>
    public Task<IReadOnlyList<ArtifactReference>> UploadProducedArtifactsAsync(
        Type moduleType,
        CancellationToken cancellationToken) =>
        UploadProducedArtifactsAsync(moduleType, artifactNames: null, cancellationToken);

    internal async Task<IReadOnlyList<ArtifactReference>> UploadProducedArtifactsAsync(
        Type moduleType,
        IReadOnlySet<string>? artifactNames,
        CancellationToken cancellationToken)
    {
        var attributes = moduleType.GetCustomAttributes(typeof(ProducesArtifactAttribute), true)
            .Cast<ProducesArtifactAttribute>()
            .Where(attribute => artifactNames is null || artifactNames.Contains(attribute.Name))
            .ToList();

        if (attributes.Count == 0)
        {
            return [];
        }

        var references = new List<ArtifactReference>();
        foreach (var attr in attributes)
        {
            try
            {
                var reference = await UploadProducedArtifactAsync(
                        moduleType,
                        attr,
                        cancellationToken)
                    .ConfigureAwait(false);
                if (reference is not null)
                {
                    references.Add(reference);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Failed to upload artifact '{Name}' for module {Module}",
                    attr.Name, moduleType.Name);
                throw;
            }
        }

        return references;
    }

    private async Task<ArtifactReference?> UploadProducedArtifactAsync(
        Type moduleType,
        ProducesArtifactAttribute attribute,
        CancellationToken cancellationToken)
    {
        var resolvedPaths = ResolvePathPattern(attribute.PathPattern);
        if (resolvedPaths.Count == 0)
        {
            _logger.LogWarning(
                "No files matched pattern '{Pattern}' for artifact '{Name}' on module {Module}",
                attribute.PathPattern,
                attribute.Name,
                moduleType.Name);
            return null;
        }

        var descriptor = new ArtifactDescriptor(
            Name: attribute.Name,
            ModuleTypeName: moduleType.FullName!);
        var reference = await UploadResolvedPathsAsync(
                descriptor,
                attribute.PathPattern,
                resolvedPaths,
                cancellationToken)
            .ConfigureAwait(false);
        if (_logger.IsEnabled(LogLevel.Information))
        {
            _logger.LogInformation(
                "Uploaded artifact '{Name}' ({Size} bytes, {FileCount} files) for module {Module}",
                attribute.Name,
                reference.SizeBytes,
                resolvedPaths.Count,
                moduleType.Name);
        }
        return reference;
    }

    private Task<ArtifactReference> UploadResolvedPathsAsync(
        ArtifactDescriptor descriptor,
        string pathPattern,
        IReadOnlyList<string> resolvedPaths,
        CancellationToken cancellationToken)
    {
        if (resolvedPaths.Count == 1
            && Directory.Exists(resolvedPaths[0])
            && pathPattern.IndexOfAny(['*', '?']) < 0)
        {
            return UploadDirectoryAsync(descriptor, resolvedPaths[0], cancellationToken);
        }

        if (resolvedPaths.Count == 1 && File.Exists(resolvedPaths[0]))
        {
            return UploadFileAsync(
                descriptor with { ContentType = "application/octet-stream" },
                resolvedPaths[0],
                cancellationToken);
        }

        return UploadPathArchiveAsync(descriptor, pathPattern, resolvedPaths, cancellationToken);
    }

    private async Task<ArtifactReference> UploadDirectoryAsync(
        ArtifactDescriptor descriptor,
        string directoryPath,
        CancellationToken cancellationToken)
    {
        var tempFile = CreateTemporaryArchivePath();
        try
        {
            ZipFile.CreateFromDirectory(
                directoryPath,
                tempFile,
                _options.CompressionLevel,
                includeBaseDirectory: false);
            return await UploadFileAsync(
                    descriptor with { ContentType = "application/zip" },
                    tempFile,
                    cancellationToken)
                .ConfigureAwait(false);
        }
        finally
        {
            File.Delete(tempFile);
        }
    }

    private async Task<ArtifactReference> UploadPathArchiveAsync(
        ArtifactDescriptor descriptor,
        string pathPattern,
        IReadOnlyList<string> resolvedPaths,
        CancellationToken cancellationToken)
    {
        var tempFile = CreateTemporaryArchivePath();
        try
        {
            CreatePathArchive(tempFile, GetArchiveBaseDirectory(pathPattern), resolvedPaths);
            return await UploadFileAsync(
                    descriptor with { ContentType = "application/zip" },
                    tempFile,
                    cancellationToken)
                .ConfigureAwait(false);
        }
        finally
        {
            File.Delete(tempFile);
        }
    }

    private void CreatePathArchive(
        string archivePath,
        string archiveBaseDirectory,
        IReadOnlyList<string> resolvedPaths)
    {
        using var archive = ZipFile.Open(archivePath, ZipArchiveMode.Create);
        var archivedEntries = new HashSet<string>(StringComparer.Ordinal);
        foreach (var resolvedPath in resolvedPaths)
        {
            AddPathToArchive(archive, archivedEntries, archiveBaseDirectory, resolvedPath);
        }
    }

    private void AddPathToArchive(
        ZipArchive archive,
        HashSet<string> archivedEntries,
        string archiveBaseDirectory,
        string resolvedPath)
    {
        var isDirectory = Directory.Exists(resolvedPath);
        IEnumerable<string> directoryPaths = isDirectory
            ? [
                resolvedPath,
                .. Directory.EnumerateDirectories(
                    resolvedPath,
                    "*",
                    SearchOption.AllDirectories),
            ]
            : [];
        foreach (var directoryPath in directoryPaths)
        {
            var entryName = GetArchiveEntryName(archiveBaseDirectory, directoryPath).TrimEnd('/') + "/";
            if (archivedEntries.Add(entryName))
            {
                archive.CreateEntry(entryName, _options.CompressionLevel);
            }
        }

        var filePaths = isDirectory
            ? Directory.EnumerateFiles(resolvedPath, "*", SearchOption.AllDirectories)
            : [resolvedPath];
        foreach (var filePath in filePaths)
        {
            var entryName = GetArchiveEntryName(archiveBaseDirectory, filePath);
            if (archivedEntries.Add(entryName))
            {
                archive.CreateEntryFromFile(filePath, entryName, _options.CompressionLevel);
            }
        }
    }

    private static string GetArchiveEntryName(string archiveBaseDirectory, string path) =>
        Path.GetRelativePath(archiveBaseDirectory, path)
            .Replace(Path.DirectorySeparatorChar, '/');

    private async Task<ArtifactReference> UploadFileAsync(
        ArtifactDescriptor descriptor,
        string filePath,
        CancellationToken cancellationToken)
    {
        await using var stream = File.OpenRead(filePath);
        return await _store.UploadAsync(descriptor, stream, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Scans a module type for <see cref="ConsumesArtifactAttribute"/> and downloads required artifacts.
    /// Deduplicates downloads — if another module already restored the same artifact to the same path,
    /// this call awaits that existing operation instead of downloading again.
    /// </summary>
    public Task DownloadConsumedArtifactsAsync(Type moduleType, CancellationToken cancellationToken) =>
        DownloadConsumedArtifactsAsync(moduleType, failIfMissing: false, cancellationToken);

    internal async Task DownloadConsumedArtifactsAsync(
        Type moduleType,
        bool failIfMissing,
        CancellationToken cancellationToken)
    {
        var attributes = moduleType.GetCustomAttributes(typeof(ConsumesArtifactAttribute), true)
            .Cast<ConsumesArtifactAttribute>()
            .ToList();

        if (attributes.Count == 0)
        {
            return;
        }

        foreach (var attr in attributes)
        {
            var producerTypeName = attr.ProducerModule.FullName!;
            var restorePath = attr.RestorePath ?? _workingDirectory;
            await DownloadConsumedArtifactsForPathAsync(
                producerTypeName,
                attr.ArtifactName,
                restorePath,
                moduleType,
                failIfMissing,
                cancellationToken);
        }
    }

    /// <summary>
    /// Downloads a specific artifact to a specific path with deduplication.
    /// If the same artifact has already been restored to the same path (by this or another module),
    /// this call is a no-op. Concurrent calls for the same key share a single in-flight download.
    /// </summary>
    internal Task DownloadConsumedArtifactsForPathAsync(
        string producerTypeName,
        string artifactName,
        string restorePath,
        Type consumerModuleType,
        CancellationToken cancellationToken) =>
        DownloadConsumedArtifactsForPathAsync(
            producerTypeName,
            artifactName,
            restorePath,
            consumerModuleType,
            failIfMissing: false,
            cancellationToken);

    internal async Task DownloadConsumedArtifactsForPathAsync(
        string producerTypeName,
        string artifactName,
        string restorePath,
        Type consumerModuleType,
        bool failIfMissing,
        CancellationToken cancellationToken)
    {
        var normalizedPath = ResolvePath(restorePath);
        var restoreKey = $"{producerTypeName}:{artifactName}:{normalizedPath}:{failIfMissing}";

        // Use CancellationToken.None for the shared download so one caller's cancellation
        // doesn't abort the download for other modules consuming the same artifact.
        var lazyTask = _completedRestores.GetOrAdd(
            restoreKey,
            _ => new Lazy<Task>(() => RestoreArtifactAsync(
                producerTypeName,
                artifactName,
                normalizedPath,
                consumerModuleType,
                failIfMissing,
                CancellationToken.None)));

        try
        {
            // WaitAsync respects the caller's token without affecting the shared download
            await lazyTask.Value.WaitAsync(cancellationToken);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            // Remove failed entry so a retry can attempt it again
            _completedRestores.TryRemove(restoreKey, out _);
            _logger.LogError(ex,
                "Failed to download artifact '{Name}' for module {Module}",
                artifactName, consumerModuleType.Name);
            throw;
        }
    }

    private async Task RestoreArtifactAsync(
        string producerTypeName,
        string artifactName,
        string restorePath,
        Type consumerModuleType,
        bool failIfMissing,
        CancellationToken cancellationToken)
    {
        var artifacts = await _store.ListArtifactsAsync(producerTypeName, cancellationToken);
        var artifact = artifacts.FirstOrDefault(a => a.Name == artifactName);

        if (artifact is null)
        {
            var message = $"Artifact '{artifactName}' from module '{producerTypeName}' " +
                          $"was not found for consumer '{consumerModuleType.Name}'.";
            if (failIfMissing)
            {
                throw new InvalidOperationException(message);
            }

            _logger.LogWarning(
                "Artifact '{Name}' from module '{Producer}' was not found for consumer {Module}",
                artifactName, producerTypeName, consumerModuleType.Name);
            return;
        }

        await using var stream = await _store.DownloadAsync(artifact, cancellationToken);

        if (artifact.ContentType == "application/zip")
        {
            Directory.CreateDirectory(restorePath);
            using var archive = new ZipArchive(stream, ZipArchiveMode.Read);
            archive.ExtractToDirectory(restorePath, overwriteFiles: true);
        }
        else
        {
            var destFile = Path.Combine(restorePath, artifact.Name);
            Directory.CreateDirectory(Path.GetDirectoryName(destFile)!);
            await using var fileStream = File.Create(destFile);
            await stream.CopyToAsync(fileStream, cancellationToken);
        }

        if (_logger.IsEnabled(LogLevel.Information))
        {
            _logger.LogInformation(
                "Restored artifact '{Name}' from module '{Producer}' to '{Path}'",
                artifactName, producerTypeName, restorePath);
        }
    }

    /// <summary>
    /// Resolves a path pattern to concrete paths. Supports simple glob patterns.
    /// Returns a list of matched files/directories.
    /// </summary>
    internal IReadOnlyList<string> ResolvePathPattern(string pathPattern)
    {
        pathPattern = ResolvePath(pathPattern);

        // If the path exists directly, return it
        if (Directory.Exists(pathPattern) || File.Exists(pathPattern))
        {
            return [pathPattern];
        }

        // Handle simple glob patterns by splitting at the first wildcard
        var wildcardIndex = pathPattern.IndexOfAny(['*', '?']);
        if (wildcardIndex < 0)
        {
            return [];
        }

        var baseDir = GetGlobBaseDirectory(pathPattern, wildcardIndex);

        if (!Directory.Exists(baseDir))
        {
            return [];
        }

        var relativePattern = Path.GetRelativePath(baseDir, pathPattern)
            .Replace(Path.DirectorySeparatorChar, '/');
        var matcher = new Matcher(
                OperatingSystem.IsWindows()
                    ? StringComparison.OrdinalIgnoreCase
                    : StringComparison.Ordinal)
            .AddInclude(relativePattern);

        bool Matches(string path) => matcher.Match(
            Path.GetRelativePath(baseDir, path)
                .Replace(Path.DirectorySeparatorChar, '/')).HasMatches;

        return
        [
            .. Directory.GetFiles(baseDir, "*", SearchOption.AllDirectories).Where(Matches),
            .. Directory.GetDirectories(baseDir, "*", SearchOption.AllDirectories).Where(Matches),
        ];
    }

    private string GetArchiveBaseDirectory(string pathPattern)
    {
        var resolvedPattern = ResolvePath(pathPattern);
        var wildcardIndex = resolvedPattern.IndexOfAny(['*', '?']);
        return wildcardIndex < 0
            ? Path.GetDirectoryName(resolvedPattern) ?? _workingDirectory
            : GetGlobBaseDirectory(resolvedPattern, wildcardIndex);
    }

    private static string GetGlobBaseDirectory(string pathPattern, int wildcardIndex)
    {
        var separatorIndex = pathPattern.LastIndexOfAny(
            [Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar],
            wildcardIndex);
        return separatorIndex < 0
            ? Directory.GetCurrentDirectory()
            : Path.GetFullPath(pathPattern[..(separatorIndex + 1)]);
    }

    private string ResolvePath(string path) =>
        Path.GetFullPath(Path.IsPathRooted(path) ? path : Path.Combine(_workingDirectory, path));

    private static string CreateTemporaryArchivePath() =>
        Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.zip");
}
