using Mediator;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Configuration;
using ModularPipelines.Context;
using ModularPipelines.Engine.Attributes;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Engine.Execution;
using ModularPipelines.Enums;
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
    private readonly ModuleRetriever _moduleRetriever;
    private readonly IModuleConditionHandler _conditionHandler;
    private readonly ISafeModuleEstimatedTimeProvider _estimatedTimeProvider;
    private readonly IModuleDependencyRegistry _dependencyRegistry;
    private readonly IModuleMetadataRegistry _metadataRegistry;
    private readonly IDependencyChainProvider _dependencyChainProvider;
    private readonly IOptions<PipelineOptions> _options;
    private readonly IMediator _mediator;
    private readonly IModuleResultHistoryProvider _resultHistoryProvider;
    private readonly IPipelineContextProvider _pipelineContextProvider;

    public PipelinePlanner(
        IServiceProvider serviceProvider,
        IEnumerable<IModule> modules,
        IRegistrationEventExecutor registrationEventExecutor,
        ModuleRetriever moduleRetriever,
        IModuleConditionHandler conditionHandler,
        ISafeModuleEstimatedTimeProvider estimatedTimeProvider,
        IModuleDependencyRegistry dependencyRegistry,
        IModuleMetadataRegistry metadataRegistry,
        IDependencyChainProvider dependencyChainProvider,
        IOptions<PipelineOptions> options,
        IMediator mediator,
        IModuleResultHistoryProvider resultHistoryProvider,
        IPipelineContextProvider pipelineContextProvider)
    {
        _serviceProvider = serviceProvider;
        _modules = modules.Distinct<IModule>(ReferenceEqualityComparer.Instance).ToArray();
        _registrationEventExecutor = registrationEventExecutor;
        _moduleRetriever = moduleRetriever;
        _conditionHandler = conditionHandler;
        _estimatedTimeProvider = estimatedTimeProvider;
        _dependencyRegistry = dependencyRegistry;
        _metadataRegistry = metadataRegistry;
        _dependencyChainProvider = dependencyChainProvider;
        _options = options;
        _mediator = mediator;
        _resultHistoryProvider = resultHistoryProvider;
        _pipelineContextProvider = pipelineContextProvider;
    }

    public async Task<PipelinePlan> CreateAsync(CancellationToken cancellationToken = default)
    {
        if (_modules.Count == 0)
        {
            throw new PipelineException("No modules have been registered");
        }

        await _registrationEventExecutor.InvokeRegistrationEventsAsync(_modules).ConfigureAwait(false);
        cancellationToken.ThrowIfCancellationRequested();
        await ValidateDependenciesAsync(cancellationToken).ConfigureAwait(false);

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

        var moduleTypesUsingHistory = new HashSet<Type>();
        var pipelineContext = _pipelineContextProvider.GetModuleContext();
        var cascadeResult = await DependencySkipCascade.ApplyAsync(
                _modules,
                runnableModules,
                ignoredModules,
                _dependencyRegistry,
                _metadataRegistry,
                async pendingIgnoredModules =>
                {
                    foreach (var ignoredModule in pendingIgnoredModules)
                    {
                        if (await _resultHistoryProvider
                                .TryGetAsync(ignoredModule.Module, pipelineContext)
                                .ConfigureAwait(false) is not null)
                        {
                            moduleTypesUsingHistory.Add(ignoredModule.Module.GetType());
                        }
                    }
                },
                moduleType => !moduleTypesUsingHistory.Contains(moduleType),
                cancellationToken)
            .ConfigureAwait(false);

        var skipDecisions = new Dictionary<IModule, SkipDecision>(ReferenceEqualityComparer.Instance);
        foreach (var ignoredModule in cascadeResult.IgnoredModules)
        {
            skipDecisions.Add(ignoredModule.Module, ignoredModule.SkipDecision);
        }

        modulesWithUnknownSkipDecisions = PropagateUnknownSkipDecisions(
            skipDecisions,
            modulesWithUnknownSkipDecisions,
            moduleTypesUsingHistory);
        var estimates = await GetEstimatesAsync(cascadeResult.RunnableModules).ConfigureAwait(false);

        _dependencyChainProvider.Initialize(_modules);
        var waves = BuildWaves(skipDecisions, modulesWithUnknownSkipDecisions, estimates);
        return new PipelinePlan(_modules, waves, CalculateEstimatedDuration(waves));
    }

    private async Task ValidateDependenciesAsync(CancellationToken cancellationToken)
    {
        try
        {
            ModuleDependencyValidator.Validate(_modules, _dependencyRegistry, _metadataRegistry);
        }
        catch (Exception exception) when (exception is ModuleNotRegisteredException
            or ModuleReferencingSelfException
            or DependencyCollisionException)
        {
            var runnableModules = await _moduleRetriever
                .GetRunnableModulesForValidation(cancellationToken)
                .ConfigureAwait(false);
            ModuleDependencyValidator.Validate(runnableModules, _dependencyRegistry, _metadataRegistry);
        }
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

        if (remainingDependencies.Count > 0)
        {
            var unresolvedModules = remainingDependencies.Keys
                .OrderBy(model => model.Module.GetType().FullName, StringComparer.Ordinal)
                .Select(model => CreatePlannedModule(
                    model.Module,
                    skipDecisions,
                    modulesWithUnknownSkipDecisions,
                    estimates))
                .ToArray();
            waves.Add(new PipelinePlanWave(waves.Count + 1, unresolvedModules));
        }

        return waves;
    }

    private HashSet<IModule> PropagateUnknownSkipDecisions(
        IReadOnlyDictionary<IModule, SkipDecision> skipDecisions,
        IReadOnlySet<IModule> initialUnknownModules,
        IReadOnlySet<Type> moduleTypesUsingHistory)
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
                        .Where(dependency => !skipDecisions.ContainsKey(dependency)
                                             || moduleTypesUsingHistory.Contains(dependency.GetType()))
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

    private TimeSpan CalculateEstimatedDuration(IReadOnlyList<PipelinePlanWave> waves)
    {
        var plannedModules = waves
            .SelectMany(wave => wave.Modules)
            .ToArray();
        var dependencyModels = CreateDependencyModelLookup();
        var finishTimes = new Dictionary<IModule, TimeSpan>(ReferenceEqualityComparer.Instance);
        var scheduledModules = new List<ScheduledModule>();
        var concurrency = _options.Value.Concurrency;

        foreach (var plannedModule in plannedModules)
        {
            var module = plannedModule.Module;
            var duration = plannedModule.ShouldSkip
                ? TimeSpan.Zero
                : plannedModule.EstimatedDuration;
            var dependencyFinish = dependencyModels[module].IsDependentOn
                .Select(dependency => finishTimes.GetValueOrDefault(dependency.Module, TimeSpan.Zero))
                .DefaultIfEmpty(TimeSpan.Zero)
                .Max();
            var schedulingProfile = new SchedulingProfile(
                GetConstraintKeys(module),
                GetExecutionType(module));
            var start = FindEarliestStart(
                dependencyFinish,
                duration,
                schedulingProfile,
                scheduledModules,
                concurrency);
            var finish = start + duration;
            finishTimes[module] = finish;
            scheduledModules.Add(new ScheduledModule(start, finish, schedulingProfile));
        }

        return finishTimes.Values.DefaultIfEmpty(TimeSpan.Zero).Max();
    }

    private static IReadOnlyCollection<string>? GetConstraintKeys(IModule module)
    {
        if (module.Configuration.ParallelConstraintKeys is { } configuredConstraintKeys)
        {
            return configuredConstraintKeys;
        }

        return module.GetType()
            .GetCustomAttributes(typeof(NotInParallelAttribute), inherit: true)
            .Cast<NotInParallelAttribute>()
            .FirstOrDefault()
            ?.ConstraintKeys;
    }

    private IReadOnlyDictionary<IModule, ModuleDependencyModel> CreateDependencyModelLookup()
    {
        var dependencyModels = new Dictionary<IModule, ModuleDependencyModel>(
            ReferenceEqualityComparer.Instance);
        foreach (var dependencyModel in _dependencyChainProvider.ModuleDependencyModels)
        {
            dependencyModels.Add(dependencyModel.Module, dependencyModel);
        }

        return dependencyModels;
    }

    private static ExecutionType GetExecutionType(IModule module) =>
        module.Configuration.ExecutionType
        ?? module.GetType()
            .GetCustomAttributes(typeof(ExecutionHintAttribute), inherit: true)
            .Cast<ExecutionHintAttribute>()
            .FirstOrDefault()
            ?.ExecutionType
        ?? ExecutionType.Default;

    private static TimeSpan FindEarliestStart(
        TimeSpan dependencyFinish,
        TimeSpan duration,
        SchedulingProfile profile,
        IReadOnlyList<ScheduledModule> scheduledModules,
        ConcurrencyOptions concurrency)
    {
        if (duration <= TimeSpan.Zero)
        {
            return dependencyFinish;
        }

        var start = dependencyFinish;
        while (true)
        {
            var blockedUntil = GetBlockedUntil(
                start,
                duration,
                profile,
                scheduledModules,
                concurrency);
            if (blockedUntil <= start)
            {
                return start;
            }

            start = blockedUntil;
        }
    }

    private static TimeSpan GetBlockedUntil(
        TimeSpan start,
        TimeSpan duration,
        SchedulingProfile profile,
        IReadOnlyList<ScheduledModule> scheduledModules,
        ConcurrencyOptions concurrency)
    {
        var finish = start + duration;
        var checkpoints = scheduledModules
            .Where(module => module.Start > start && module.Start < finish)
            .Select(module => module.Start)
            .Append(start)
            .Distinct()
            .Order()
            .ToArray();

        foreach (var checkpoint in checkpoints)
        {
            var activeModules = scheduledModules
                .Where(module => module.Start <= checkpoint && module.Finish > checkpoint)
                .ToArray();
            var blockedUntil = GetCapacityBlocker(
                activeModules,
                concurrency.MaxParallelism,
                static _ => true);
            var executionTypeLimit = GetExecutionTypeLimit(profile.ExecutionType, concurrency);
            if (executionTypeLimit is { } limit)
            {
                blockedUntil = Max(
                    blockedUntil,
                    GetCapacityBlocker(
                        activeModules,
                        limit,
                        module => module.Profile.ExecutionType == profile.ExecutionType));
            }

            var constraintBlocker = activeModules
                .Where(module => HasConstraintConflict(profile, module.Profile))
                .Select(module => module.Finish)
                .DefaultIfEmpty(TimeSpan.Zero)
                .Max();
            blockedUntil = Max(blockedUntil, constraintBlocker);
            if (blockedUntil > checkpoint)
            {
                return blockedUntil;
            }
        }

        return start;
    }

    private static TimeSpan GetCapacityBlocker(
        IReadOnlyCollection<ScheduledModule> activeModules,
        int limit,
        Func<ScheduledModule, bool> predicate)
    {
        var constrainedModules = activeModules.Where(predicate).ToArray();
        return constrainedModules.Length >= limit
            ? constrainedModules.Min(module => module.Finish)
            : TimeSpan.Zero;
    }

    private static int? GetExecutionTypeLimit(
        ExecutionType executionType,
        ConcurrencyOptions concurrency) =>
        executionType switch
        {
            ExecutionType.CpuIntensive => concurrency.MaxCpuIntensiveModules,
            ExecutionType.IoIntensive => concurrency.MaxIoIntensiveModules,
            _ => null,
        };

    private static bool HasConstraintConflict(
        SchedulingProfile candidate,
        SchedulingProfile scheduled)
    {
        if (candidate.ConstraintKeys is { Count: 0 }
            || scheduled.ConstraintKeys is { Count: 0 })
        {
            return true;
        }

        return candidate.ConstraintKeys is not null
               && scheduled.ConstraintKeys is not null
               && candidate.ConstraintKeys.Intersect(
                   scheduled.ConstraintKeys,
                   StringComparer.Ordinal).Any();
    }

    private static TimeSpan Max(TimeSpan first, TimeSpan second) =>
        first > second ? first : second;

    private sealed record SchedulingProfile(
        IReadOnlyCollection<string>? ConstraintKeys,
        ExecutionType ExecutionType);

    private sealed record ScheduledModule(
        TimeSpan Start,
        TimeSpan Finish,
        SchedulingProfile Profile);
}
