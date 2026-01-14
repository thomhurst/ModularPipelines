using ModularPipelines.Context.Domains;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Logging;
using ModularPipelines.Models;
using ModularPipelines.Modules;

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

    #region IPipelineContext delegation (domain-based)

    /// <inheritdoc />
    public IModuleLogger Logger => _pipelineContext.Logger;

    /// <inheritdoc />
    public Domains.IShellContext Shell => _pipelineContext.Shell;

    /// <inheritdoc />
    public IFilesContext Files => _pipelineContext.Files;

    /// <inheritdoc />
    public IDataContext Data => _pipelineContext.Data;

    /// <inheritdoc />
    public IEnvironmentDomainContext Environment => _pipelineContext.Environment;

    /// <inheritdoc />
    public IInstallersContext Installers => _pipelineContext.Installers;

    /// <inheritdoc />
    public INetworkContext Network => _pipelineContext.Network;

    /// <inheritdoc />
    public ISecurityContext Security => _pipelineContext.Security;

    /// <inheritdoc />
    public IServicesContext Services => _pipelineContext.Services;

    #endregion
}
