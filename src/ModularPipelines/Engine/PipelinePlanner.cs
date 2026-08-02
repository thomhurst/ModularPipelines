using Mediator;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ModularPipelines.Configuration;
using ModularPipelines.Context;
using ModularPipelines.Engine.Attributes;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Engine.Execution;
using ModularPipelines.Exceptions;
using ModularPipelines.Logging;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;

namespace ModularPipelines.Engine;

internal sealed class PipelinePlanner
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IReadOnlyList<IModule> _modules;
    private readonly IRegistrationEventExecutor _registrationEventExecutor;
    private readonly IModuleConditionHandler _conditionHandler;
    private readonly ISafeModuleEstimatedTimeProvider _estimatedTimeProvider;
    private readonly IModuleDependencyRegistry _dependencyRegistry;
    private readonly IModuleMetadataRegistry _metadataRegistry;
    private readonly IDependencyChainProvider _dependencyChainProvider;
    private readonly IOptions<PipelineOptions> _options;
    private readonly IMediator _mediator;

    public PipelinePlanner(
        IServiceProvider serviceProvider,
        IEnumerable<IModule> modules,
        IRegistrationEventExecutor registrationEventExecutor,
        IModuleConditionHandler conditionHandler,
        ISafeModuleEstimatedTimeProvider estimatedTimeProvider,
        IModuleDependencyRegistry dependencyRegistry,
        IModuleMetadataRegistry metadataRegistry,
        IDependencyChainProvider dependencyChainProvider,
        IOptions<PipelineOptions> options,
        IMediator mediator)
    {
        _serviceProvider = serviceProvider;
        _modules = modules.Distinct<IModule>(ReferenceEqualityComparer.Instance).ToArray();
        _registrationEventExecutor = registrationEventExecutor;
        _conditionHandler = conditionHandler;
        _estimatedTimeProvider = estimatedTimeProvider;
        _dependencyRegistry = dependencyRegistry;
        _metadataRegistry = metadataRegistry;
        _dependencyChainProvider = dependencyChainProvider;
        _options = options;
        _mediator = mediator;
    }

    public async Task<PipelinePlan> CreateAsync(CancellationToken cancellationToken = default)
    {
        if (_modules.Count == 0)
        {
            throw new PipelineException("No modules have been registered");
        }

        await _registrationEventExecutor.InvokeRegistrationEventsAsync(_modules).ConfigureAwait(false);
        cancellationToken.ThrowIfCancellationRequested();
        ModuleDependencyValidator.Validate(_modules, _dependencyRegistry, _metadataRegistry);

        var selection = ModuleSelection.Create(_modules, _dependencyChainProvider, _options.Value);
        var runnableModules = new List<IModule>();
        var ignoredModules = new List<IgnoredModule>();
        var modulesWithUnknownSkipDecisions = new HashSet<IModule>(ReferenceEqualityComparer.Instance);

        foreach (var module in _modules)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var skipDecision = await EvaluateSkipDecisionAsync(module, selection, cancellationToken)
                .ConfigureAwait(false);
            if (skipDecision?.ShouldSkip is true)
            {
                ignoredModules.Add(new IgnoredModule(module, skipDecision));
            }
            else
            {
                runnableModules.Add(module);
                if (skipDecision is null)
                {
                    modulesWithUnknownSkipDecisions.Add(module);
                }
            }
        }

        var cascadeResult = await DependencySkipCascade.ApplyAsync(
                _modules,
                runnableModules,
                ignoredModules,
                _dependencyRegistry,
                _metadataRegistry,
                _ => Task.CompletedTask,
                _ => true,
                cancellationToken)
            .ConfigureAwait(false);

        var skipDecisions = new Dictionary<IModule, SkipDecision>(ReferenceEqualityComparer.Instance);
        foreach (var ignoredModule in cascadeResult.IgnoredModules)
        {
            skipDecisions.Add(ignoredModule.Module, ignoredModule.SkipDecision);
        }

        modulesWithUnknownSkipDecisions = PropagateUnknownSkipDecisions(
            skipDecisions,
            modulesWithUnknownSkipDecisions);
        var estimates = await GetEstimatesAsync(cascadeResult.RunnableModules).ConfigureAwait(false);

        _dependencyChainProvider.Initialize(_modules);
        return new PipelinePlan(
            _modules,
            BuildWaves(skipDecisions, modulesWithUnknownSkipDecisions, estimates));
    }

    private async Task<SkipDecision?> EvaluateSkipDecisionAsync(
        IModule module,
        ModuleSelection selection,
        CancellationToken cancellationToken)
    {
        if (selection.GetSkipDecision(module) is { } selectionDecision)
        {
            return selectionDecision;
        }

        var (shouldIgnore, attributeDecision) = await _conditionHandler
            .ShouldIgnoreForPlanning(module, cancellationToken)
            .ConfigureAwait(false);
        if (shouldIgnore)
        {
            return attributeDecision ?? SkipDecision.Skip("Module was ignored");
        }

        var configuration = module.Configuration;
        if (configuration.SkipCondition is null)
        {
            return SkipDecision.DoNotSkip;
        }

        return await EvaluateFluentSkipConditionAsync(module, configuration, cancellationToken)
            .ConfigureAwait(false);
    }

    private async Task<SkipDecision?> EvaluateFluentSkipConditionAsync(
        IModule module,
        ModuleConfiguration configuration,
        CancellationToken cancellationToken)
    {
        await using var scope = _serviceProvider.CreateAsyncScope();
        var scopedServices = scope.ServiceProvider;
        var executionContext = ExecutionContextFactory.Create(module, module.GetType());

        try
        {
            var moduleContext = new ModuleContext(
                scopedServices.GetRequiredService<IPipelineContext>(),
                module,
                executionContext,
                scopedServices.GetRequiredService<IInternalModuleLoggerProvider>().GetLogger(module.GetType()),
                _mediator,
                _estimatedTimeProvider,
                moduleResultAccessAllowed: false);
            try
            {
                return await configuration.SkipCondition!(moduleContext, cancellationToken).ConfigureAwait(false);
            }
            catch (PlanningModuleResultUnavailableException)
            {
                return null;
            }
        }
        finally
        {
            executionContext.ModuleCancellationTokenSource.Dispose();
        }
    }

    private async Task<IReadOnlyDictionary<IModule, TimeSpan>> GetEstimatesAsync(
        IReadOnlyList<IModule> runnableModules)
    {
        var estimates = await Task.WhenAll(runnableModules.Select(async module =>
            (Module: module, Duration: await _estimatedTimeProvider
                .GetModuleEstimatedTimeAsync(module.GetType())
                .ConfigureAwait(false)))).ConfigureAwait(false);

        var estimatesByModule = new Dictionary<IModule, TimeSpan>(ReferenceEqualityComparer.Instance);
        foreach (var estimate in estimates)
        {
            estimatesByModule.Add(estimate.Module, estimate.Duration);
        }

        return estimatesByModule;
    }

    private IReadOnlyList<PipelinePlanWave> BuildWaves(
        IReadOnlyDictionary<IModule, SkipDecision> skipDecisions,
        IReadOnlySet<IModule> modulesWithUnknownSkipDecisions,
        IReadOnlyDictionary<IModule, TimeSpan> estimates)
    {
        var remainingDependencies = _dependencyChainProvider.ModuleDependencyModels.ToDictionary(
            model => model,
            model => model.IsDependentOn.Count);
        var currentWave = remainingDependencies
            .Where(pair => pair.Value == 0)
            .Select(pair => pair.Key)
            .OrderBy(model => model.Module.GetType().FullName, StringComparer.Ordinal)
            .ToArray();
        var waves = new List<PipelinePlanWave>();

        while (currentWave.Length > 0)
        {
            var plannedModules = currentWave
                .Select(model => CreatePlannedModule(
                    model.Module,
                    skipDecisions,
                    modulesWithUnknownSkipDecisions,
                    estimates))
                .ToArray();
            waves.Add(new PipelinePlanWave(waves.Count + 1, plannedModules));

            var nextWave = new List<ModuleDependencyModel>();
            foreach (var completed in currentWave)
            {
                remainingDependencies.Remove(completed);
                foreach (var dependent in completed.IsDependencyFor)
                {
                    if (--remainingDependencies[dependent] == 0)
                    {
                        nextWave.Add(dependent);
                    }
                }
            }

            currentWave = nextWave
                .OrderBy(model => model.Module.GetType().FullName, StringComparer.Ordinal)
                .ToArray();
        }

        return waves;
    }

    private HashSet<IModule> PropagateUnknownSkipDecisions(
        IReadOnlyDictionary<IModule, SkipDecision> skipDecisions,
        IReadOnlySet<IModule> initialUnknownModules)
    {
        var unknownModules = initialUnknownModules
            .Where(module => !skipDecisions.ContainsKey(module))
            .ToHashSet<IModule>(ReferenceEqualityComparer.Instance);
        var availableModuleTypes = _modules
            .Select(module => module.GetType())
            .Distinct()
            .ToArray();
        var modulesByType = _modules
            .GroupBy(module => module.GetType())
            .ToDictionary(group => group.Key, group => group.ToArray());

        bool changed;
        do
        {
            changed = false;
            foreach (var module in _modules)
            {
                if (skipDecisions.ContainsKey(module) || unknownModules.Contains(module))
                {
                    continue;
                }

                var dependsOnUnknownModule = ModuleDependencyResolver
                    .GetAllDependencies(
                        module,
                        availableModuleTypes,
                        _dependencyRegistry,
                        _metadataRegistry)
                    .Where(dependency => !dependency.Optional)
                    .Select(dependency => dependency.DependencyType)
                    .Any(dependencyType => modulesByType[dependencyType]
                        .Where(dependency => !skipDecisions.ContainsKey(dependency))
                        .All(unknownModules.Contains));
                if (dependsOnUnknownModule)
                {
                    changed |= unknownModules.Add(module);
                }
            }
        }
        while (changed);

        return unknownModules;
    }

    private PipelinePlanModule CreatePlannedModule(
        IModule module,
        IReadOnlyDictionary<IModule, SkipDecision> skipDecisions,
        IReadOnlySet<IModule> modulesWithUnknownSkipDecisions,
        IReadOnlyDictionary<IModule, TimeSpan> estimates)
    {
        var skipDecision = skipDecisions.TryGetValue(module, out var knownSkipDecision)
            ? knownSkipDecision
            : modulesWithUnknownSkipDecisions.Contains(module)
                ? null
                : SkipDecision.DoNotSkip;
        var estimatedDuration = estimates.GetValueOrDefault(module, TimeSpan.Zero);
        return new PipelinePlanModule(
            module,
            _metadataRegistry.GetCategory(module.GetType()),
            skipDecision,
            estimatedDuration);
    }
}
