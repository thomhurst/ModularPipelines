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

    public bool IsEnabled => true;

    [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "Module result cache requires runtime result type metadata.")]
    [UnconditionalSuppressMessage("AOT", "IL3050", Justification = "Module result cache requires runtime result type metadata.")]
    public async Task SaveResultAsync<T>(
        Module<T> module,
        ModuleResult<T> moduleResult,
        IPipelineContext pipelineContext)
    {
        if (!((IModule) module).Configuration.CacheEnabled || moduleResult.ModuleStatus != Status.Successful)
        {
            return;
        }

        if (!_fingerprints.TryGetValue(module, out var fingerprint))
        {
            fingerprint = await ComputeFingerprintAsync(module, pipelineContext, CancellationToken.None)
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
                                cancellationToken: CancellationToken.None)
                            .ConfigureAwait(false);
                    }

                    AddArtifacts(archive, module.GetType());
                }

                stream.Position = 0;
                await _store.WriteAsync(fingerprint, stream, CancellationToken.None).ConfigureAwait(false);
            }

            _logger.LogDebug(
                "Saved module cache entry {Fingerprint} for {Module}",
                fingerprint,
                module.GetType().Name);
        }
        finally
        {
            _fingerprints.TryRemove(module, out _);
            File.Delete(temporary);
        }
    }

    [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "Module result cache requires runtime result type metadata.")]
    [UnconditionalSuppressMessage("AOT", "IL3050", Justification = "Module result cache requires runtime result type metadata.")]
    public async Task<ModuleResult<T>?> GetResultAsync<T>(
        Module<T> module,
        IPipelineContext pipelineContext)
    {
        if (!((IModule) module).Configuration.CacheEnabled)
        {
            return null;
        }

        var fingerprint = await ComputeFingerprintAsync(module, pipelineContext, CancellationToken.None)
            .ConfigureAwait(false);
        _fingerprints[module] = fingerprint;
        await using var cachedStream = await _store.OpenReadAsync(fingerprint, CancellationToken.None)
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
                await cachedStream.CopyToAsync(output).ConfigureAwait(false);
            }

            using var archive = ZipFile.OpenRead(temporary);
            var resultEntry = archive.GetEntry(ResultEntryName)
                              ?? throw new InvalidDataException("Module cache entry does not contain result.json.");
            ModuleResult<T>? result;
            await using (var resultStream = resultEntry.Open())
            {
                result = await JsonSerializer.DeserializeAsync<ModuleResult<T>>(resultStream)
                    .ConfigureAwait(false);
            }

            if (result is null)
            {
                throw new InvalidDataException("Module cache result is empty.");
            }

            RestoreArtifacts(archive);
            _fingerprints.TryRemove(module, out _);
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
            _options.MaximumInputFiles);
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

            var dependencyResult = await dependencyModule.ResultTask.ConfigureAwait(false);
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

    private void AddArtifacts(ZipArchive archive, Type moduleType)
    {
        var paths = moduleType
            .GetCustomAttributes(typeof(ProducesArtifactAttribute), inherit: true)
            .Cast<ProducesArtifactAttribute>()
            .Select(attribute => attribute.PathPattern);
        var files = ModuleCacheFileResolver.ResolveFiles(
            _options.WorkingDirectory,
            paths,
            _options.MaximumInputFiles);

        foreach (var file in files)
        {
            var relativePath = ModuleCacheFileResolver.GetRelativePath(_options.WorkingDirectory, file);
            archive.CreateEntryFromFile(file, $"{ArtifactPrefix}{relativePath}", CompressionLevel.Fastest);
        }
    }

    private void RestoreArtifacts(ZipArchive archive)
    {
        var root = Path.GetFullPath(_options.WorkingDirectory);
        foreach (var entry in archive.Entries.Where(entry => entry.FullName.StartsWith(ArtifactPrefix, StringComparison.Ordinal)))
        {
            var relativePath = entry.FullName[ArtifactPrefix.Length..].Replace('/', Path.DirectorySeparatorChar);
            var destination = Path.GetFullPath(Path.Combine(root, relativePath));
            var verifiedRelativePath = Path.GetRelativePath(root, destination);
            if (verifiedRelativePath == ".."
                || verifiedRelativePath.StartsWith($"..{Path.DirectorySeparatorChar}", StringComparison.Ordinal)
                || Path.IsPathRooted(verifiedRelativePath))
            {
                throw new InvalidDataException($"Cache artifact path '{entry.FullName}' escapes the working directory.");
            }

            Directory.CreateDirectory(Path.GetDirectoryName(destination)!);
            entry.ExtractToFile(destination, overwrite: true);
        }
    }

    private static void Append(IncrementalHash hash, string name, string value)
    {
        hash.AppendData(Encoding.UTF8.GetBytes(name));
        hash.AppendData([0]);
        hash.AppendData(Encoding.UTF8.GetBytes(value));
        hash.AppendData([0xFF]);
    }
}
