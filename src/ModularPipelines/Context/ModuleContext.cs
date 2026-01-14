using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Context.Domains;
using ModularPipelines.Engine;
using ModularPipelines.Exceptions;
using ModularPipelines.Helpers;
using ModularPipelines.Logging;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Context;

/// <summary>
/// Module-specific context that wraps the pipeline context and adds module-specific capabilities.
/// </summary>
internal class ModuleContext : IModuleContext, IInternalPipelineContext
{
    private readonly IPipelineHookContext _pipelineContext;
    private readonly IInternalPipelineContext _internalContext;
    private readonly IModule _currentModule;
    private readonly ModuleExecutionContext _executionContext;
    private readonly IModuleLogger _logger;

    public ModuleContext(
        IPipelineHookContext pipelineContext,
        IModule currentModule,
        ModuleExecutionContext executionContext,
        IModuleLogger logger)
    {
        _pipelineContext = pipelineContext;
        _internalContext = (IInternalPipelineContext)pipelineContext;
        _currentModule = currentModule;
        _executionContext = executionContext;
        _logger = logger;
    }

    #region IModuleContext specific methods

    public ModuleResult<TResult> GetModule<TModule, TResult>()
        where TModule : Module<TResult>
    {
        var moduleType = typeof(TModule);

        // Check for self-reference
        if (moduleType == _currentModule.GetType())
        {
            throw new ModuleReferencingSelfException(
                $"Module '{moduleType.Name}' cannot reference itself. " +
                "A module cannot depend on its own result.");
        }

        var result = GetModuleIfRegistered<TModule, TResult>();

        if (result == null)
        {
            throw new ModuleNotRegisteredException(
                $"Module '{moduleType.Name}' is not registered in the pipeline.\n" +
                $"Add '.AddModule<{moduleType.Name}>()' to your pipeline configuration.",
                null);
        }

        return result;
    }

    public ModuleResult<TResult>? GetModuleIfRegistered<TModule, TResult>()
        where TModule : Module<TResult>
    {
        var moduleType = typeof(TModule);

        // Find it via the module registry
        var registry = Services.Get<IModuleResultRegistry>();
        if (registry != null)
        {
            return registry.GetResult<TResult>(moduleType);
        }

        return null;
    }

    public async Task<T> SubModule<T>(string name, Func<Task<T>> action)
    {
        var tracker = new SubModuleTracker(name, _currentModule.GetType());
        _executionContext.SubModules.Add(tracker);
        return await tracker.ExecuteAsync(action).ConfigureAwait(false);
    }

    public async Task SubModule(string name, Func<Task> action)
    {
        var tracker = new SubModuleTracker(name, _currentModule.GetType());
        _executionContext.SubModules.Add(tracker);
        await tracker.ExecuteAsync(action).ConfigureAwait(false);
    }

    #endregion

    #region IPipelineContext delegation (domain-based)

    /// <inheritdoc />
    public IModuleLogger Logger => _logger;

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

    #region IInternalPipelineContext delegation

    IDependencyCollisionDetector IInternalPipelineContext.DependencyCollisionDetector =>
        _internalContext.DependencyCollisionDetector;

    IModuleResultRepository IInternalPipelineContext.ModuleResultRepository =>
        _internalContext.ModuleResultRepository;

    void IInternalPipelineContext.InitializeLogger(Type getType)
    {
        // Logger is already initialized in constructor
    }

    EngineCancellationToken IInternalPipelineContext.EngineCancellationToken =>
        _internalContext.EngineCancellationToken;

    TModule? IInternalPipelineContext.GetModule<TModule>()
        where TModule : class
    {
        var moduleType = typeof(TModule);

        // Check for self-reference
        if (moduleType == _currentModule.GetType())
        {
            throw new ModuleReferencingSelfException(
                $"Module '{moduleType.Name}' cannot reference itself. " +
                "A module cannot depend on its own result.");
        }

        return _internalContext.GetModule<TModule>();
    }

    IModule? IInternalPipelineContext.GetModule(Type type)
    {
        return _internalContext.GetModule(type);
    }

    #endregion
}
