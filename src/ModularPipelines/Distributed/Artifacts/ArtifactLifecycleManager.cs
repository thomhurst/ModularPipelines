using System.Collections.Concurrent;
using System.IO.Compression;
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
                var resolvedPaths = ResolvePathPattern(attr.PathPattern);
                if (resolvedPaths.Count == 0)
                {
                    _logger.LogWarning(
                        "No files matched pattern '{Pattern}' for artifact '{Name}' on module {Module}",
                        attr.PathPattern, attr.Name, moduleType.Name);
                    continue;
                }

                var descriptor = new ArtifactDescriptor(
                    Name: attr.Name,
                    ModuleTypeName: moduleType.FullName!);

                ArtifactReference reference;
                if (resolvedPaths.Count == 1 && Directory.Exists(resolvedPaths[0]))
                {
                    // Single directory — zip to temp file to avoid OOM on large directories
                    descriptor = descriptor with { ContentType = "application/zip" };
                    var tempFile = CreateTemporaryArchivePath();
                    try
                    {
                        ZipFile.CreateFromDirectory(resolvedPaths[0], tempFile, _options.CompressionLevel, includeBaseDirectory: false);
                        await using var stream = File.OpenRead(tempFile);
                        reference = await _store.UploadAsync(descriptor, stream, cancellationToken);
                    }
                    finally
                    {
                        File.Delete(tempFile);
                    }
                }
                else if (resolvedPaths.Count == 1 && File.Exists(resolvedPaths[0]))
                {
                    // Single file — upload directly
                    descriptor = descriptor with { ContentType = "application/octet-stream" };
                    await using var stream = File.OpenRead(resolvedPaths[0]);
                    reference = await _store.UploadAsync(descriptor, stream, cancellationToken);
                }
                else
                {
                    // Multiple files — zip to temp file to avoid OOM
                    descriptor = descriptor with { ContentType = "application/zip" };
                    var archiveBaseDirectory = GetArchiveBaseDirectory(attr.PathPattern);
                    var tempFile = CreateTemporaryArchivePath();
                    try
                    {
                        using (var archive = ZipFile.Open(tempFile, ZipArchiveMode.Create))
                        {
                            foreach (var filePath in resolvedPaths)
                            {
                                var entryName = Path.GetRelativePath(archiveBaseDirectory, filePath)
                                    .Replace(Path.DirectorySeparatorChar, '/');
                                archive.CreateEntryFromFile(filePath, entryName, _options.CompressionLevel);
                            }
                        }

                        await using var stream = File.OpenRead(tempFile);
                        reference = await _store.UploadAsync(descriptor, stream, cancellationToken);
                    }
                    finally
                    {
                        File.Delete(tempFile);
                    }
                }

                references.Add(reference);
                _logger.LogInformation(
                    "Uploaded artifact '{Name}' ({Size} bytes, {FileCount} files) for module {Module}",
                    attr.Name, reference.SizeBytes, resolvedPaths.Count, moduleType.Name);
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

        _logger.LogInformation(
            "Restored artifact '{Name}' from module '{Producer}' to '{Path}'",
            artifactName, producerTypeName, restorePath);
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

        // Convert glob to search pattern
        var searchPattern = Path.GetFileName(pathPattern);
        if (string.IsNullOrEmpty(searchPattern))
        {
            searchPattern = "*";
        }

        // Try to find matching files
        var matches = Directory.GetFiles(baseDir, searchPattern, SearchOption.AllDirectories);
        if (matches.Length > 0)
        {
            return matches;
        }

        // Try directories
        var dirMatches = Directory.GetDirectories(baseDir, searchPattern, SearchOption.AllDirectories);
        return dirMatches;
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
