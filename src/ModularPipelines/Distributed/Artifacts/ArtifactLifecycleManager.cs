using System.Collections.Concurrent;
using System.IO.Compression;
using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Caching;
using ModularPipelines.FileSystem;
using ModularPipelines.Logging;

namespace ModularPipelines.Distributed.Artifacts;

/// <summary>
/// Manages automatic upload/download of artifacts based on <see cref="ProducesArtifactAttribute"/>
/// and <see cref="ConsumesArtifactAttribute"/> declarations.
/// </summary>
internal class ArtifactLifecycleManager
{
    private readonly IDistributedArtifactStore _store;
    private readonly DistributedOptions _options;
    private readonly ILogger<ArtifactLifecycleManager> _logger;
    private readonly string _workingDirectory;
    private readonly AcceptedArtifactRegistry? _acceptedArtifacts;

    private ILogger Logger =>
        (ILogger?) AmbientModuleOutputContext.Current?.Logger ?? _logger;

    /// <summary>
    /// Tracks completed and in-flight restores by module ID, artifact name, normalized path, and missing-artifact policy.
    /// Multiple modules consuming the same artifact to the same path share a single download.
    /// </summary>
    private readonly ConcurrentDictionary<(ModuleId Module, string Artifact, string Path, bool FailIfMissing), Lazy<Task>> _completedRestores = new();

    public ArtifactLifecycleManager(
        IDistributedArtifactStore store,
        IOptions<DistributedOptions> options,
        ILogger<ArtifactLifecycleManager> logger,
        AcceptedArtifactRegistry? acceptedArtifacts = null)
        : this(store, options, logger, Directory.GetCurrentDirectory(), acceptedArtifacts)
    {
    }

    public ArtifactLifecycleManager(
        IDistributedArtifactStore store,
        IOptions<DistributedOptions> options,
        ILogger<ArtifactLifecycleManager> logger,
        IOptions<ModuleCacheOptions> cacheOptions,
        AcceptedArtifactRegistry? acceptedArtifacts = null)
        : this(store, options, logger, cacheOptions.Value.WorkingDirectory, acceptedArtifacts)
    {
    }

