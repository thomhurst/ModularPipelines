using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Context.Domains;
using ModularPipelines.Engine;
using ModularPipelines.Exceptions;
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
    private readonly IInternalModuleLoggerProvider _moduleLoggerProvider;
    private readonly Lazy<ModuleLookup> _moduleLookup;

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

    /// <inheritdoc />
    public ISummaryLogger Summary { get; }

    // Internal properties for IInternalPipelineContext
    public IDependencyCollisionDetector DependencyCollisionDetector { get; }

    public IModuleResultRepository ModuleResultRepository { get; }

    public EngineCancellationToken EngineCancellationToken { get; }

    public void InitializeLogger(Type getType)
    {
        _logger = _moduleLoggerProvider.GetLogger(getType);
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="PipelineContext"/> class.
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
        IServicesContext services,
        ISummaryLogger summary)
    {
        _moduleLookup = new Lazy<ModuleLookup>(
            () => ModuleLookup.Create(serviceProvider.GetServices<IModule>()),
            LazyThreadSafetyMode.ExecutionAndPublication);
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
        Summary = summary;
    }

    public TModule? GetModule<TModule>()
        where TModule : class, IModule
    {
        return (TModule?) _moduleLookup.Value.GetAssignable(typeof(TModule));
    }

    public IModule? GetModule(Type type)
    {
        return _moduleLookup.Value.GetExact(type);
    }

    private sealed class ModuleLookup
    {
        private readonly ConcurrentDictionary<Type, IReadOnlyList<IModule>> _assignableModules = new();
        private readonly IReadOnlyDictionary<Type, IReadOnlyList<IModule>> _ambiguousConcreteModules;
        private readonly IReadOnlyDictionary<Type, IModule> _concreteModules;
        private readonly IReadOnlyList<IModule> _modules;

        private ModuleLookup(
            IReadOnlyList<IModule> modules,
            IReadOnlyDictionary<Type, IModule> concreteModules,
            IReadOnlyDictionary<Type, IReadOnlyList<IModule>> ambiguousConcreteModules)
        {
            _modules = modules;
            _concreteModules = concreteModules;
            _ambiguousConcreteModules = ambiguousConcreteModules;
        }

        public static ModuleLookup Create(IEnumerable<IModule> registeredModules)
        {
            var modules = registeredModules
                .Distinct<IModule>(ReferenceEqualityComparer.Instance)
                .ToArray();

            var modulesByConcreteType = modules
                .GroupBy(x => x.GetType())
                .ToDictionary(
                    x => x.Key,
                    x => (IReadOnlyList<IModule>) x.ToArray());

            return new ModuleLookup(
                modules,
                modulesByConcreteType
                    .Where(x => x.Value.Count == 1)
                    .ToDictionary(x => x.Key, x => x.Value[0]),
                modulesByConcreteType
                    .Where(x => x.Value.Count > 1)
                    .ToDictionary(x => x.Key, x => x.Value));
        }

        public IModule? GetAssignable(Type type)
        {
            if (_concreteModules.ContainsKey(type) || _ambiguousConcreteModules.ContainsKey(type))
            {
                return GetExact(type);
            }

            var matches = _assignableModules.GetOrAdd(
                type,
                requestedType => _modules.Where(requestedType.IsInstanceOfType).ToArray());

            return GetSingleMatch(type, matches);
        }

        public IModule? GetExact(Type type)
        {
            ThrowIfAmbiguous(type, _ambiguousConcreteModules);
            return _concreteModules.GetValueOrDefault(type);
        }

        private static IModule? GetSingleMatch(Type requestedType, IReadOnlyList<IModule> matches)
        {
            if (matches.Count > 1)
            {
                throw new AmbiguousModuleException(
                    requestedType,
                    matches.Select(x => x.GetType()).ToArray());
            }

            return matches.Count == 1 ? matches[0] : null;
        }

        private static void ThrowIfAmbiguous(
            Type requestedType,
            IReadOnlyDictionary<Type, IReadOnlyList<IModule>> ambiguousModules)
        {
            if (ambiguousModules.TryGetValue(requestedType, out var matches))
            {
                throw new AmbiguousModuleException(
                    requestedType,
                    matches.Select(x => x.GetType()).ToArray());
            }
        }
    }
}
