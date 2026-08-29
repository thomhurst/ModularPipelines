using Mediator;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModularPipelines.Context.Domains;
using ModularPipelines.Engine;
using ModularPipelines.Enums;
using ModularPipelines.Events;
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
    private static readonly TimeSpan DefaultSubModuleEstimatedDuration = TimeSpan.FromMinutes(2);

    private readonly IPipelineContext _pipelineContext;
    private readonly IInternalPipelineContext _internalContext;
    private readonly IModule _currentModule;
    private readonly ModuleExecutionContext _executionContext;
    private readonly IModuleLogger _logger;
    private readonly IMediator _mediator;
    private readonly ISafeModuleEstimatedTimeProvider _estimatedTimeProvider;
    private readonly bool _moduleResultAccessAllowed;
    private readonly Lazy<Task<SubModuleEstimation[]>> _subModuleEstimations;

    public ModuleContext(
        IPipelineContext pipelineContext,
        IModule currentModule,
        ModuleExecutionContext executionContext,
        IModuleLogger logger,
        IMediator mediator,
        ISafeModuleEstimatedTimeProvider estimatedTimeProvider,
        bool moduleResultAccessAllowed = true)
    {
        _pipelineContext = pipelineContext;
        _internalContext = (IInternalPipelineContext) pipelineContext;
        _currentModule = currentModule;
        _executionContext = executionContext;
        _logger = logger;
        _mediator = mediator;
        _estimatedTimeProvider = estimatedTimeProvider;
        _moduleResultAccessAllowed = moduleResultAccessAllowed;
        _subModuleEstimations = new Lazy<Task<SubModuleEstimation[]>>(LoadSubModuleEstimationsAsync);
    }

    #region IModuleContext specific methods

    public TModule GetModule<TModule>()
        where TModule : class, IModule
    {
        var moduleType = typeof(TModule);
        EnsureModuleResultAccessAllowed(moduleType);

        // Check for self-reference
        if (moduleType == _currentModule.GetType())
        {
            throw new ModuleSelfDependencyException(
                $"Module '{moduleType.Name}' cannot reference itself. " +
                "A module cannot depend on its own result.");
        }

        var module = _internalContext.GetModule<TModule>();

        if (module == null)
        {
            throw new ModuleNotRegisteredException(
                $"Module '{moduleType.Name}' is not registered in the pipeline.\n" +
                $"Add '.AddModule<{moduleType.Name}>()' to your pipeline configuration.",
                null);
        }

        return module;
    }

    public TModule? GetModuleIfRegistered<TModule>()
        where TModule : class, IModule
    {
        var moduleType = typeof(TModule);

        // Check for self-reference - still throw, as this is a programming error
        if (moduleType == _currentModule.GetType())
        {
            throw new ModuleSelfDependencyException(
                $"Module '{moduleType.Name}' cannot reference itself. " +
                "A module cannot depend on its own result.");
        }

        EnsureModuleResultAccessAllowed(moduleType);
        return _internalContext.GetModule<TModule>();
    }

    private void EnsureModuleResultAccessAllowed(Type moduleType)
    {
        if (!_moduleResultAccessAllowed)
        {
            throw new PlanningModuleResultUnavailableException(moduleType);
        }
    }

    public Task<T> RunSubModuleAsync<T>(
        string name,
        Func<CancellationToken, Task<T>> body,
        CancellationToken cancellationToken = default) =>
        ExecuteSubModuleAsync(name, body, cancellationToken);

    public Task RunSubModuleAsync(
        string name,
        Func<CancellationToken, Task> body,
        CancellationToken cancellationToken = default) =>
        ExecuteSubModuleAsync(name, async token =>
        {
            await body(token).ConfigureAwait(false);
            return 0;
        }, cancellationToken);

    private async Task<T> ExecuteSubModuleAsync<T>(
        string name,
        Func<CancellationToken, Task<T>> body,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var moduleType = _currentModule.GetType();
        var tracker = new SubModuleTracker(name, moduleType);
        var estimates = await _subModuleEstimations.Value.ConfigureAwait(false);
        var estimatedDuration = estimates
            .FirstOrDefault(x => string.Equals(x.SubModuleName, name, StringComparison.Ordinal))
            ?.EstimatedDuration ?? DefaultSubModuleEstimatedDuration;

        await _mediator.Publish(
            new SubModuleCreatedNotification(_currentModule, tracker, estimatedDuration),
            cancellationToken)
            .ConfigureAwait(false);

        try
        {
            return await tracker.ExecuteAsync(() => body(cancellationToken)).ConfigureAwait(false);
        }
        finally
        {
            var isSuccessful = tracker.Status == ModuleStatus.Succeeded;
            if (isSuccessful)
            {
                await _estimatedTimeProvider.SaveSubModuleTimeAsync(
                    moduleType,
                    new SubModuleEstimation(name, tracker.Duration)).ConfigureAwait(false);
            }

            await _mediator.Publish(
                new SubModuleCompletedNotification(_currentModule, tracker, isSuccessful),
                cancellationToken)
                .ConfigureAwait(false);
        }
    }

    private async Task<SubModuleEstimation[]> LoadSubModuleEstimationsAsync()
    {
        var estimates = await _estimatedTimeProvider
            .GetSubModuleEstimatedTimesAsync(_currentModule.GetType())
            .ConfigureAwait(false);

        return estimates.ToArray();
    }

    #endregion

    #region IPipelineContext delegation (domain-based)

    /// <inheritdoc />
    public ILogger Logger => _logger;

    /// <inheritdoc />
    public Domains.IShellContext Shell => _pipelineContext.Shell;

    /// <inheritdoc />
    public IFilesContext Files => _pipelineContext.Files;

    /// <inheritdoc />
    public IDataContext Data => _pipelineContext.Data;

    /// <inheritdoc />
    public IEnvironmentContext Environment => _pipelineContext.Environment;

    /// <inheritdoc />
    public IInstallersContext Installers => _pipelineContext.Installers;

    /// <inheritdoc />
    public INetworkContext Network => _pipelineContext.Network;

    /// <inheritdoc />
    public ISecurityContext Security => _pipelineContext.Security;

    /// <inheritdoc />
    public IServicesContext Services => _pipelineContext.Services;

    /// <inheritdoc />
    public IToolsContext Tools => _pipelineContext.Tools;

    /// <inheritdoc />
    public ISummaryLogger Summary => _pipelineContext.Summary;

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
            throw new ModuleSelfDependencyException(
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
