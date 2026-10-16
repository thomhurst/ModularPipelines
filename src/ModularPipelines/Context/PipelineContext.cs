using Microsoft.Extensions.Logging;
using ModularPipelines.Distributed;
using ModularPipelines.Engine;
using ModularPipelines.Helpers;
using ModularPipelines.Logging;
using ModularPipelines.Modules;
using ModularPipelines.Reporting;

namespace ModularPipelines.Context;

/// <summary>
/// Provides context and services for module execution.
/// </summary>
/// <remarks>
/// This class is registered as Scoped in the DI container, meaning each module execution
/// gets its own instance. This ensures proper isolation between concurrent module executions.
/// </remarks>
internal class PipelineContext(
    ModuleLookup moduleLookup,
    IDependencyCollisionDetector dependencyCollisionDetector,
    IModuleResultRepository moduleResultRepository,
    IInternalModuleLoggerAccessor moduleLoggerAccessor,
    IConsoleWriter consoleWriter,
    EngineCancellationToken engineCancellationToken,
    IShellContext shell,
    IFilesContext files,
    IDataContext data,
    IEnvironmentContext environment,
    IInstallersContext installers,
    INetworkContext network,
    ISecurityContext security,
    IServicesContext services,
    IArtifactContext artifacts,
    ISummaryLogger summary) : IPipelineContext, IInternalPipelineContext
{
    private readonly IInternalModuleLoggerAccessor _moduleLoggerAccessor = moduleLoggerAccessor;
    private readonly ModuleLookup _moduleLookup = moduleLookup;

    /// <summary>
    /// Logger explicitly bound to a module. Ambient loggers must be resolved for the current phase.
    /// </summary>
    private IModuleLogger? _logger;

    /// <inheritdoc />
    public ILogger Logger => _logger ?? _moduleLoggerAccessor.GetLogger();

    /// <inheritdoc />
    public IConsoleWriter Console { get; } = consoleWriter;

    /// <inheritdoc />
    public IShellContext Shell { get; } = shell;

    /// <inheritdoc />
    public IFilesContext Files { get; } = files;

    /// <inheritdoc />
    public IDataContext Data { get; } = data;

    /// <inheritdoc />
    public IEnvironmentContext Environment { get; } = environment;

    /// <inheritdoc />
    public IInstallersContext Installers { get; } = installers;

    /// <inheritdoc />
    public INetworkContext Network { get; } = network;

    /// <inheritdoc />
    public ISecurityContext Security { get; } = security;

    /// <inheritdoc />
    public IServicesContext Services { get; } = services;

    /// <inheritdoc />
    public IToolsContext Tools { get; } = new ToolsContext(services);

    /// <inheritdoc />
    public IArtifactContext Artifacts { get; } = artifacts;

    /// <inheritdoc />
    public ISummaryLogger Summary { get; } = summary;

    // Internal properties for IInternalPipelineContext
    public IDependencyCollisionDetector DependencyCollisionDetector { get; } = dependencyCollisionDetector;

    public IModuleResultRepository ModuleResultRepository { get; } = moduleResultRepository;

    public EngineCancellationToken EngineCancellationToken { get; } = engineCancellationToken;

    public void InitializeLogger(Type getType)
    {
        _logger = _moduleLoggerAccessor.GetLogger(getType);
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