    internal ArtifactLifecycleManager(
        IDistributedArtifactStore store,
        IOptions<DistributedOptions> options,
        ILogger<ArtifactLifecycleManager> logger,
        string workingDirectory,
        AcceptedArtifactRegistry? acceptedArtifacts = null)
    {
        _store = store;
        _options = options.Value;
        _logger = logger;
        _workingDirectory = Path.GetFullPath(workingDirectory);
        _acceptedArtifacts = acceptedArtifacts;
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
                Logger.LogError(ex,
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
        var resolvedPaths = ResolvePathPattern(attribute.PathPattern, cancellationToken);
        if (resolvedPaths.Count == 0)
        {
            Logger.LogWarning(
                "No files matched pattern '{Pattern}' for artifact '{Name}' on module {Module}",
                attribute.PathPattern,
                attribute.Name,
                moduleType.Name);
            return null;
        }

        var descriptor = new ArtifactDescriptor
        {
            Name = attribute.Name,
            ModuleId = ModuleId.FromType(moduleType),
        };
        var reference = await UploadResolvedPathsAsync(
                descriptor,
                attribute.PathPattern,
                resolvedPaths,
                cancellationToken)
            .ConfigureAwait(false);
        if (Logger.IsEnabled(LogLevel.Information))
        {
            Logger.LogInformation(
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
        var isLiteralPath = pathPattern.IndexOfAny(['*', '?']) < 0;
        if (resolvedPaths.Count == 1 && isLiteralPath)
        {
            if (Directory.Exists(resolvedPaths[0]))
            {
                return UploadDirectoryAsync(descriptor, resolvedPaths[0], cancellationToken);
            }

            if (File.Exists(resolvedPaths[0]))
            {
                return UploadFileAsync(
                    descriptor with { ContentType = "application/octet-stream" },
                    resolvedPaths[0],
                    cancellationToken);
            }
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
            await ArtifactContextImpl.CreateDirectoryArchiveAsync(
                directoryPath, tempFile, _options.ArtifactCompressionLevel, cancellationToken)
                .ConfigureAwait(false);
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
            await ArtifactContextImpl.CreatePathArchiveAsync(
                tempFile, GetArchiveBaseDirectory(pathPattern),
                EnumerateArchivePaths(resolvedPaths, cancellationToken),
                _options.ArtifactCompressionLevel, cancellationToken).ConfigureAwait(false);
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

    private static IEnumerable<(string Path, bool IsDirectory)> EnumerateArchivePaths(
        IReadOnlyList<string> resolvedPaths, CancellationToken cancellationToken)
    {
        foreach (var path in resolvedPaths)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArtifactContextImpl.EnsureSourcePathContainsNoLinks(SystemFileSystemProvider.Instance, path);
            var isDirectory = Directory.Exists(path);
            yield return (path, isDirectory);
            if (isDirectory)
            {
                foreach (var entry in ArtifactContextImpl.EnumerateDirectoryPaths(path, cancellationToken))
                {
                    yield return entry;
                }
            }
        }
    }

    private async Task<ArtifactReference> UploadFileAsync(
        ArtifactDescriptor descriptor,
        string filePath,
        CancellationToken cancellationToken)
    {
        ArtifactContextImpl.EnsureSourcePathContainsNoLinks(SystemFileSystemProvider.Instance, filePath);
        var stream = File.OpenRead(filePath);
        await using var streamScope = stream.ConfigureAwait(false);
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
            var producerModuleId = ModuleId.FromType(attr.ProducerModule);
            var restorePath = attr.RestorePath ?? _workingDirectory;
            await DownloadConsumedArtifactsForPathAsync(
                producerModuleId,
                attr.ArtifactName,
                restorePath,
                moduleType,
                failIfMissing,
                attr.ProducerModule,
                cancellationToken).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Throws when a distributed producer did not upload an artifact that a planned consumer requires,
    /// matching the check standalone execution applies after uploading.
    /// </summary>
    internal static void EnsureRequiredArtifactsProduced(
        Type moduleType,
        IReadOnlyCollection<ArtifactReference> uploadedArtifacts,
        IReadOnlyCollection<string> requiredArtifactNames)
    {
        if (requiredArtifactNames.Count == 0)
        {
            return;
        }

        var uploadedNames = uploadedArtifacts
            .Select(static artifact => artifact.Name)
            .ToHashSet(StringComparer.Ordinal);
        var missing = moduleType
            .GetCustomAttributes(typeof(ProducesArtifactAttribute), inherit: true)
            .Cast<ProducesArtifactAttribute>()
            .Where(attribute => requiredArtifactNames.Contains(attribute.Name, StringComparer.Ordinal)
                                && !uploadedNames.Contains(attribute.Name))
            .OrderBy(static attribute => attribute.Name, StringComparer.Ordinal)
            .Select(static attribute =>
                $"Artifact '{attribute.Name}' matched no files for pattern '{attribute.PathPattern}'.")
            .ToArray();
        if (missing.Length == 0)
        {
            return;
        }

        throw new InvalidOperationException(
            $"Module '{moduleType.Name}' did not produce required artifacts:{Environment.NewLine}"
            + string.Join(Environment.NewLine, missing));
    }

    /// <summary>
    /// Downloads a specific artifact to a specific path with deduplication.
    /// If the same artifact has already been restored to the same path (by this or another module),
    /// this call is a no-op. Concurrent calls for the same key share a single in-flight download.
    /// </summary>
    internal Task DownloadConsumedArtifactsForPathAsync(
        ModuleId producerModuleId,
        string artifactName,
        string restorePath,
        Type consumerModuleType,
        CancellationToken cancellationToken) =>
        DownloadConsumedArtifactsForPathAsync(
            producerModuleId,
            artifactName,
            restorePath,
            consumerModuleType,
            failIfMissing: false,
            producerModuleType: null,
            cancellationToken);

    internal async Task DownloadConsumedArtifactsForPathAsync(
        ModuleId producerModuleId,
        string artifactName,
        string restorePath,
        Type consumerModuleType,
        bool failIfMissing,
        Type? producerModuleType,
        CancellationToken cancellationToken)
    {
        var normalizedPath = ResolvePath(restorePath);
        var restoreKey = (producerModuleId, artifactName, normalizedPath, failIfMissing);

        // Use CancellationToken.None for the shared download so one caller's cancellation
        // doesn't abort the download for other modules consuming the same artifact.
        var lazyTask = _completedRestores.GetOrAdd(
            restoreKey,
            _ => new Lazy<Task>(() => RestoreArtifactAsync(
                producerModuleId,
                artifactName,
                normalizedPath,
                consumerModuleType,
                failIfMissing,
                producerModuleType,
                CancellationToken.None)));

        try
        {
            // WaitAsync respects the caller's token without affecting the shared download
            await lazyTask.Value.WaitAsync(cancellationToken).ConfigureAwait(false);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            // Remove failed entry so a retry can attempt it again
            _completedRestores.TryRemove(restoreKey, out _);
            Logger.LogError(ex,
                "Failed to download artifact '{Name}' for module {Module}",
                artifactName, consumerModuleType.Name);
            throw;
        }
    }

    private async Task RestoreArtifactAsync(
        ModuleId producerModuleId,
        string artifactName,
        string restorePath,
        Type consumerModuleType,
        bool failIfMissing,
        Type? producerModuleType,
        CancellationToken cancellationToken)
    {
        var artifact = await ResolveArtifactAsync(
                _store,
                _acceptedArtifacts,
                producerModuleId,
                artifactName,
                cancellationToken)
            .ConfigureAwait(false);

        if (artifact is null)
        {
            var message = $"Artifact '{artifactName}' from module '{producerModuleId}' " +
                          $"was not found for consumer '{consumerModuleType.Name}'.";
            if (failIfMissing)
            {
                if (producerModuleType is not null)
                {
                    throw new MissingConsumedArtifactException(
                        producerModuleType,
                        artifactName,
                        consumerModuleType);
                }

                throw new InvalidOperationException(message);
            }

            Logger.LogWarning(
                "Artifact '{Name}' from module '{Producer}' was not found for consumer {Module}",
                artifactName, producerModuleId, consumerModuleType.Name);
            return;
        }

        var stream = await _store.DownloadAsync(artifact, cancellationToken).ConfigureAwait(false);
        await using var streamScope = stream.ConfigureAwait(false);

        if (artifact.ContentType == "application/zip")
        {
            using var archive = new ZipArchive(stream, ZipArchiveMode.Read);
            await ArtifactContextImpl.ExtractDirectoryArchiveAsync(archive, restorePath, cancellationToken)
                .ConfigureAwait(false);
        }
        else
        {
            var destFile = Path.GetFullPath(Path.Combine(restorePath, artifact.Name));
            var restorePrefix = Path.EndsInDirectorySeparator(restorePath)
                ? restorePath
                : restorePath + Path.DirectorySeparatorChar;
            if (!destFile.StartsWith(restorePrefix, ArtifactContextImpl.GetArchivePathComparison()))
            {
                throw new IOException($"Restoring '{artifact.Name}' would leave the destination directory.");
            }

            await ArtifactContextImpl.RestoreFileAsync(stream, destFile, cancellationToken).ConfigureAwait(false);
        }

        if (Logger.IsEnabled(LogLevel.Information))
        {
            Logger.LogInformation(
                "Restored artifact '{Name}' from module '{Producer}' to '{Path}'",
                artifactName, producerModuleId, restorePath);
        }
    }

    /// <summary>
    /// Resolves the artifact to download: the reference listed by the producer's accepted distributed
    /// result when one is known, otherwise the newest matching upload in the store.
    /// </summary>
    internal static async Task<ArtifactReference?> ResolveArtifactAsync(
        IDistributedArtifactStore store,
        AcceptedArtifactRegistry? acceptedArtifacts,
        ModuleId producerModuleId,
        string artifactName,
        CancellationToken cancellationToken)
    {
        if (acceptedArtifacts?.TryGet(producerModuleId, out var accepted) == true)
        {
            return accepted.FirstOrDefault(artifact => artifact.Name == artifactName);
        }

        var artifacts = await store.ListArtifactsAsync(producerModuleId, cancellationToken).ConfigureAwait(false);
        return artifacts
            .Where(artifact => artifact.Name == artifactName)
            .OrderByDescending(static artifact => artifact.UploadedAt)
            .FirstOrDefault();
    }

    /// <summary>
    /// Resolves a path pattern to concrete paths. Supports simple glob patterns.
    /// Returns a list of matched files/directories.
    /// </summary>
    internal IReadOnlyList<string> ResolvePathPattern(string pathPattern, CancellationToken cancellationToken = default)
    {
        pathPattern = ResolvePath(pathPattern);

        var wildcardIndex = pathPattern.IndexOfAny(['*', '?']);
        if (wildcardIndex < 0)
        {
            ArtifactContextImpl.EnsureSourcePathContainsNoLinks(SystemFileSystemProvider.Instance, pathPattern);
        }

        // If the path exists directly, return it
        if (Directory.Exists(pathPattern) || File.Exists(pathPattern))
        {
            return [pathPattern];
        }

        // Handle simple glob patterns by splitting at the first wildcard
        if (wildcardIndex < 0)
        {
            return [];
        }

        var baseDir = GetGlobBaseDirectory(pathPattern, wildcardIndex);

        ArtifactContextImpl.EnsureSourcePathContainsNoLinks(SystemFileSystemProvider.Instance, baseDir);
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

        return ArtifactContextImpl.EnumerateDirectoryPaths(baseDir, cancellationToken)
            .Select(entry => entry.Path)
            .Where(Matches)
            .ToList();
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
