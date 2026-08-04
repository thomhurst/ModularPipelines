using System.Buffers;
using System.Buffers.Binary;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Configuration;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Enums;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Caching;

internal sealed class ModuleCacheResultRepository : IModuleCacheResultRepository
{
    private const string ResultEntryName = "result.json";
    private const string ArtifactPrefix = "artifacts/";
    private const int UnixFileTypeMask = 0xF000;
    private const int UnixFileTypeRegular = 0x8000;
    private const int UnixFileTypeDirectory = 0x4000;
    private const int UnixFileTypeSymbolicLink = 0xA000;
    private const int UnixPermissionMask = 0x0FFF;
    private readonly IModuleCacheStore _store;
    private readonly ModuleCacheOptions _options;
    private readonly ModuleCacheFileHasher _fileHasher;
    private readonly ILogger<ModuleCacheResultRepository> _logger;
    private readonly ModuleLookup _moduleLookup;
    private readonly IModuleDependencyRegistry _dependencyRegistry;
    private readonly IModuleMetadataRegistry _metadataRegistry;
    private readonly ConcurrentDictionary<IModule, string> _fingerprints =
        new(ReferenceEqualityComparer.Instance);

    public ModuleCacheResultRepository(
        IModuleCacheStore store,
        IOptions<ModuleCacheOptions> options,
        ModuleCacheFileHasher fileHasher,
        ModuleLookup moduleLookup,
        IModuleDependencyRegistry dependencyRegistry,
        IModuleMetadataRegistry metadataRegistry,
        ILogger<ModuleCacheResultRepository> logger)
    {
        _store = store;
        _options = options.Value;
        _fileHasher = fileHasher;
        _moduleLookup = moduleLookup;
        _dependencyRegistry = dependencyRegistry;
        _metadataRegistry = metadataRegistry;
        _logger = logger;

        if (_options.MaximumArtifactEntries <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(options),
                "ModuleCacheOptions.MaximumArtifactEntries must be positive.");
        }

