using Mediator;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Caching;
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
    private readonly bool _moduleCachingEnabled;

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
        IPipelineContextProvider pipelineContextProvider,
        IModuleCacheResultRepository? moduleCacheResultRepository = null)
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
        _moduleCachingEnabled = moduleCacheResultRepository is not null;
    }

    public async Task<PipelinePlan> CreateAsync(CancellationToken cancellationToken = default)
    {
        if (_modules.Count == 0)
        {
            throw new NoModulesRegisteredException();
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

        ValidateRunnableGraph(cascadeResult, moduleTypesUsingHistory);

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
            or ModuleSelfDependencyException
            or DependencyCollisionException)
        {
            var runnableModules = await _moduleRetriever
                .GetRunnableModulesForValidation(cancellationToken)
                .ConfigureAwait(false);
            ModuleDependencyValidator.Validate(runnableModules, _dependencyRegistry, _metadataRegistry);
        }
    }

    private void ValidateRunnableGraph(
        DependencySkipCascadeResult cascadeResult,
        IReadOnlySet<Type> moduleTypesUsingHistory)
    {
        var runnableModules = cascadeResult.RunnableModules
            .Concat(cascadeResult.IgnoredModules
                .Select(ignoredModule => ignoredModule.Module)
                .Where(module => moduleTypesUsingHistory.Contains(module.GetType())))
            .ToArray();

        ModuleDependencyValidator.Validate(
            runnableModules,
            _dependencyRegistry,
            _metadataRegistry,
            moduleTypesUsingHistory);
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
                using var planningResultAccess = PlanningModuleResultAccess.Enter();
                var planningCondition = configuration.PlanningSkipCondition;
                return planningCondition is null
                    ? await configuration.SkipCondition!(moduleContext, cancellationToken).ConfigureAwait(false)
                    : await planningCondition(moduleContext, cancellationToken).ConfigureAwait(false);
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
            estimatedDuration,
            _moduleCachingEnabled
            && !_options.Value.DisableModuleCache
            && module.Configuration.CacheEnabled
            && skipDecision?.ShouldSkip is not true);
    }

    private TimeSpan CalculateEstimatedDuration(IReadOnlyList<PipelinePlanWave> waves)
    {
        var unscheduledModules = waves
            .SelectMany(wave => wave.Modules)
            .ToList();
        var dependencyModels = CreateDependencyModelLookup();
        var finishTimes = new Dictionary<IModule, TimeSpan>(ReferenceEqualityComparer.Instance);
        var scheduledModules = new List<ScheduledModule>();
        var concurrency = _options.Value.Concurrency;
        var currentTime = TimeSpan.Zero;

        while (unscheduledModules.Count > 0)
        {
            var readyModules = unscheduledModules
                .Where(plannedModule => dependencyModels[plannedModule.Module].IsDependentOn
                    .All(dependency => finishTimes.TryGetValue(dependency.Module, out var finish)
                                       && finish <= currentTime))
                .OrderByDescending(plannedModule => GetPriority(plannedModule.Module))
                .ThenBy(plannedModule => plannedModule.Module.GetType().FullName, StringComparer.Ordinal)
                .ToArray();
            if (readyModules.Length == 0)
            {
                var nextFinish = scheduledModules
                    .Where(module => module.Finish > currentTime)
                    .Select(module => module.Finish)
                    .DefaultIfEmpty(TimeSpan.Zero)
                    .Min();
                if (nextFinish > currentTime)
                {
                    currentTime = nextFinish;
                    continue;
                }

                readyModules = unscheduledModules
                    .OrderByDescending(plannedModule => GetPriority(plannedModule.Module))
                    .ThenBy(plannedModule => plannedModule.Module.GetType().FullName, StringComparer.Ordinal)
                    .ToArray();
            }

            var queuedAny = false;
            foreach (var plannedModule in readyModules)
            {
                var profile = CreateSchedulingProfile(plannedModule);
                if (scheduledModules.Any(module => module.QueueTime <= currentTime
                                                   && module.Finish > currentTime
                                                   && HasConstraintConflict(profile, module.Profile)))
                {
                    continue;
                }

                var selected = CreateSchedulingCandidate(
                    plannedModule,
                    currentTime,
                    profile,
                    scheduledModules,
                    concurrency);
                var module = selected.PlannedModule.Module;
                var finish = selected.Timing.ExecutionStart + selected.Duration;
                unscheduledModules.Remove(selected.PlannedModule);
                finishTimes.Add(module, finish);
                scheduledModules.Add(new ScheduledModule(
                    currentTime,
                    selected.Timing.WorkerStart,
                    selected.Timing.LimiterStart,
                    selected.Timing.ExecutionStart,
                    finish,
                    selected.Profile));
                queuedAny = true;
            }

            if (!queuedAny)
            {
                currentTime = scheduledModules
                    .Where(module => module.Finish > currentTime)
                    .Min(module => module.Finish);
            }
        }

        return finishTimes.Values.DefaultIfEmpty(TimeSpan.Zero).Max();
    }

    private static SchedulingCandidate CreateSchedulingCandidate(
        PipelinePlanModule plannedModule,
        TimeSpan queueTime,
        SchedulingProfile profile,
        IReadOnlyList<ScheduledModule> scheduledModules,
        ConcurrencyOptions concurrency)
    {
        var module = plannedModule.Module;
        var duration = plannedModule.ShouldSkip
            ? TimeSpan.Zero
            : plannedModule.EstimatedDuration;
        var timing = FindModuleTiming(
            queueTime,
            duration,
            profile,
            scheduledModules,
            concurrency);
        return new SchedulingCandidate(plannedModule, duration, profile, timing);
    }

    private static SchedulingProfile CreateSchedulingProfile(PipelinePlanModule plannedModule) => new(
        GetConstraintKeys(plannedModule.Module),
        GetExecutionHint(plannedModule.Module),
        plannedModule.ShouldSkip ? null : GetParallelLimiter(plannedModule.Module));

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

    private static ExecutionHint GetExecutionHint(IModule module) =>
        module.Configuration.ExecutionHint
        ?? module.GetType()
            .GetCustomAttributes(typeof(ExecutionHintAttribute), inherit: true)
            .Cast<ExecutionHintAttribute>()
            .FirstOrDefault()
            ?.ExecutionHint
        ?? ExecutionHint.Default;

    private static ModulePriority GetPriority(IModule module) =>
        module.Configuration.Priority
        ?? module.GetType()
            .GetCustomAttributes(typeof(PriorityAttribute), inherit: true)
            .Cast<PriorityAttribute>()
            .FirstOrDefault()
            ?.Priority
        ?? ModulePriority.Normal;

    private static ParallelLimiterProfile? GetParallelLimiter(IModule module)
    {
        var attribute = module.GetType()
            .GetCustomAttributes(typeof(ParallelLimiterAttribute), inherit: true)
            .Cast<ParallelLimiterAttribute>()
            .FirstOrDefault();
        if (attribute is null)
        {
            return null;
        }

        if (attribute.Limit <= 0)
        {
            throw new ArgumentException(
                $"Parallel limit for type '{attribute.Type.FullName}' must be a positive integer, but was {attribute.Limit}.");
        }

        return new ParallelLimiterProfile(attribute.Type, attribute.Limit);
    }

    private static ModuleTiming FindModuleTiming(
        TimeSpan dependencyFinish,
        TimeSpan duration,
        SchedulingProfile profile,
        IReadOnlyList<ScheduledModule> scheduledModules,
        ConcurrencyOptions concurrency)
    {
        if (duration <= TimeSpan.Zero)
        {
            return new ModuleTiming(dependencyFinish, dependencyFinish, dependencyFinish);
        }

        var workerStart = dependencyFinish;
        var workerOccupiedDuration = duration;
        var limiterOccupiedDuration = duration;
        while (true)
        {
            workerStart = FindEarliestWorkerStart(
                workerStart,
                workerOccupiedDuration,
                profile,
                scheduledModules,
                concurrency);
            var limiterStart = FindEarliestLimiterStart(
                workerStart,
                limiterOccupiedDuration,
                profile,
                scheduledModules);
            var executionStart = FindEarliestExecutionStart(
                limiterStart,
                duration,
                profile,
                scheduledModules,
                concurrency);
            var finish = executionStart + duration;
            var adjustedWorkerOccupiedDuration = finish - workerStart;
            var adjustedLimiterOccupiedDuration = finish - limiterStart;
            if (adjustedWorkerOccupiedDuration == workerOccupiedDuration
                && adjustedLimiterOccupiedDuration == limiterOccupiedDuration)
            {
                return new ModuleTiming(workerStart, limiterStart, executionStart);
            }

            workerOccupiedDuration = adjustedWorkerOccupiedDuration;
            limiterOccupiedDuration = adjustedLimiterOccupiedDuration;
        }
    }

    private static TimeSpan FindEarliestWorkerStart(
        TimeSpan earliestStart,
        TimeSpan occupiedDuration,
        SchedulingProfile profile,
        IReadOnlyList<ScheduledModule> scheduledModules,
        ConcurrencyOptions concurrency)
    {
        var start = earliestStart;
        while (true)
        {
            var finish = start + occupiedDuration;
            var checkpoints = scheduledModules
                .Where(module => module.WorkerStart > start && module.WorkerStart < finish)
                .Select(module => module.WorkerStart)
                .Append(start)
                .Distinct()
                .Order();
            var blockedUntil = TimeSpan.Zero;
            foreach (var checkpoint in checkpoints)
            {
                var activeModules = scheduledModules
                    .Where(module => module.WorkerStart <= checkpoint && module.Finish > checkpoint)
                    .ToArray();
                blockedUntil = Max(
                    blockedUntil,
                    GetCapacityBlocker(
                        activeModules,
                        concurrency.MaxParallelism,
                        static _ => true));
                blockedUntil = Max(
                    blockedUntil,
                    activeModules
                        .Where(module => HasConstraintConflict(profile, module.Profile))
                        .Select(module => module.Finish)
                        .DefaultIfEmpty(TimeSpan.Zero)
                        .Max());
                if (blockedUntil > checkpoint)
                {
                    break;
                }
            }

            if (blockedUntil <= start)
            {
                return start;
            }

            start = blockedUntil;
        }
    }

    private static TimeSpan FindEarliestExecutionStart(
        TimeSpan earliestStart,
        TimeSpan duration,
        SchedulingProfile profile,
        IReadOnlyList<ScheduledModule> scheduledModules,
        ConcurrencyOptions concurrency)
    {
        var start = earliestStart;
        while (true)
        {
            var finish = start + duration;
            var checkpoints = scheduledModules
                .Where(module => module.ExecutionStart > start && module.ExecutionStart < finish)
                .Select(module => module.ExecutionStart)
                .Append(start)
                .Distinct()
                .Order();
            var blockedUntil = TimeSpan.Zero;
            foreach (var checkpoint in checkpoints)
            {
                var activeModules = scheduledModules
                    .Where(module => module.ExecutionStart <= checkpoint && module.Finish > checkpoint)
                    .ToArray();
                var executionHintLimit = GetExecutionHintLimit(profile.ExecutionHint, concurrency);
                if (executionHintLimit is { } limit)
                {
                    blockedUntil = Max(
                        blockedUntil,
                        GetCapacityBlocker(
                            activeModules,
                            limit,
                            module => module.Profile.ExecutionHint == profile.ExecutionHint));
                }

                if (blockedUntil > checkpoint)
                {
                    break;
                }
            }

            if (blockedUntil <= start)
            {
                return start;
            }

            start = blockedUntil;
        }
    }

    private static TimeSpan FindEarliestLimiterStart(
        TimeSpan earliestStart,
        TimeSpan occupiedDuration,
        SchedulingProfile profile,
        IReadOnlyList<ScheduledModule> scheduledModules)
    {
        if (profile.ParallelLimiter is not { } parallelLimiter)
        {
            return earliestStart;
        }

        var start = earliestStart;
        while (true)
        {
            var finish = start + occupiedDuration;
            var checkpoints = scheduledModules
                .Where(module => module.LimiterStart > start && module.LimiterStart < finish)
                .Select(module => module.LimiterStart)
                .Append(start)
                .Distinct()
                .Order();
            var blockedUntil = TimeSpan.Zero;
            foreach (var checkpoint in checkpoints)
            {
                var activeModules = scheduledModules
                    .Where(module => module.LimiterStart <= checkpoint && module.Finish > checkpoint)
                    .ToArray();
                blockedUntil = GetCapacityBlocker(
                    activeModules,
                    parallelLimiter.Limit,
                    module => module.Profile.ParallelLimiter?.Type == parallelLimiter.Type);
                if (blockedUntil > checkpoint)
                {
                    break;
                }
            }

            if (blockedUntil <= start)
            {
                return start;
            }

            start = blockedUntil;
        }
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

    private static int? GetExecutionHintLimit(
        ExecutionHint executionHint,
        ConcurrencyOptions concurrency) =>
        executionHint switch
        {
            ExecutionHint.CpuBound => concurrency.MaxCpuIntensiveModules,
            ExecutionHint.IoBound => concurrency.MaxIoIntensiveModules,
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
        ExecutionHint ExecutionHint,
        ParallelLimiterProfile? ParallelLimiter);

    private sealed record ParallelLimiterProfile(Type Type, int Limit);

    private sealed record ScheduledModule(
        TimeSpan QueueTime,
        TimeSpan WorkerStart,
        TimeSpan LimiterStart,
        TimeSpan ExecutionStart,
        TimeSpan Finish,
        SchedulingProfile Profile);

    private sealed record ModuleTiming(
        TimeSpan WorkerStart,
        TimeSpan LimiterStart,
        TimeSpan ExecutionStart);

    private sealed record SchedulingCandidate(
        PipelinePlanModule PlannedModule,
        TimeSpan Duration,
        SchedulingProfile Profile,
        ModuleTiming Timing);
}
