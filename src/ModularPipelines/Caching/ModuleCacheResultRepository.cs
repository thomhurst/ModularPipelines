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
                fingerprint = await ComputeFingerprintAsync(module, pipelineContext, cancellationToken)
                    .ConfigureAwait(false);
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
            _options.CacheDirectory);
        var hashes = await _fileHasher.HashAsync(inputFiles, cancellationToken).ConfigureAwait(false);

        using var incrementalHash = IncrementalHash.CreateHash(HashAlgorithmName.SHA256);
        Append(incrementalHash, "format", "1");
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
            Append(incrementalHash, $"environment:{variableName}", Environment.GetEnvironmentVariable(variableName) ?? "<null>");
        }

        var availableModuleTypes = _moduleLookup.Modules
            .Select(registeredModule => registeredModule.GetType())
            .Distinct()
            .ToArray();
        var dependencyTypes = ModuleDependencyResolver
            .GetAllDependencies(module, availableModuleTypes, _dependencyRegistry, _metadataRegistry)
            .Select(dependency => dependency.DependencyType)
            .Distinct()
            .OrderBy(dependencyType => dependencyType.FullName, StringComparer.Ordinal)
            .ToArray();

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
            Append(incrementalHash, "dependency", dependencyType.AssemblyQualifiedName!);
            Append(
                incrementalHash,
                "dependency-status",
                dependencyResult.ModuleStatus == Status.UsedHistory
                    ? Status.Successful.ToString()
                    : dependencyResult.ModuleStatus.ToString());

            if (dependencyResult.ValueOrDefault is { } value)
            {
                var valueBytes = JsonSerializer.SerializeToUtf8Bytes(value, value.GetType());
                Append(incrementalHash, "dependency-value", Convert.ToHexString(SHA256.HashData(valueBytes)));
            }
            else
            {
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

        return Convert.ToHexString(incrementalHash.GetHashAndReset());
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
        var files = ModuleCacheFileResolver.ResolveFiles(
            _options.WorkingDirectory,
            artifactPaths,
            _options.MaximumInputFiles,
            _options.CacheDirectory);

        foreach (var directory in directories)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var relativePath = ModuleCacheFileResolver.GetRelativePath(_options.WorkingDirectory, directory);
            if (relativePath != ".")
            {
                var entry = archive.CreateEntry($"{ArtifactPrefix}{relativePath.TrimEnd('/')}/");
                if (!OperatingSystem.IsWindows())
                {
                    entry.ExternalAttributes =
                        (UnixFileTypeDirectory | (int) File.GetUnixFileMode(directory)) << 16;
                }
            }
        }

        foreach (var file in files)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var relativePath = ModuleCacheFileResolver.GetRelativePath(_options.WorkingDirectory, file);
            var entry = archive.CreateEntry(
                $"{ArtifactPrefix}{relativePath}",
                CompressionLevel.Fastest);
            if (!OperatingSystem.IsWindows()
                && new FileInfo(file).LinkTarget is { } linkTarget)
            {
                entry.ExternalAttributes = UnixFileTypeSymbolicLink << 16;
                await using var linkOutput = entry.Open();
                await linkOutput.WriteAsync(
                        Encoding.UTF8.GetBytes(linkTarget),
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
            await input.CopyToAsync(output, cancellationToken).ConfigureAwait(false);
        }
    }

    private async Task RestoreArtifactsAsync(
        ZipArchive archive,
        Type moduleType,
        CancellationToken cancellationToken)
    {
        var root = Path.GetFullPath(_options.WorkingDirectory);
        var artifactEntries = archive.Entries
            .Where(entry => entry.FullName.StartsWith(ArtifactPrefix, StringComparison.Ordinal))
            .Select(entry => (
                Entry: entry,
                Destination: GetArtifactDestination(root, entry),
                IsDirectory: entry.FullName.EndsWith('/'),
                IsSymbolicLink: IsUnixSymbolicLink(entry)))
            .ToArray();

        ClearArtifacts(moduleType, cancellationToken);

        foreach (var destination in artifactEntries
                     .Select(artifact => artifact.Destination)
                     .Distinct(PathComparer))
        {
            cancellationToken.ThrowIfCancellationRequested();
            RemoveLinkedDestinationComponents(root, destination);
        }

        foreach (var (_, destination, isDirectory, _) in artifactEntries)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (isDirectory)
            {
                Directory.CreateDirectory(destination);
            }
        }

        foreach (var (entry, destination, isDirectory, isSymbolicLink) in artifactEntries)
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
                await input.CopyToAsync(output, cancellationToken).ConfigureAwait(false);
            }

            RestoreUnixMode(entry, destination, UnixFileTypeRegular);
        }

        foreach (var (entry, destination, _, _) in artifactEntries
                     .Where(artifact => artifact.IsDirectory)
                     .OrderByDescending(artifact => artifact.Destination.Length))
        {
            RestoreUnixMode(entry, destination, UnixFileTypeDirectory);
        }

        foreach (var (entry, destination, _, _) in artifactEntries
                     .Where(artifact => artifact.IsSymbolicLink))
        {
            cancellationToken.ThrowIfCancellationRequested();
            Directory.CreateDirectory(Path.GetDirectoryName(destination)!);
            await using var input = entry.Open();
            using var reader = new StreamReader(
                input,
                Encoding.UTF8,
                detectEncodingFromByteOrderMarks: false,
                bufferSize: 1024,
                leaveOpen: false);
            var linkTarget = await reader.ReadToEndAsync(cancellationToken).ConfigureAwait(false);
            File.CreateSymbolicLink(destination, linkTarget);
        }
    }

    private void ClearArtifacts(Type moduleType, CancellationToken cancellationToken)
    {
        var artifactPaths = GetArtifactPaths(moduleType).ToArray();
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

        foreach (var file in files)
        {
            cancellationToken.ThrowIfCancellationRequested();
            File.Delete(file);
        }

        var root = Path.GetFullPath(_options.WorkingDirectory);
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

    private static bool IsUnixSymbolicLink(ZipArchiveEntry entry)
    {
        if (OperatingSystem.IsWindows())
        {
            return false;
        }

        var unixAttributes = (entry.ExternalAttributes >> 16) & 0xFFFF;
        return (unixAttributes & UnixFileTypeMask) == UnixFileTypeSymbolicLink;
    }

    private static void Append(IncrementalHash hash, string name, string value)
    {
        hash.AppendData(Encoding.UTF8.GetBytes(name));
        hash.AppendData([0]);
        hash.AppendData(Encoding.UTF8.GetBytes(value));
        hash.AppendData([0xFF]);
    }

    private static StringComparer PathComparer =>
        OperatingSystem.IsWindows() ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal;
}
