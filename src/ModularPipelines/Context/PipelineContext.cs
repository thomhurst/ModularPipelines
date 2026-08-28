using Microsoft.Extensions.Logging;
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
internal class PipelineContext : IPipelineContext, IInternalPipelineContext
{
    private readonly IInternalModuleLoggerAccessor _moduleLoggerAccessor;
    private readonly ModuleLookup _moduleLookup;

    /// <summary>
    /// Cached logger instance for this context.
    /// </summary>
    private IModuleLogger? _logger;

    /// <inheritdoc />
    public ILogger Logger => _logger ??= _moduleLoggerAccessor.GetLogger();

    /// <inheritdoc />
    public Domains.IShellContext Shell { get; }

    /// <inheritdoc />
    public IFilesContext Files { get; }

    /// <inheritdoc />
    public IDataContext Data { get; }

    /// <inheritdoc />
    public IEnvironmentContext Environment { get; }

    /// <inheritdoc />
    public IInstallersContext Installers { get; }

    /// <inheritdoc />
    public INetworkContext Network { get; }

    /// <inheritdoc />
    public ISecurityContext Security { get; }

    /// <inheritdoc />
    public IServicesContext Services { get; }

    /// <inheritdoc />
    public IToolsContext Tools { get; }

    /// <inheritdoc />
    public ISummaryLogger Summary { get; }

    // Internal properties for IInternalPipelineContext
    public IDependencyCollisionDetector DependencyCollisionDetector { get; }

    public IModuleResultRepository ModuleResultRepository { get; }

    public EngineCancellationToken EngineCancellationToken { get; }

    public void InitializeLogger(Type getType)
    {
        _logger = _moduleLoggerAccessor.GetLogger(getType);
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="PipelineContext"/> class.
    /// </summary>
    public PipelineContext(
        ModuleLookup moduleLookup,
        IDependencyCollisionDetector dependencyCollisionDetector,
        IModuleResultRepository moduleResultRepository,
        IInternalModuleLoggerAccessor moduleLoggerAccessor,
        EngineCancellationToken engineCancellationToken,
        Domains.IShellContext shell,
        IFilesContext files,
        IDataContext data,
        IEnvironmentContext environment,
        IInstallersContext installers,
        INetworkContext network,
        ISecurityContext security,
        IServicesContext services,
        ISummaryLogger summary)
    {
        _moduleLookup = moduleLookup;
        _moduleLoggerAccessor = moduleLoggerAccessor;
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
        Tools = new ToolsContext(services);
        Summary = summary;
    }

    public TModule? GetModule<TModule>()
        where TModule : class, IModule
    {
        return (TModule?) _moduleLookup.GetAssignable(typeof(TModule));
    }

    public IModule? GetModule(Type type)
    {
        return _moduleLookup.GetExact(type);
    }
}
