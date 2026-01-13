using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Context.Domains;
using ModularPipelines.Engine;
using ModularPipelines.Helpers;
using ModularPipelines.Logging;
using ModularPipelines.Modules;

namespace ModularPipelines.Context;

/// <summary>
/// Provides context and services for module execution.
/// </summary>
/// <remarks>
/// This class is registered as Scoped in the DI container, meaning each module execution
/// gets its own instance. This ensures proper isolation between concurrent module executions.
/// </remarks>
internal class PipelineContext : IPipelineHookContext, IInternalPipelineContext
{
    private readonly IInternalModuleLoggerProvider _moduleLoggerProvider;
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Cached logger instance for this context.
    /// </summary>
    private IModuleLogger? _logger;

    /// <inheritdoc />
    public IModuleLogger Logger => _logger ??= _moduleLoggerProvider.GetLogger();

    /// <inheritdoc />
    public Domains.IShellContext Shell { get; }

    /// <inheritdoc />
    public IFilesContext Files { get; }

    /// <inheritdoc />
    public IDataContext Data { get; }

    /// <inheritdoc />
    public IEnvironmentDomainContext Environment { get; }

    /// <inheritdoc />
    public IInstallersContext Installers { get; }

    /// <inheritdoc />
    public INetworkContext Network { get; }

    /// <inheritdoc />
    public ISecurityContext Security { get; }

    /// <inheritdoc />
    public IServicesContext Services { get; }

    // Internal properties for IInternalPipelineContext
    public IDependencyCollisionDetector DependencyCollisionDetector { get; }

    public IModuleResultRepository ModuleResultRepository { get; }

    public EngineCancellationToken EngineCancellationToken { get; }

    public void InitializeLogger(Type getType)
    {
        _logger = _moduleLoggerProvider.GetLogger(getType);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PipelineContext"/> class.
    /// </summary>
    public PipelineContext(
        IServiceProvider serviceProvider,
        IDependencyCollisionDetector dependencyCollisionDetector,
        IModuleResultRepository moduleResultRepository,
        IInternalModuleLoggerProvider moduleLoggerProvider,
        EngineCancellationToken engineCancellationToken,
        Domains.IShellContext shell,
        IFilesContext files,
        IDataContext data,
        IEnvironmentDomainContext environment,
        IInstallersContext installers,
        INetworkContext network,
        ISecurityContext security,
        IServicesContext services)
    {
        _serviceProvider = serviceProvider;
        _moduleLoggerProvider = moduleLoggerProvider;
        DependencyCollisionDetector = dependencyCollisionDetector;
        ModuleResultRepository = moduleResultRepository;
        EngineCancellationToken = engineCancellationToken;

        // Domain contexts (v2.0)
        Shell = shell;
        Files = files;
        Data = data;
        Environment = environment;
        Installers = installers;
        Network = network;
        Security = security;
        Services = services;
    }

    public TModule? GetModule<TModule>()
        where TModule : class, IModule
    {
        return _serviceProvider.GetServices<IModule>().OfType<TModule>().SingleOrDefault();
    }

    public IModule? GetModule(Type type)
    {
        return _serviceProvider.GetServices<IModule>().SingleOrDefault(module => module.GetType() == type);
    }
}
