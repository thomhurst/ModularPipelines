using System.Runtime.CompilerServices;
using EnumerableAsyncProcessor.Extensions;
using Microsoft.Extensions.Options;
using ModularPipelines.Engine.Attributes;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Exceptions;
using ModularPipelines.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;

namespace ModularPipelines.Engine;

internal class ModuleRetriever
{
    private readonly IModuleConditionHandler _moduleConditionHandler;
    private readonly IRegistrationEventExecutor _registrationEventExecutor;
    private readonly ISafeModuleEstimatedTimeProvider _estimatedTimeProvider;
    private readonly IModuleDependencyRegistry _dependencyRegistry;
    private readonly IModuleMetadataRegistry _metadataRegistry;
    private readonly IDependencyChainProvider _dependencyChainProvider;
    private readonly IOptions<PipelineOptions> _pipelineOptions;
    private readonly List<IModule> _modules;
    private Task<OrganizedModules>? _cached;

    internal IReadOnlyList<IModule> RegisteredModules => _modules;

    public ModuleRetriever(
        IModuleConditionHandler moduleConditionHandler,
        IRegistrationEventExecutor registrationEventExecutor,
        IEnumerable<IModule> modules,
        ISafeModuleEstimatedTimeProvider estimatedTimeProvider,
        IModuleDependencyRegistry dependencyRegistry,
        IModuleMetadataRegistry metadataRegistry,
        IDependencyChainProvider dependencyChainProvider,
        IOptions<PipelineOptions> pipelineOptions
    )
    {
        _moduleConditionHandler = moduleConditionHandler;
        _registrationEventExecutor = registrationEventExecutor;
        _estimatedTimeProvider = estimatedTimeProvider;
        _dependencyRegistry = dependencyRegistry;
        _metadataRegistry = metadataRegistry;
        _dependencyChainProvider = dependencyChainProvider;
        _pipelineOptions = pipelineOptions;

        // Defend against direct service registrations that repeat one module instance.
        // The module object still represents one execution.
        _modules = modules
            .Distinct<IModule>(ReferenceEqualityComparer.Instance)
            .ToList();
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public Task<OrganizedModules> GetOrganizedModules(CancellationToken cancellationToken = default)
    {
        return _cached ??= GetInternal(cancellationToken);
    }

    internal async Task<IReadOnlyList<IModule>> GetRunnableModulesForValidation(
        CancellationToken cancellationToken = default)
    {
        return (await DiscoverModules(cancellationToken, cascadeSkippedDependencies: true).ConfigureAwait(false)).RunnableModules;
    }

    internal async Task<(IReadOnlyList<IModule> RunnableModules, IReadOnlyList<IgnoredModule> IgnoredModules)>
        GetUncascadedModulesForValidation(CancellationToken cancellationToken = default)
    {
        var modules = await DiscoverModules(cancellationToken, cascadeSkippedDependencies: false)
            .ConfigureAwait(false);
        return (modules.RunnableModules, modules.IgnoredModules);
    }

    internal async Task ValidateSelectionAsync()
    {
        await _registrationEventExecutor.InvokeRegistrationEventsAsync(_modules).ConfigureAwait(false);
        _ = ModuleSelection.Create(
            _modules,
            _dependencyChainProvider,
            _pipelineOptions.Value);
    }

    private async Task<DiscoveredModules> DiscoverModules(
        CancellationToken cancellationToken,
        bool cascadeSkippedDependencies)
    {
        if (_modules.Count == 0)
        {
            throw new PipelineException("No modules have been registered");
        }

        // Dynamic dependencies and metadata must be registered before conditions and cascade skipping are evaluated.
        await _registrationEventExecutor.InvokeRegistrationEventsAsync(_modules).ConfigureAwait(false);
        var moduleSelection = ModuleSelection.Create(
            _modules,
            _dependencyChainProvider,
            _pipelineOptions.Value);

        var modulesToIgnore = new List<IgnoredModule>();
        var modulesToProcess = new List<IModule>();

        foreach (var module in _modules)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (moduleSelection.GetSkipDecision(module) is { } selectionSkipDecision)
            {
                modulesToIgnore.Add(new IgnoredModule(module, selectionSkipDecision));
                continue;
            }

            var (shouldIgnore, skipDecision) = await _moduleConditionHandler
                .ShouldIgnoreByCategory(module, cancellationToken)
                .ConfigureAwait(false);
            if (shouldIgnore)
            {
                modulesToIgnore.Add(new IgnoredModule(module, skipDecision ?? SkipDecision.Skip("Module was ignored")));
            }
            else
            {
                modulesToProcess.Add(module);
            }
        }

        if (cascadeSkippedDependencies)
        {
            var cascadeResult = await DependencySkipCascade.ApplyAsync(
                _modules,
                modulesToProcess,
                modulesToIgnore,
                _dependencyRegistry,
                _metadataRegistry,
                _ => Task.CompletedTask,
                _ => true,
                cancellationToken).ConfigureAwait(false);
            modulesToProcess = cascadeResult.RunnableModules.ToList();
            modulesToIgnore = cascadeResult.IgnoredModules.ToList();
        }

        return new DiscoveredModules(modulesToProcess, modulesToIgnore);
    }

    private async Task<OrganizedModules> GetInternal(CancellationToken cancellationToken)
    {
        var discoveredModules = await DiscoverModules(
            cancellationToken,
            cascadeSkippedDependencies: false).ConfigureAwait(false);
        var runnableModulesWithEstimatatedDuration = await discoveredModules.RunnableModules.ToAsyncProcessorBuilder()
            .SelectAsync(async module =>
            {
                var estimatedTime = await _estimatedTimeProvider.GetModuleEstimatedTimeAsync(module.GetType());

                return new RunnableModule(module, estimatedTime);
            })
            .ProcessInParallel();

        return new OrganizedModules(
            RunnableModules: runnableModulesWithEstimatatedDuration,
            IgnoredModules: discoveredModules.IgnoredModules
        );
    }

    private sealed record DiscoveredModules(
        IReadOnlyList<IModule> RunnableModules,
        IReadOnlyList<IgnoredModule> IgnoredModules);
}
