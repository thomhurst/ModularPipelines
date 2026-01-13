using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using ModularPipelines.Context.Domains;
using ModularPipelines.Engine;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Helpers;
using ModularPipelines.Http;
using ModularPipelines.Logging;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;

namespace ModularPipelines.Context;

/// <summary>
/// Implementation of <see cref="IModuleHookContext"/>.
/// </summary>
internal class ModuleHookContext : IModuleHookContext
{
    private readonly IPipelineHookContext _pipelineContext;
    private readonly IModuleMetadataRegistry _metadataRegistry;
    private readonly DateTimeOffset _startTime;

    public ModuleHookContext(
        IModule module,
        IReadOnlyList<Attribute> moduleAttributes,
        DateTimeOffset startTime,
        IModuleResult? result,
        IPipelineHookContext pipelineContext,
        IModuleMetadataRegistry metadataRegistry)
    {
        Module = module;
        ModuleAttributes = moduleAttributes;
        _startTime = startTime;
        Result = result;
        _pipelineContext = pipelineContext;
        _metadataRegistry = metadataRegistry;
    }

    #region IModuleHookContext members

    public IModule Module { get; }

    public Type ModuleType => Module.GetType();

    public string ModuleName => ModuleType.Name;

    public IReadOnlyList<Attribute> ModuleAttributes { get; }

    public DateTimeOffset StartTime => _startTime;

    public TimeSpan ElapsedTime => DateTimeOffset.UtcNow - _startTime;

    public IModuleResult? Result { get; }

    // Control flow state (internal for engine to read)
    public bool RetryRequested { get; private set; }

    public TimeSpan? RetryDelay { get; private set; }

    public bool ShouldSkipDependents { get; private set; }

    public string? SkipDependentsReason { get; private set; }

    public bool ShouldFailPipeline { get; private set; }

    public string? FailPipelineReason { get; private set; }

    public void RequestRetry(TimeSpan? delay = null)
    {
        RetryRequested = true;
        RetryDelay = delay;
    }

    public void SkipDependentModules(string reason)
    {
        ShouldSkipDependents = true;
        SkipDependentsReason = reason;
    }

    public void FailPipeline(string reason)
    {
        ShouldFailPipeline = true;
        FailPipelineReason = reason;
    }

    public T? GetMetadata<T>(string key)
        => _metadataRegistry.GetMetadata<T>(ModuleType, key);

    #endregion

    #region IPipelineHookContext delegation

    // IPipelineServices
    public IServiceProvider ServiceProvider => _pipelineContext.ServiceProvider;

    public T? Get<T>() => _pipelineContext.Get<T>();

    public IConfiguration Configuration => _pipelineContext.Configuration;

    public IOptions<PipelineOptions> PipelineOptions => _pipelineContext.PipelineOptions;

    // IPipelineLogging
    public IModuleLogger Logger => ((IPipelineLogging)_pipelineContext).Logger;

    // IPipelineTools
    public ICommand Command => _pipelineContext.Command;

    public IPowershell Powershell => _pipelineContext.Powershell;

    public IBash Bash => _pipelineContext.Bash;

    public IHttp Http => _pipelineContext.Http;

    public IDownloader Downloader => _pipelineContext.Downloader;

    public IInstaller Installer => _pipelineContext.Installer;

    // IPipelineEncoding
    public IJson Json => _pipelineContext.Json;

    public IXml Xml => _pipelineContext.Xml;

    public IYaml Yaml => _pipelineContext.Yaml;

    public IHex Hex => _pipelineContext.Hex;

    public IBase64 Base64 => _pipelineContext.Base64;

    public IHasher Hasher => _pipelineContext.Hasher;

    /// <inheritdoc />
    public ModularPipelines.Context.Domains.IShellContext Shell => _pipelineContext.Shell;

    /// <inheritdoc />
    public IFilesContext Files => _pipelineContext.Files;

    /// <inheritdoc />
    public IDataContext Data => _pipelineContext.Data;

    /// <inheritdoc />
    /// <remarks>Stub implementation - will be fully implemented in Task 3.4.</remarks>
    IEnvironmentDomainContext IPipelineContext.Environment => throw new NotImplementedException("EnvironmentDomainContext implementation pending (Task 3.4)");

    /// <inheritdoc />
    /// <remarks>Stub implementation - will be fully implemented in Task 3.5.</remarks>
    public IInstallersContext Installers => throw new NotImplementedException("InstallersContext implementation pending (Task 3.5)");

    /// <inheritdoc />
    /// <remarks>Stub implementation - will be fully implemented in Task 3.6.</remarks>
    public INetworkContext Network => throw new NotImplementedException("NetworkContext implementation pending (Task 3.6)");

    /// <inheritdoc />
    /// <remarks>Stub implementation - will be fully implemented in Task 3.7.</remarks>
    public ISecurityContext Security => throw new NotImplementedException("SecurityContext implementation pending (Task 3.7)");

    /// <inheritdoc />
    /// <remarks>Stub implementation - will be fully implemented in Task 3.8.</remarks>
    public IServicesContext Services => throw new NotImplementedException("ServicesContext implementation pending (Task 3.8)");

    // IPipelineFileSystem
    public IFileSystemContext FileSystem => _pipelineContext.FileSystem;

    public IZip Zip => _pipelineContext.Zip;

    public IChecksum Checksum => _pipelineContext.Checksum;

    // IPipelineEnvironment
    public IEnvironmentContext Environment => ((IPipelineEnvironment)_pipelineContext).Environment;

    public IBuildSystemDetector BuildSystemDetector => _pipelineContext.BuildSystemDetector;

    #endregion
}