        if (_options.MaximumArtifactBytes <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(options),
                "ModuleCacheOptions.MaximumArtifactBytes must be positive.");
        }

        if (_options.MaximumCacheEntryBytes <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(options),
                "ModuleCacheOptions.MaximumCacheEntryBytes must be positive.");
        }

        if (_options.MaximumResultBytes <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(options),
                "ModuleCacheOptions.MaximumResultBytes must be positive.");
        }
    }

    public void DiscardFingerprint(IModule module) =>
        _fingerprints.TryRemove(module, out _);

    [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "Module result cache requires runtime result type metadata.")]
    [UnconditionalSuppressMessage("AOT", "IL3050", Justification = "Module result cache requires runtime result type metadata.")]
    public async Task SaveResultAsync<T>(
        Module<T> module,
        ModuleResult<T> moduleResult,
        IPipelineContext pipelineContext,
        CancellationToken cancellationToken)
    {
        try
        {
            if (!((IModule) module).Configuration.CacheEnabled || moduleResult.ModuleStatus != Status.Successful)
            {
                return;
            }

            if (!_fingerprints.TryGetValue(module, out var fingerprint))
            {
                _logger.LogDebug(
                    "Skipping module cache save for {Module} because no pre-execution fingerprint was captured",
                    module.GetType().Name);
                return;
            }

            var serializedResult = Path.GetTempFileName();
            try
            {
                await using (var resultStream = new FileStream(
                                 serializedResult,
                                 FileMode.Create,
                                 FileAccess.ReadWrite,
                                 FileShare.None,
                                 64 * 1024,
                                 FileOptions.Asynchronous))
                {
                    await JsonSerializer.SerializeAsync<ModuleResult<T>>(
                            resultStream,
                            moduleResult,
                            cancellationToken: cancellationToken)
                        .ConfigureAwait(false);

                    if (resultStream.Length > _options.MaximumResultBytes)
                    {
                        _logger.LogDebug(
                            "Skipping module cache save for {Module} because its serialized result exceeded the configured limit of {MaximumResultBytes} bytes",
                            module.GetType().Name,
                            _options.MaximumResultBytes);
                        return;
                    }

                    resultStream.Position = 0;
                    var temporary = Path.GetTempFileName();
                    try
                    {
                        var saved = await WriteCacheEntryAsync(
                                temporary,
                                resultStream,
                                module.GetType(),
                                fingerprint,
                                cancellationToken)
                            .ConfigureAwait(false);
                        if (!saved)
                        {
                            return;
                        }
                    }
                    finally
                    {
                        File.Delete(temporary);
                    }
                }

                _logger.LogDebug(
                    "Saved module cache entry {Fingerprint} for {Module}",
                    fingerprint,
                    module.GetType().Name);
            }
            finally
            {
                File.Delete(serializedResult);
            }
        }
        finally
        {
            DiscardFingerprint(module);
        }
    }

    [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "Module result cache requires runtime result type metadata.")]
    [UnconditionalSuppressMessage("AOT", "IL3050", Justification = "Module result cache requires runtime result type metadata.")]
    public async Task<ModuleResult<T>?> GetResultAsync<T>(
        Module<T> module,
        IPipelineContext pipelineContext,
        CancellationToken cancellationToken)
    {
        if (!((IModule) module).Configuration.CacheEnabled)
        {
            return null;
        }

        var computedFingerprint = await ComputeFingerprintAsync(module, pipelineContext, cancellationToken)
            .ConfigureAwait(false);
        var fingerprint = computedFingerprint.Value;
        _fingerprints[module] = fingerprint;
        await using var cachedStream = await _store.OpenReadAsync(fingerprint, cancellationToken)
            .ConfigureAwait(false);
        if (cachedStream is null)
        {
            _logger.LogDebug(
                "Module cache miss {Fingerprint} for {Module}. Fingerprint components: {FingerprintComponents}",
                fingerprint,
                module.GetType().Name,
                computedFingerprint.Diagnostics);
            return null;
        }

        var temporary = Path.GetTempFileName();
        try
        {
            await using (var output = new FileStream(
                             temporary,
                             FileMode.Create,
                             FileAccess.Write,
                             FileShare.None,
                             64 * 1024,
                             FileOptions.Asynchronous))
            {
                await CopyCacheEntryToAsync(cachedStream, output, cancellationToken)
                    .ConfigureAwait(false);
            }

            ValidateArchiveEntryCount(temporary);
            using var archive = ZipFile.OpenRead(temporary);
            var resultEntry = archive.GetEntry(ResultEntryName)
                              ?? throw new InvalidDataException("Module cache entry does not contain result.json.");
            var result = await DeserializeResultAsync<T>(resultEntry, cancellationToken)
                .ConfigureAwait(false);

            if (result is not ModuleResult<T>.Success || result.ModuleStatus != Status.Successful)
            {
                throw new InvalidDataException("Module cache result is not a successful result.");
            }

            await RestoreArtifactsAsync(archive, module.GetType(), cancellationToken)
                .ConfigureAwait(false);
            DiscardFingerprint(module);
            _logger.LogInformation(
                "Module cache hit {Fingerprint} for {Module}",
                fingerprint,
                module.GetType().Name);
            return result;
        }
        finally
        {
            File.Delete(temporary);
        }
    }

    private async Task<bool> WriteCacheEntryAsync(
        string path,
        Stream serializedResult,
        Type moduleType,
        string fingerprint,
        CancellationToken cancellationToken)
    {
        await using var stream = new FileStream(
            path,
            FileMode.Create,
            FileAccess.ReadWrite,
            FileShare.None,
            64 * 1024,
            FileOptions.Asynchronous);
        try
        {
            var limitedStream = new MaximumLengthWriteStream(
                stream,
                _options.MaximumCacheEntryBytes);
            using var archive = new ZipArchive(limitedStream, ZipArchiveMode.Create, leaveOpen: true);
            var resultEntry = archive.CreateEntry(ResultEntryName, CompressionLevel.Fastest);
            await using (var resultEntryStream = resultEntry.Open())
            {
                await CopyWithLimitAsync(
                        serializedResult,
                        resultEntryStream,
                        _options.MaximumResultBytes,
                        "Cache result",
                        cancellationToken)
                    .ConfigureAwait(false);
            }

            await AddArtifactsAsync(archive, moduleType, cancellationToken)
                .ConfigureAwait(false);
        }
        catch (MaximumLengthExceededException)
        {
            _logger.LogDebug(
                "Skipping module cache save for {Module} because its archive exceeded the configured limit of {MaximumCacheEntryBytes} bytes",
                moduleType.Name,
                _options.MaximumCacheEntryBytes);
            return false;
        }

        stream.Position = 0;
        await _store.WriteAsync(fingerprint, stream, cancellationToken).ConfigureAwait(false);
        return true;
    }

    [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "Dependency result fingerprints require runtime result type metadata.")]
    [UnconditionalSuppressMessage("AOT", "IL3050", Justification = "Dependency result fingerprints require runtime result type metadata.")]
    private async Task<ComputedFingerprint> ComputeFingerprintAsync<T>(
        Module<T> module,
        IPipelineContext pipelineContext,
        CancellationToken cancellationToken)
    {
        var configuration = ((IModule) module).Configuration;
        var inputFiles = ModuleCacheFileResolver.ResolveFiles(
            _options.WorkingDirectory,
            configuration.CacheInputPatterns,
            _options.MaximumInputFiles,
            _options.CacheDirectory,
            rejectLinkedPaths: true);
        var hashes = await _fileHasher.HashAsync(
                inputFiles,
                _options.WorkingDirectory,
                cancellationToken)
            .ConfigureAwait(false);

        using var fingerprint = new FingerprintBuilder(_logger.IsEnabled(LogLevel.Debug));
        AppendModuleFingerprintData(
            fingerprint,
            module,
            configuration,
            inputFiles,
            hashes);
        var dependencyTypes = GetDependencyTypes(module);
        await AppendDependencyFingerprintsAsync(
                fingerprint,
                dependencyTypes,
                pipelineContext,
                cancellationToken)
            .ConfigureAwait(false);

        return fingerprint.Complete();
    }

    private void AppendModuleFingerprintData<T>(
        FingerprintBuilder fingerprint,
        Module<T> module,
        ModuleConfiguration configuration,
        IReadOnlyList<string> inputFiles,
        IReadOnlyDictionary<string, string> hashes)
    {
        fingerprint.Append("format", "2");
        fingerprint.Append("module", module.GetType().AssemblyQualifiedName ?? module.GetType().FullName!);
        if (configuration.CacheAssemblyVersionKey is { } assemblyVersionKey)
        {
            fingerprint.Append(
                "module-version",
                assemblyVersionKey,
                protectDiagnosticValue: true,
                diagnosticName: "module-version-override");
        }
        else
        {
            fingerprint.Append(
                "module-version",
                module.GetType().Assembly.ManifestModule.ModuleVersionId.ToString("N"),
                diagnosticName: "module-version-mvid");
        }

        foreach (var pattern in configuration.CacheInputPatterns)
        {
            fingerprint.Append("input-pattern", pattern);
        }

        foreach (var path in inputFiles)
        {
            fingerprint.Append("input-path", ModuleCacheFileResolver.GetRelativePath(_options.WorkingDirectory, path));
            fingerprint.Append("input-hash", hashes[path]);
        }

        foreach (var keyPart in configuration.CacheKeyParts)
        {
            fingerprint.Append("key-part", keyPart, protectDiagnosticValue: true);
        }

        foreach (var variableName in configuration.CacheEnvironmentVariables.Order(StringComparer.Ordinal))
        {
            var value = Environment.GetEnvironmentVariable(variableName);
            fingerprint.Append(
                $"environment:{variableName}:presence",
                value is null ? "unset" : "set");
            if (value is not null)
            {
                fingerprint.Append(
                    $"environment:{variableName}:value",
                    value,
                    protectDiagnosticValue: true);
            }
        }
    }

    private Type[] GetDependencyTypes<T>(Module<T> module)
    {
        var availableModuleTypes = _moduleLookup.Modules
            .Select(registeredModule => registeredModule.GetType())
            .Distinct()
            .ToArray();
        return ModuleDependencyResolver
            .GetAllDependencies(module, availableModuleTypes, _dependencyRegistry, _metadataRegistry)
            .Select(dependency => dependency.DependencyType)
            .Distinct()
            .OrderBy(dependencyType => dependencyType.FullName, StringComparer.Ordinal)
            .ToArray();
    }

    private static async Task AppendDependencyFingerprintsAsync(
        FingerprintBuilder fingerprint,
        IEnumerable<Type> dependencyTypes,
        IPipelineContext pipelineContext,
        CancellationToken cancellationToken)
    {
        var internalContext = (IInternalPipelineContext) pipelineContext;
        foreach (var dependencyType in dependencyTypes)
        {
            var dependencyModule = internalContext.GetModule(dependencyType);
            if (dependencyModule is null)
            {
                fingerprint.Append("dependency-missing", dependencyType.AssemblyQualifiedName!);
                continue;
            }

            var dependencyResult = await dependencyModule.ResultTask
                .WaitAsync(cancellationToken)
                .ConfigureAwait(false);
            AppendDependencyFingerprint(fingerprint, dependencyType, dependencyResult);
        }
    }

    [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "Dependency result fingerprints require runtime result type metadata.")]
    [UnconditionalSuppressMessage("AOT", "IL3050", Justification = "Dependency result fingerprints require runtime result type metadata.")]
    private static void AppendDependencyFingerprint(
        FingerprintBuilder fingerprint,
        Type dependencyType,
        IModuleResult dependencyResult)
    {
        fingerprint.Append("dependency", dependencyType.AssemblyQualifiedName!);
        fingerprint.Append(
            "dependency-status",
            dependencyResult.ModuleStatus is Status.UsedHistory or Status.CachedResult
                ? Status.Successful.ToString()
                : dependencyResult.ModuleStatus.ToString());

        if (dependencyResult.ValueOrDefault is { } value)
        {
            fingerprint.Append(
                "dependency-value-type",
                value.GetType().AssemblyQualifiedName ?? value.GetType().FullName!);
            var valueBytes = JsonSerializer.SerializeToUtf8Bytes(value, value.GetType());
            fingerprint.Append("dependency-value", Convert.ToHexString(SHA256.HashData(valueBytes)));
        }
        else
        {
            fingerprint.Append("dependency-value-type", "<null>");
            fingerprint.Append("dependency-value", "<null>");
        }

        if (dependencyResult.ExceptionOrDefault is { } exception)
        {
            fingerprint.Append("dependency-exception-type", exception.GetType().FullName!);
            fingerprint.Append(
                "dependency-exception-message",
                exception.Message,
                protectDiagnosticValue: true);
        }

        if (dependencyResult.SkipDecisionOrDefault is { } skipDecision)
        {
            fingerprint.Append(
                "dependency-skip",
                skipDecision.Reason ?? string.Empty,
                protectDiagnosticValue: true);
        }
    }

    private async Task AddArtifactsAsync(
        ZipArchive archive,
        Type moduleType,
        CancellationToken cancellationToken)
    {
        var artifactPaths = GetArtifactPaths(moduleType).ToArray();
        var directories = ModuleCacheFileResolver.ResolveDirectories(
            _options.WorkingDirectory,
            artifactPaths,
            _options.MaximumArtifactEntries,
            _options.CacheDirectory);
        var directoryLinks = ModuleCacheFileResolver.ResolveDirectoryLinks(
            _options.WorkingDirectory,
            artifactPaths,
            _options.MaximumArtifactEntries,
            _options.CacheDirectory);
        var files = ModuleCacheFileResolver.ResolveFiles(
            _options.WorkingDirectory,
            artifactPaths,
            _options.MaximumArtifactEntries,
            _options.CacheDirectory);

        var entryCount = checked(directories.Count + directoryLinks.Count + files.Count);
        if (entryCount > _options.MaximumArtifactEntries)
        {
            throw new InvalidDataException(
                $"Cache artifact entry count exceeded the configured limit of "
                + $"{_options.MaximumArtifactEntries:N0} entries.");
        }

        var byteBudget = new ArtifactByteBudget(_options.MaximumArtifactBytes);

        foreach (var directory in directories)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var relativePath = ModuleCacheFileResolver.GetRelativePath(_options.WorkingDirectory, directory);
            var entry = archive.CreateEntry(
                relativePath == "."
                    ? ArtifactPrefix
                    : $"{ArtifactPrefix}{relativePath.TrimEnd('/')}/");
            if (!OperatingSystem.IsWindows())
            {
                entry.ExternalAttributes =
                    (UnixFileTypeDirectory | (int) File.GetUnixFileMode(directory)) << 16;
            }
        }

        foreach (var directoryLink in directoryLinks)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var linkTarget = new DirectoryInfo(directoryLink).LinkTarget;
            if (linkTarget is null)
            {
                continue;
            }

            var relativePath = ModuleCacheFileResolver.GetRelativePath(
                _options.WorkingDirectory,
                directoryLink);
            var entry = archive.CreateEntry($"{ArtifactPrefix}{relativePath}");
            entry.ExternalAttributes =
                (UnixFileTypeSymbolicLink << 16) | (int) FileAttributes.Directory;
            var linkTargetBytes = Encoding.UTF8.GetBytes(linkTarget);
            byteBudget.Consume(linkTargetBytes.Length);
            await using var linkOutput = entry.Open();
            await linkOutput.WriteAsync(
                    linkTargetBytes,
                    cancellationToken)
                .ConfigureAwait(false);
        }

        foreach (var file in files)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var relativePath = ModuleCacheFileResolver.GetRelativePath(_options.WorkingDirectory, file);
            var entry = archive.CreateEntry(
                $"{ArtifactPrefix}{relativePath}",
                CompressionLevel.Fastest);
            if (new FileInfo(file).LinkTarget is { } linkTarget)
            {
                entry.ExternalAttributes = UnixFileTypeSymbolicLink << 16;
                var linkTargetBytes = Encoding.UTF8.GetBytes(linkTarget);
                byteBudget.Consume(linkTargetBytes.Length);
                await using var linkOutput = entry.Open();
                await linkOutput.WriteAsync(
                        linkTargetBytes,
                        cancellationToken)
                    .ConfigureAwait(false);
                continue;
            }

            if (!OperatingSystem.IsWindows())
            {
                entry.ExternalAttributes =
                    (UnixFileTypeRegular | (int) File.GetUnixFileMode(file)) << 16;
            }

            await using var input = new FileStream(
                file,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read,
                64 * 1024,
                FileOptions.Asynchronous | FileOptions.SequentialScan);
            await using var output = entry.Open();
            await byteBudget.CopyToAsync(input, output, cancellationToken).ConfigureAwait(false);
        }
    }

    private async Task RestoreArtifactsAsync(
        ZipArchive archive,
        Type moduleType,
        CancellationToken cancellationToken)
    {
        var root = Path.GetFullPath(_options.WorkingDirectory);
        var pathComparer = ModuleCacheFileResolver.GetPathComparer(root);
        var artifactPaths = GetArtifactPaths(moduleType).ToArray();
        var archivedArtifacts = new List<ZipArchiveEntry>();
        foreach (var entry in archive.Entries.Where(entry =>
                     entry.FullName.StartsWith(ArtifactPrefix, StringComparison.Ordinal)))
        {
            if (archivedArtifacts.Count >= _options.MaximumArtifactEntries)
            {
                throw new InvalidDataException(
                    $"Cache artifact entry count exceeded the configured limit of "
                    + $"{_options.MaximumArtifactEntries:N0} entries.");
            }

            archivedArtifacts.Add(entry);
        }

        var artifactEntries = archivedArtifacts
            .Select(entry => (
                Entry: entry,
                Destination: GetArtifactDestination(root, entry),
                IsDirectory: entry.FullName.EndsWith('/'),
                IsSymbolicLink: IsSymbolicLink(entry),
                IsDirectorySymbolicLink: IsDirectorySymbolicLink(entry)))
            .ToArray();
        ValidateDeclaredArtifactBytes(artifactEntries.Select(artifact => artifact.Entry));
        var byteBudget = new ArtifactByteBudget(_options.MaximumArtifactBytes);
        var symbolicLinkTargets = new Dictionary<ZipArchiveEntry, string>();

        foreach (var artifact in artifactEntries)
        {
            if (!ModuleCacheFileResolver.IsWithinDeclaredArtifactScope(
                    root,
                    artifact.Destination,
                    artifactPaths))
            {
                throw new InvalidDataException(
                    $"Cache artifact entry '{artifact.Entry.FullName}' is outside "
                    + "the module's declared artifact paths.");
            }
        }

        foreach (var symbolicLink in artifactEntries
                     .Where(artifact => artifact.IsSymbolicLink))
        {
            await using (var input = symbolicLink.Entry.Open())
            {
                var linkTarget = await byteBudget.ReadSymbolicLinkTargetAsync(
                        input,
                        cancellationToken)
                    .ConfigureAwait(false);
                ValidateSymbolicLinkTarget(root, symbolicLink.Destination, linkTarget);
                symbolicLinkTargets.Add(symbolicLink.Entry, linkTarget);
            }

            if (artifactEntries.Any(artifact =>
                    IsNestedPath(
                        symbolicLink.Destination,
                        artifact.Destination,
                        pathComparer)))
            {
                throw new InvalidDataException(
                    $"Cache artifact entry '{symbolicLink.Entry.FullName}' is a symbolic link "
                    + "with nested artifact entries.");
            }
        }

        using var artifactDirectoryModeRollback = new UnixDirectoryModeRollback(
            ModuleCacheFileResolver.ResolveDirectories(
                root,
                artifactPaths,
                _options.MaximumArtifactEntries,
                _options.CacheDirectory),
            pathComparer);
        using var writableArtifactParents = MakeArtifactParentsTemporarilyWritable(
            root,
            artifactPaths,
            artifactEntries
                .Select(artifact => artifact.Destination)
                .Concat(GetExactArtifactDestinations(root, artifactPaths)));
        try
        {
            ClearArtifacts(moduleType, cancellationToken);

            foreach (var destination in artifactEntries
                         .Select(artifact => artifact.Destination)
                         .Distinct(pathComparer))
            {
                cancellationToken.ThrowIfCancellationRequested();
                RemoveLinkedDestinationComponents(root, destination);
            }

            foreach (var (_, destination, isDirectory, _, _) in artifactEntries)
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (isDirectory)
                {
                    Directory.CreateDirectory(destination);
                }
            }

            foreach (var (entry, destination, isDirectory, isSymbolicLink, _) in artifactEntries)
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (isDirectory || isSymbolicLink)
                {
                    continue;
                }

                Directory.CreateDirectory(Path.GetDirectoryName(destination)!);
                await using (var input = entry.Open())
                await using (var output = new FileStream(
                                 destination,
                                 FileMode.Create,
                                 FileAccess.Write,
                                 FileShare.None,
                                 64 * 1024,
                                 FileOptions.Asynchronous))
                {
                    await byteBudget.CopyToAsync(input, output, cancellationToken)
                        .ConfigureAwait(false);
                }

                RestoreUnixMode(entry, destination, UnixFileTypeRegular);
            }

            foreach (var (entry, destination, _, _, isDirectorySymbolicLink) in artifactEntries
                         .Where(artifact => artifact.IsSymbolicLink))
            {
                cancellationToken.ThrowIfCancellationRequested();
                Directory.CreateDirectory(Path.GetDirectoryName(destination)!);
                var linkTarget = symbolicLinkTargets[entry];
                if (isDirectorySymbolicLink)
                {
                    Directory.CreateSymbolicLink(destination, linkTarget);
                }
                else
                {
                    File.CreateSymbolicLink(destination, linkTarget);
                }
            }

            foreach (var (entry, destination, _, _, _) in artifactEntries
                         .Where(artifact => artifact.IsDirectory)
                         .OrderByDescending(artifact => artifact.Destination.Length))
            {
                RestoreUnixMode(entry, destination, UnixFileTypeDirectory);
            }

            artifactDirectoryModeRollback.Complete(
                artifactEntries
                    .Where(artifact => artifact.IsDirectory)
                    .Select(artifact => artifact.Destination));
        }
        catch (Exception restoreException)
        {
            try
            {
                ClearArtifacts(moduleType, CancellationToken.None);
            }
            catch (Exception cleanupException)
            {
                throw new AggregateException(
                    "Module cache artifact restoration and rollback both failed.",
                    restoreException,
                    cleanupException);
            }

            throw;
        }
    }

    private void ValidateArchiveEntryCount(string path)
    {
        var entryCount = ZipCentralDirectory.ReadEntryCount(path);
        var maximumEntryCount = (long) _options.MaximumArtifactEntries + 1;
        if (entryCount > maximumEntryCount)
        {
            throw new InvalidDataException(
                $"Cache archive entry count exceeded the configured artifact limit of "
                + $"{_options.MaximumArtifactEntries:N0} entries.");
        }
    }

    private void ValidateDeclaredArtifactBytes(IEnumerable<ZipArchiveEntry> entries)
    {
        var totalBytes = 0L;
        foreach (var entry in entries)
        {
            try
            {
                totalBytes = checked(totalBytes + entry.Length);
            }
            catch (OverflowException exception)
            {
                throw new InvalidDataException(
                    "Cache artifact uncompressed size is invalid.",
                    exception);
            }

            if (totalBytes > _options.MaximumArtifactBytes)
            {
                throw new InvalidDataException(
                    $"Cache artifact data exceeded the configured limit of "
                    + $"{_options.MaximumArtifactBytes:N0} bytes.");
            }
        }
    }

    private UnixDirectoryModeScope MakeArtifactParentsTemporarilyWritable(
        string root,
        IReadOnlyCollection<string> artifactPaths,
        IEnumerable<string> destinations)
    {
        if (OperatingSystem.IsWindows())
        {
            return new UnixDirectoryModeScope([]);
        }

        var pathComparer = ModuleCacheFileResolver.GetPathComparer(root);
        var artifactDirectories = ModuleCacheFileResolver.ResolveDirectories(
                root,
                artifactPaths,
                _options.MaximumArtifactEntries,
                _options.CacheDirectory)
            .ToHashSet(pathComparer);
        var existingArtifactDestinations = artifactDirectories
            .Concat(ModuleCacheFileResolver.ResolveDirectoryLinks(
                root,
                artifactPaths,
                _options.MaximumArtifactEntries,
                _options.CacheDirectory))
            .Concat(ModuleCacheFileResolver.ResolveFiles(
                root,
                artifactPaths,
                _options.MaximumArtifactEntries,
                _options.CacheDirectory));
        var parentDirectories = new HashSet<string>(pathComparer);
        foreach (var destination in destinations.Concat(existingArtifactDestinations))
        {
            if (pathComparer.Equals(destination, root))
            {
                continue;
            }

            for (var current = Path.GetDirectoryName(destination);
                 current is not null;
                 current = Path.GetDirectoryName(current))
            {
                ModuleCacheFileResolver.GetRelativePath(root, current);
                if (Directory.Exists(current)
                    && !artifactDirectories.Contains(current)
                    && !TryGetReparsePointAttributes(current, out _))
                {
                    parentDirectories.Add(current);
                }

                if (pathComparer.Equals(current, root))
                {
                    break;
                }
            }
        }

        return new UnixDirectoryModeScope(parentDirectories);
    }

    private void ClearArtifacts(Type moduleType, CancellationToken cancellationToken)
    {
        var artifactPaths = GetArtifactPaths(moduleType).ToArray();
        var root = Path.GetFullPath(_options.WorkingDirectory);
        var pathComparer = ModuleCacheFileResolver.GetPathComparer(root);
        RemoveExactArtifactLinks(root, artifactPaths, cancellationToken);

        var directoryLinks = ModuleCacheFileResolver.ResolveDirectoryLinks(
            _options.WorkingDirectory,
            artifactPaths,
            _options.MaximumArtifactEntries,
            _options.CacheDirectory);
        var directories = ModuleCacheFileResolver.ResolveDirectories(
            _options.WorkingDirectory,
            artifactPaths,
            _options.MaximumArtifactEntries,
            _options.CacheDirectory);
        var files = ModuleCacheFileResolver.ResolveFiles(
            _options.WorkingDirectory,
            artifactPaths,
            _options.MaximumArtifactEntries,
            _options.CacheDirectory);
        MakeDirectoriesWritable(directories);
        MakeFilesWritable(files);

        foreach (var directoryLink in directoryLinks.OrderByDescending(path => path.Length))
        {
            cancellationToken.ThrowIfCancellationRequested();
            Directory.Delete(directoryLink);
        }

        foreach (var file in files)
        {
            cancellationToken.ThrowIfCancellationRequested();
            File.Delete(file);
        }

        foreach (var directory in directories.OrderByDescending(path => path.Length))
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (!pathComparer.Equals(root, directory)
                && Directory.Exists(directory)
                && !Directory.EnumerateFileSystemEntries(directory).Any())
            {
                Directory.Delete(directory);
            }
        }
    }

    private static void MakeDirectoriesWritable(IEnumerable<string> directories)
    {
        if (OperatingSystem.IsWindows())
        {
            return;
        }

        const UnixFileMode requiredMode =
            UnixFileMode.UserWrite | UnixFileMode.UserExecute;
        foreach (var directory in directories.OrderBy(path => path.Length))
        {
            var mode = File.GetUnixFileMode(directory);
            if ((mode & requiredMode) != requiredMode)
            {
                File.SetUnixFileMode(directory, mode | requiredMode);
            }
        }
    }

    private static void MakeFilesWritable(IEnumerable<string> files)
    {
        if (!OperatingSystem.IsWindows())
        {
            return;
        }

        foreach (var file in files)
        {
            var attributes = File.GetAttributes(file);
            if ((attributes & (FileAttributes.ReadOnly | FileAttributes.ReparsePoint))
                == FileAttributes.ReadOnly)
            {
                File.SetAttributes(file, attributes & ~FileAttributes.ReadOnly);
            }
        }
    }

    private static void RemoveExactArtifactLinks(
        string root,
        IEnumerable<string> artifactPaths,
        CancellationToken cancellationToken)
    {
        foreach (var destination in GetExactArtifactDestinations(root, artifactPaths))
        {
            cancellationToken.ThrowIfCancellationRequested();
            RemoveLinkedDestinationComponents(root, destination);
        }
    }

    private static IEnumerable<string> GetExactArtifactDestinations(
        string root,
        IEnumerable<string> artifactPaths)
    {
        foreach (var artifactPath in artifactPaths)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(artifactPath);
            if (artifactPath.IndexOfAny(['*', '?']) >= 0)
            {
                continue;
            }

            var destination = Path.IsPathRooted(artifactPath)
                ? Path.GetFullPath(artifactPath)
                : Path.GetFullPath(Path.Combine(root, artifactPath));
            ModuleCacheFileResolver.GetRelativePath(root, destination);
            yield return destination;
        }
    }

    private static void RemoveLinkedDestinationComponents(string root, string destination)
    {
        var relativePath = Path.GetRelativePath(root, destination);
        var currentPath = root;

        foreach (var component in relativePath.Split(
                     [Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar],
                     StringSplitOptions.RemoveEmptyEntries))
        {
            currentPath = Path.Combine(currentPath, component);
            if (!TryGetReparsePointAttributes(currentPath, out var attributes))
            {
                continue;
            }

            if ((attributes & FileAttributes.Directory) != 0)
            {
                Directory.Delete(currentPath);
            }
            else
            {
                File.Delete(currentPath);
            }
        }
    }

    private static bool TryGetReparsePointAttributes(
        string path,
        out FileAttributes attributes)
    {
        try
        {
            attributes = File.GetAttributes(path);
            return (attributes & FileAttributes.ReparsePoint) != 0;
        }
        catch (FileNotFoundException)
        {
            attributes = default;
            return false;
        }
        catch (DirectoryNotFoundException)
        {
            attributes = default;
            return false;
        }
    }

    private static bool IsDirectorySymbolicLink(ZipArchiveEntry entry) =>
        IsSymbolicLink(entry)
        && (entry.ExternalAttributes & (int) FileAttributes.Directory) != 0;

    private static IEnumerable<string> GetArtifactPaths(Type moduleType) =>
        moduleType
            .GetCustomAttributes(typeof(ProducesArtifactAttribute), inherit: true)
            .Cast<ProducesArtifactAttribute>()
            .Select(attribute => attribute.PathPattern);

    private static void ValidateSymbolicLinkTarget(
        string root,
        string destination,
        string linkTarget)
    {
        var parent = Path.GetDirectoryName(destination)!;
        var resolvedTarget = Path.IsPathRooted(linkTarget)
            ? Path.GetFullPath(linkTarget)
            : Path.GetFullPath(Path.Combine(parent, linkTarget));
        if (!ModuleCacheFileResolver.IsWithin(root, resolvedTarget))
        {
            throw new InvalidDataException(
                $"Cache artifact symbolic-link target '{linkTarget}' escapes the working directory.");
        }

        var relativeTarget = Path.GetRelativePath(root, resolvedTarget);
        var currentPath = root;
        foreach (var component in relativeTarget.Split(
                     [Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar],
                     StringSplitOptions.RemoveEmptyEntries))
        {
            if (component == ".")
            {
                continue;
            }

            currentPath = Path.Combine(currentPath, component);
            if (TryGetReparsePointAttributes(currentPath, out _))
            {
                throw new InvalidDataException(
                    $"Cache artifact symbolic-link target '{linkTarget}' traverses a linked path.");
            }
        }
    }

    private static string GetArtifactDestination(string root, ZipArchiveEntry entry)
    {
        var relativePath = entry.FullName[ArtifactPrefix.Length..]
            .Replace('/', Path.DirectorySeparatorChar);
        var destination = Path.GetFullPath(Path.Combine(root, relativePath));
        if (!ModuleCacheFileResolver.IsWithin(root, destination))
        {
            throw new InvalidDataException($"Cache artifact path '{entry.FullName}' escapes the working directory.");
        }

        return destination;
    }

    private static bool IsNestedPath(
        string parent,
        string candidate,
        StringComparer pathComparer)
    {
        var normalizedParent = parent.TrimEnd(
            Path.DirectorySeparatorChar,
            Path.AltDirectorySeparatorChar);
        return candidate.Length > normalizedParent.Length
               && pathComparer.Equals(
                   normalizedParent,
                   candidate[..normalizedParent.Length])
               && candidate[normalizedParent.Length] is var separator
               && (separator == Path.DirectorySeparatorChar
                   || separator == Path.AltDirectorySeparatorChar);
    }

    private async Task CopyCacheEntryToAsync(
        Stream input,
        Stream output,
        CancellationToken cancellationToken)
    {
        await CopyWithLimitAsync(
                input,
                output,
                _options.MaximumCacheEntryBytes,
                "Cache entry",
                cancellationToken)
            .ConfigureAwait(false);
    }

    [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "Module result cache requires runtime result type metadata.")]
    [UnconditionalSuppressMessage("AOT", "IL3050", Justification = "Module result cache requires runtime result type metadata.")]
    private async Task<ModuleResult<T>?> DeserializeResultAsync<T>(
        ZipArchiveEntry resultEntry,
        CancellationToken cancellationToken)
    {
        if (resultEntry.Length > _options.MaximumResultBytes)
        {
            throw new InvalidDataException(
                $"Cache result exceeded the configured limit of {_options.MaximumResultBytes:N0} bytes.");
        }

        var temporaryResult = Path.GetTempFileName();
        try
        {
            await using (var input = resultEntry.Open())
            await using (var output = new FileStream(
                             temporaryResult,
                             FileMode.Create,
                             FileAccess.Write,
                             FileShare.None,
                             64 * 1024,
                             FileOptions.Asynchronous))
            {
                await CopyWithLimitAsync(
                        input,
                        output,
                        _options.MaximumResultBytes,
                        "Cache result",
                        cancellationToken)
                    .ConfigureAwait(false);
            }

            await using var validatedResult = new FileStream(
                temporaryResult,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read,
                64 * 1024,
                FileOptions.Asynchronous | FileOptions.SequentialScan);
            return await JsonSerializer.DeserializeAsync<ModuleResult<T>>(
                    validatedResult,
                    cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }
        finally
        {
            File.Delete(temporaryResult);
        }
    }

    private static async Task CopyWithLimitAsync(
        Stream input,
        Stream output,
        long maximumBytes,
        string description,
        CancellationToken cancellationToken)
    {
        const int bufferSize = 64 * 1024;
        var buffer = ArrayPool<byte>.Shared.Rent(bufferSize);
        var totalBytes = 0L;
        try
        {
            while (true)
            {
                var bytesRead = await input.ReadAsync(
                        buffer.AsMemory(0, bufferSize),
                        cancellationToken)
                    .ConfigureAwait(false);
                if (bytesRead == 0)
                {
                    return;
                }

                if (totalBytes > maximumBytes - bytesRead)
                {
                    throw new InvalidDataException(
                        $"{description} exceeded the configured limit of {maximumBytes:N0} bytes.");
                }

                totalBytes += bytesRead;
                await output.WriteAsync(
                        buffer.AsMemory(0, bytesRead),
                        cancellationToken)
                    .ConfigureAwait(false);
            }
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }

    private static void RestoreUnixMode(
        ZipArchiveEntry entry,
        string destination,
        int expectedFileType)
    {
        if (OperatingSystem.IsWindows())
        {
            return;
        }

        var unixAttributes = (entry.ExternalAttributes >> 16) & 0xFFFF;
        if ((unixAttributes & expectedFileType) == expectedFileType)
        {
            var permissions = Enum.ToObject(
                typeof(UnixFileMode),
                unixAttributes & UnixPermissionMask);
            if (permissions is UnixFileMode unixFileMode)
            {
                File.SetUnixFileMode(destination, unixFileMode);
            }
        }
    }

    private static bool IsSymbolicLink(ZipArchiveEntry entry)
    {
        var unixAttributes = (entry.ExternalAttributes >> 16) & 0xFFFF;
        return (unixAttributes & UnixFileTypeMask) == UnixFileTypeSymbolicLink;
    }

    private static void AppendLengthPrefixed(IncrementalHash hash, byte[] value)
    {
        Span<byte> length = stackalloc byte[sizeof(int)];
        BinaryPrimitives.WriteInt32LittleEndian(length, value.Length);
        hash.AppendData(length);
        hash.AppendData(value);
    }

    private sealed record ComputedFingerprint(string Value, string Diagnostics);

    private sealed class FingerprintBuilder(bool captureDiagnostics) : IDisposable
    {
        private const int MaximumLoggedComponents = 128;
        private readonly IncrementalHash _hash = IncrementalHash.CreateHash(HashAlgorithmName.SHA256);
        private readonly List<string>? _components = captureDiagnostics ? [] : null;

        public void Append(
            string name,
            string value,
            bool protectDiagnosticValue = false,
            string? diagnosticName = null)
        {
            AppendLengthPrefixed(_hash, Encoding.UTF8.GetBytes(name));
            AppendLengthPrefixed(_hash, Encoding.UTF8.GetBytes(value));

            if (_components is null)
            {
                return;
            }

            var diagnosticValue = protectDiagnosticValue
                ? $"sha256:{Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(value)))}"
                : value;
            _components.Add($"{diagnosticName ?? name}=\"{JsonEncodedText.Encode(diagnosticValue)}\"");
        }

        public ComputedFingerprint Complete()
        {
            var value = Convert.ToHexString(_hash.GetHashAndReset());
            return new ComputedFingerprint(value, BuildDiagnostics());
        }

        public void Dispose() => _hash.Dispose();

        private string BuildDiagnostics()
        {
            if (_components is null)
            {
                return string.Empty;
            }

            if (_components.Count <= MaximumLoggedComponents)
            {
                return string.Join(", ", _components);
            }

            var omittedComponents = string.Join(", ", _components.Skip(MaximumLoggedComponents));
            var omittedDigest = Convert.ToHexString(
                SHA256.HashData(Encoding.UTF8.GetBytes(omittedComponents)));
            return string.Join(", ", _components.Take(MaximumLoggedComponents))
                   + $", ... {_components.Count - MaximumLoggedComponents} component(s) omitted"
                   + $" (sha256:{omittedDigest})";
        }
    }

    private sealed class ArtifactByteBudget
    {
        private const int BufferSize = 64 * 1024;
        private const int MaximumSymbolicLinkTargetBytes = 64 * 1024;
        private long _remainingBytes;

        public ArtifactByteBudget(long maximumBytes)
        {
            _remainingBytes = maximumBytes;
        }

        public void Consume(int byteCount)
        {
            if (byteCount > _remainingBytes)
            {
                throw new InvalidDataException(
                    "Cache artifact data exceeded the configured uncompressed-size limit.");
            }

            _remainingBytes -= byteCount;
        }

        public Task CopyToAsync(
            Stream input,
            Stream output,
            CancellationToken cancellationToken) =>
            CopyToAsync(input, output, maximumEntryBytes: null, cancellationToken);

        public async Task<string> ReadSymbolicLinkTargetAsync(
            Stream input,
            CancellationToken cancellationToken)
        {
            using var output = new MemoryStream();
            await CopyToAsync(
                    input,
                    output,
                    MaximumSymbolicLinkTargetBytes,
                    cancellationToken)
                .ConfigureAwait(false);
            return Encoding.UTF8.GetString(output.ToArray());
        }

        private async Task CopyToAsync(
            Stream input,
            Stream output,
            int? maximumEntryBytes,
            CancellationToken cancellationToken)
        {
            var buffer = ArrayPool<byte>.Shared.Rent(BufferSize);
            var entryBytes = 0;
            try
            {
                while (true)
                {
                    var bytesRead = await input.ReadAsync(
                            buffer.AsMemory(0, BufferSize),
                            cancellationToken)
                        .ConfigureAwait(false);
                    if (bytesRead == 0)
                    {
                        return;
                    }

                    if (maximumEntryBytes is { } maximum
                        && entryBytes > maximum - bytesRead)
                    {
                        throw new InvalidDataException(
                            "Cache artifact symbolic-link target exceeded the configured limit.");
                    }

                    Consume(bytesRead);
                    entryBytes += bytesRead;
                    await output.WriteAsync(
                            buffer.AsMemory(0, bytesRead),
                            cancellationToken)
                        .ConfigureAwait(false);
                }
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buffer);
            }
        }
    }

    private sealed class UnixDirectoryModeScope : IDisposable
    {
        private readonly IReadOnlyList<(string Path, UnixFileMode Mode)> _originalModes;

        public UnixDirectoryModeScope(IEnumerable<string> directories)
        {
            if (OperatingSystem.IsWindows())
            {
                _originalModes = [];
                return;
            }

            const UnixFileMode requiredMode =
                UnixFileMode.UserWrite | UnixFileMode.UserExecute;
            var originalModes = new List<(string Path, UnixFileMode Mode)>();
            foreach (var path in directories.OrderBy(path => path.Length))
            {
                originalModes.Add((path, File.GetUnixFileMode(path)));
            }

            _originalModes = originalModes;
            foreach (var (path, mode) in _originalModes)
            {
                if ((mode & requiredMode) != requiredMode)
                {
                    File.SetUnixFileMode(path, mode | requiredMode);
                }
            }
        }

        public void Dispose()
        {
            if (OperatingSystem.IsWindows())
            {
                return;
            }

            foreach (var (path, mode) in _originalModes
                         .OrderByDescending(item => item.Path.Length))
            {
                if (Directory.Exists(path))
                {
                    File.SetUnixFileMode(path, mode);
                }
            }
        }
    }

    private sealed class UnixDirectoryModeRollback : IDisposable
    {
        private readonly IReadOnlyList<(string Path, UnixFileMode Mode)> _originalModes;
        private readonly StringComparer _pathComparer;
        private bool _completed;

        public UnixDirectoryModeRollback(
            IEnumerable<string> directories,
            StringComparer pathComparer)
        {
            if (OperatingSystem.IsWindows())
            {
                _originalModes = [];
                _pathComparer = pathComparer;
                return;
            }

            var originalModes = new List<(string Path, UnixFileMode Mode)>();
            foreach (var path in directories.Distinct(pathComparer))
            {
                originalModes.Add((path, File.GetUnixFileMode(path)));
            }

            _originalModes = originalModes;
            _pathComparer = pathComparer;
        }

        public void Complete(IEnumerable<string> restoredDirectories)
        {
            var restored = restoredDirectories.ToHashSet(_pathComparer);
            RestoreOriginalModes(item => !restored.Contains(item.Path));
            _completed = true;
        }

        public void Dispose()
        {
            if (_completed || OperatingSystem.IsWindows())
            {
                return;
            }

            RestoreOriginalModes(static _ => true);
        }

        private void RestoreOriginalModes(
            Func<(string Path, UnixFileMode Mode), bool> predicate)
        {
            if (OperatingSystem.IsWindows())
            {
                return;
            }

            foreach (var (path, mode) in _originalModes
                         .Where(predicate)
                         .OrderByDescending(item => item.Path.Length))
            {
                if (Directory.Exists(path))
                {
                    File.SetUnixFileMode(path, mode);
                }
            }
        }
    }
}
