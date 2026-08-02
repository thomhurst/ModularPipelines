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

            var temporary = Path.GetTempFileName();
            try
            {
                await using (var stream = new FileStream(
                                 temporary,
                                 FileMode.Create,
                                 FileAccess.ReadWrite,
                                 FileShare.None,
                                 64 * 1024,
                                 FileOptions.Asynchronous))
                {
                    using (var archive = new ZipArchive(stream, ZipArchiveMode.Create, leaveOpen: true))
                    {
                        var resultEntry = archive.CreateEntry(ResultEntryName, CompressionLevel.Fastest);
                        await using (var resultStream = resultEntry.Open())
                        {
                            await JsonSerializer.SerializeAsync<ModuleResult<T>>(
                                    resultStream,
                                    moduleResult,
                                    cancellationToken: cancellationToken)
                                .ConfigureAwait(false);
                        }

                        await AddArtifactsAsync(archive, module.GetType(), cancellationToken)
                            .ConfigureAwait(false);
                    }

                    stream.Position = 0;
                    await _store.WriteAsync(fingerprint, stream, cancellationToken).ConfigureAwait(false);
                }

                _logger.LogDebug(
                    "Saved module cache entry {Fingerprint} for {Module}",
                    fingerprint,
                    module.GetType().Name);
            }
            finally
            {
                File.Delete(temporary);
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

        var fingerprint = await ComputeFingerprintAsync(module, pipelineContext, cancellationToken)
            .ConfigureAwait(false);
        _fingerprints[module] = fingerprint;
        await using var cachedStream = await _store.OpenReadAsync(fingerprint, cancellationToken)
            .ConfigureAwait(false);
        if (cachedStream is null)
        {
            _logger.LogDebug(
                "Module cache miss {Fingerprint} for {Module}",
                fingerprint,
                module.GetType().Name);
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
                await cachedStream.CopyToAsync(output, cancellationToken).ConfigureAwait(false);
            }

            ValidateArchiveEntryCount(temporary);
            using var archive = ZipFile.OpenRead(temporary);
            var resultEntry = archive.GetEntry(ResultEntryName)
                              ?? throw new InvalidDataException("Module cache entry does not contain result.json.");
            ModuleResult<T>? result;
            await using (var resultStream = resultEntry.Open())
            {
                result = await JsonSerializer.DeserializeAsync<ModuleResult<T>>(
                        resultStream,
                        cancellationToken: cancellationToken)
                    .ConfigureAwait(false);
            }

            if (result is null)
            {
                throw new InvalidDataException("Module cache result is empty.");
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

    [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "Dependency result fingerprints require runtime result type metadata.")]
    [UnconditionalSuppressMessage("AOT", "IL3050", Justification = "Dependency result fingerprints require runtime result type metadata.")]
    private async Task<string> ComputeFingerprintAsync<T>(
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

        using var incrementalHash = IncrementalHash.CreateHash(HashAlgorithmName.SHA256);
        AppendModuleFingerprintData(
            incrementalHash,
            module,
            configuration,
            inputFiles,
            hashes);
        var dependencyTypes = GetDependencyTypes(module);
        await AppendDependencyFingerprintsAsync(
                incrementalHash,
                dependencyTypes,
                pipelineContext,
                cancellationToken)
            .ConfigureAwait(false);

        return Convert.ToHexString(incrementalHash.GetHashAndReset());
    }

    private void AppendModuleFingerprintData<T>(
        IncrementalHash incrementalHash,
        Module<T> module,
        ModuleConfiguration configuration,
        IReadOnlyList<string> inputFiles,
        IReadOnlyDictionary<string, string> hashes)
    {
        Append(incrementalHash, "format", "2");
        Append(incrementalHash, "module", module.GetType().AssemblyQualifiedName ?? module.GetType().FullName!);
        Append(incrementalHash, "module-version", module.GetType().Assembly.ManifestModule.ModuleVersionId.ToString("N"));

        foreach (var pattern in configuration.CacheInputPatterns)
        {
            Append(incrementalHash, "input-pattern", pattern);
        }

        foreach (var path in inputFiles)
        {
            Append(incrementalHash, "input-path", ModuleCacheFileResolver.GetRelativePath(_options.WorkingDirectory, path));
            Append(incrementalHash, "input-hash", hashes[path]);
        }

        foreach (var keyPart in configuration.CacheKeyParts)
        {
            Append(incrementalHash, "key-part", keyPart);
        }

        foreach (var variableName in configuration.CacheEnvironmentVariables.Order(StringComparer.Ordinal))
        {
            var value = Environment.GetEnvironmentVariable(variableName);
            Append(
                incrementalHash,
                $"environment:{variableName}:presence",
                value is null ? "unset" : "set");
            if (value is not null)
            {
                Append(incrementalHash, $"environment:{variableName}:value", value);
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
        IncrementalHash incrementalHash,
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
                Append(incrementalHash, "dependency-missing", dependencyType.AssemblyQualifiedName!);
                continue;
            }

            var dependencyResult = await dependencyModule.ResultTask
                .WaitAsync(cancellationToken)
                .ConfigureAwait(false);
            AppendDependencyFingerprint(incrementalHash, dependencyType, dependencyResult);
        }
    }

    [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "Dependency result fingerprints require runtime result type metadata.")]
    [UnconditionalSuppressMessage("AOT", "IL3050", Justification = "Dependency result fingerprints require runtime result type metadata.")]
    private static void AppendDependencyFingerprint(
        IncrementalHash incrementalHash,
        Type dependencyType,
        IModuleResult dependencyResult)
    {
        Append(incrementalHash, "dependency", dependencyType.AssemblyQualifiedName!);
        Append(
            incrementalHash,
            "dependency-status",
            dependencyResult.ModuleStatus == Status.UsedHistory
                ? Status.Successful.ToString()
                : dependencyResult.ModuleStatus.ToString());

        if (dependencyResult.ValueOrDefault is { } value)
        {
            Append(
                incrementalHash,
                "dependency-value-type",
                value.GetType().AssemblyQualifiedName ?? value.GetType().FullName!);
            var valueBytes = JsonSerializer.SerializeToUtf8Bytes(value, value.GetType());
            Append(incrementalHash, "dependency-value", Convert.ToHexString(SHA256.HashData(valueBytes)));
        }
        else
        {
            Append(incrementalHash, "dependency-value-type", "<null>");
            Append(incrementalHash, "dependency-value", "<null>");
        }

        if (dependencyResult.ExceptionOrDefault is { } exception)
        {
            Append(incrementalHash, "dependency-exception-type", exception.GetType().FullName!);
            Append(incrementalHash, "dependency-exception-message", exception.Message);
        }

        if (dependencyResult.SkipDecisionOrDefault is { } skipDecision)
        {
            Append(incrementalHash, "dependency-skip", skipDecision.Reason ?? string.Empty);
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
            _options.MaximumInputFiles,
            _options.CacheDirectory);
        var directoryLinks = ModuleCacheFileResolver.ResolveDirectoryLinks(
            _options.WorkingDirectory,
            artifactPaths,
            _options.MaximumInputFiles,
            _options.CacheDirectory);
        var files = ModuleCacheFileResolver.ResolveFiles(
            _options.WorkingDirectory,
            artifactPaths,
            _options.MaximumInputFiles,
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
            if (artifactEntries.Any(artifact =>
                    IsNestedPath(symbolicLink.Destination, artifact.Destination)))
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
                _options.MaximumInputFiles,
                _options.CacheDirectory));
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
                         .Distinct(PathComparer))
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
                await using var input = entry.Open();
                var linkTarget = await byteBudget.ReadSymbolicLinkTargetAsync(
                        input,
                        cancellationToken)
                    .ConfigureAwait(false);
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

            artifactDirectoryModeRollback.Complete();
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

        var artifactDirectories = ModuleCacheFileResolver.ResolveDirectories(
                root,
                artifactPaths,
                _options.MaximumInputFiles,
                _options.CacheDirectory)
            .ToHashSet(PathComparer);
        var existingArtifactDestinations = artifactDirectories
            .Concat(ModuleCacheFileResolver.ResolveDirectoryLinks(
                root,
                artifactPaths,
                _options.MaximumInputFiles,
                _options.CacheDirectory))
            .Concat(ModuleCacheFileResolver.ResolveFiles(
                root,
                artifactPaths,
                _options.MaximumInputFiles,
                _options.CacheDirectory));
        var parentDirectories = new HashSet<string>(PathComparer);
        foreach (var destination in destinations.Concat(existingArtifactDestinations))
        {
            if (PathComparer.Equals(destination, root))
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

                if (PathComparer.Equals(current, root))
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
        RemoveExactArtifactLinks(root, artifactPaths, cancellationToken);

        var directoryLinks = ModuleCacheFileResolver.ResolveDirectoryLinks(
            _options.WorkingDirectory,
            artifactPaths,
            _options.MaximumInputFiles,
            _options.CacheDirectory);
        var directories = ModuleCacheFileResolver.ResolveDirectories(
            _options.WorkingDirectory,
            artifactPaths,
            _options.MaximumInputFiles,
            _options.CacheDirectory);
        var files = ModuleCacheFileResolver.ResolveFiles(
            _options.WorkingDirectory,
            artifactPaths,
            _options.MaximumInputFiles,
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
            if (!PathComparer.Equals(root, directory)
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

    private static string GetArtifactDestination(string root, ZipArchiveEntry entry)
    {
        var relativePath = entry.FullName[ArtifactPrefix.Length..]
            .Replace('/', Path.DirectorySeparatorChar);
        var destination = Path.GetFullPath(Path.Combine(root, relativePath));
        var verifiedRelativePath = Path.GetRelativePath(root, destination);
        if (verifiedRelativePath == ".."
            || verifiedRelativePath.StartsWith($"..{Path.DirectorySeparatorChar}", StringComparison.Ordinal)
            || Path.IsPathRooted(verifiedRelativePath))
        {
            throw new InvalidDataException($"Cache artifact path '{entry.FullName}' escapes the working directory.");
        }

        return destination;
    }

    private static bool IsNestedPath(string parent, string candidate)
    {
        var relativePath = Path.GetRelativePath(parent, candidate);
        return relativePath != "."
               && relativePath != ".."
               && !relativePath.StartsWith(
                   $"..{Path.DirectorySeparatorChar}",
                   StringComparison.Ordinal)
               && !Path.IsPathRooted(relativePath);
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

    private static void Append(IncrementalHash hash, string name, string value)
    {
        AppendLengthPrefixed(hash, Encoding.UTF8.GetBytes(name));
        AppendLengthPrefixed(hash, Encoding.UTF8.GetBytes(value));
    }

    private static void AppendLengthPrefixed(IncrementalHash hash, byte[] value)
    {
        Span<byte> length = stackalloc byte[sizeof(int)];
        BinaryPrimitives.WriteInt32LittleEndian(length, value.Length);
        hash.AppendData(length);
        hash.AppendData(value);
    }

    private static StringComparer PathComparer =>
        OperatingSystem.IsWindows() ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal;

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
            foreach (var path in directories
                         .Distinct(PathComparer)
                         .OrderBy(path => path.Length))
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
        private bool _completed;

        public UnixDirectoryModeRollback(IEnumerable<string> directories)
        {
            if (OperatingSystem.IsWindows())
            {
                _originalModes = [];
                return;
            }

            var originalModes = new List<(string Path, UnixFileMode Mode)>();
            foreach (var path in directories.Distinct(PathComparer))
            {
                originalModes.Add((path, File.GetUnixFileMode(path)));
            }

            _originalModes = originalModes;
        }

        public void Complete() => _completed = true;

        public void Dispose()
        {
            if (_completed || OperatingSystem.IsWindows())
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
}
