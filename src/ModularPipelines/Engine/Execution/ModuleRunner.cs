using System.Collections.Concurrent;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Mediator;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Distributed;
using ModularPipelines.Distributed.Artifacts;
using ModularPipelines.Engine.Attributes;
using ModularPipelines.Engine.Executors;
using ModularPipelines.Events;
using ModularPipelines.Exceptions;
using ModularPipelines.Helpers;
using ModularPipelines.Logging;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using ModularPipelines.Tracing;

namespace ModularPipelines.Engine.Execution;

/// <summary>
/// Responsible for executing a single module with proper scoping and coordination.
/// </summary>
internal class ModuleRunner : IModuleRunner
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IModuleExecutionPipeline _executionPipeline;
    private readonly IPipelineSetupExecutor _pipelineSetupExecutor;
    private readonly IMediator _mediator;
    private readonly ISafeModuleEstimatedTimeProvider _moduleEstimatedTimeProvider;
    private readonly ModuleDisposer _moduleDisposer;
    private readonly IModuleResultRegistry _resultRegistry;
    private readonly IOptions<PipelineOptions> _pipelineOptions;
    private readonly ILogger<ModuleRunner> _logger;
    private readonly IDependencyWaiter _dependencyWaiter;
    private readonly IParallelLimitHandler _parallelLimitHandler;
    private readonly IModuleLifecycleEventInvoker _lifecycleEventInvoker;
    private readonly IModuleAttributeEventService _moduleAttributeEventService;
    private readonly IModuleResultRegistrar _resultRegistrar;
    private readonly ISecretObfuscator _secretObfuscator;
    private readonly ModulePlanningSkipEvaluator _modulePlanningSkipEvaluator;
    private readonly IModuleResultHistoryProvider _resultHistoryProvider;
    private readonly IPipelineContextProvider _pipelineContextProvider;
    private readonly ArtifactLifecycleManager _artifactLifecycleManager;
    private readonly bool _manageArtifactsLocally;
    private readonly IReadOnlyDictionary<Type, IReadOnlyDictionary<string, IReadOnlySet<Type>>>
        _localArtifactConsumers;
    private readonly IReadOnlyList<Type> _registeredModuleTypes;
    private readonly ArtifactDemandPlanCache _artifactDemandPlanCache = new();
    private readonly ConcurrentDictionary<Type, Lazy<Task<SkipDecision?>>>
        _planningSkipEvaluations = new();
    private readonly ConcurrentDictionary<Type, Lazy<Task<bool>>> _historicalResultAvailability = new();

    public ModuleRunner(
        IServiceProvider serviceProvider,
        IModuleExecutionPipeline executionPipeline,
        IPipelineSetupExecutor pipelineSetupExecutor,
        IMediator mediator,
        ISafeModuleEstimatedTimeProvider moduleEstimatedTimeProvider,
        ModuleDisposer moduleDisposer,
        IModuleResultRegistry resultRegistry,
        IOptions<PipelineOptions> pipelineOptions,
        ILogger<ModuleRunner> logger,
        IDependencyWaiter dependencyWaiter,
        IParallelLimitHandler parallelLimitHandler,
        IModuleLifecycleEventInvoker lifecycleEventInvoker,
        IModuleAttributeEventService moduleAttributeEventService,
        IModuleResultRegistrar resultRegistrar,
        ModulePlanningSkipEvaluator modulePlanningSkipEvaluator,
        IModuleResultHistoryProvider resultHistoryProvider,
        IPipelineContextProvider pipelineContextProvider,
        ArtifactLifecycleManager artifactLifecycleManager,
        IOptions<DistributedOptions> distributedOptions,
        IEnumerable<IModule> modules,
        ISecretObfuscator secretObfuscator)
    {
        _serviceProvider = serviceProvider;
        _executionPipeline = executionPipeline;
        _pipelineSetupExecutor = pipelineSetupExecutor;
        _mediator = mediator;
        _moduleEstimatedTimeProvider = moduleEstimatedTimeProvider;
        _moduleDisposer = moduleDisposer;
        _resultRegistry = resultRegistry;
        _pipelineOptions = pipelineOptions;
        _logger = logger;
        _dependencyWaiter = dependencyWaiter;
        _parallelLimitHandler = parallelLimitHandler;
        _lifecycleEventInvoker = lifecycleEventInvoker;
        _moduleAttributeEventService = moduleAttributeEventService;
        _resultRegistrar = resultRegistrar;
        _secretObfuscator = secretObfuscator;
        _modulePlanningSkipEvaluator = modulePlanningSkipEvaluator;
        _resultHistoryProvider = resultHistoryProvider;
        _pipelineContextProvider = pipelineContextProvider;
        _artifactLifecycleManager = artifactLifecycleManager;
        _manageArtifactsLocally = !distributedOptions.Value.Enabled
                                  || distributedOptions.Value.TotalInstances <= 1;
        var registeredModules = modules.ToArray();
        _registeredModuleTypes = registeredModules.Select(static module => module.GetType()).ToArray();
        _localArtifactConsumers = GetLocalArtifactConsumers(registeredModules);
    }

    /// <inheritdoc />
    public Task ExecuteAsync(ModuleState moduleState, IModuleScheduler scheduler, CancellationToken cancellationToken)
    {
        return ExecuteCore(moduleState, scheduler, cancellationToken, skipDependencyWait: false);
    }

    /// <inheritdoc />
    public Task ExecuteWithoutDependencyWaitAsync(ModuleState moduleState, IModuleScheduler scheduler, CancellationToken cancellationToken)
    {
        return ExecuteCore(moduleState, scheduler, cancellationToken, skipDependencyWait: true);
    }

    private async Task ExecuteCore(
        ModuleState moduleState,
        IModuleScheduler scheduler,
        CancellationToken cancellationToken,
        bool skipDependencyWait)
    {
        var module = moduleState.Module;
        var moduleType = moduleState.ModuleType;
        var moduleName = moduleType.Name;

        // Create a scope to resolve scoped services like IModuleContext and ModuleLogger<T>
        var scope = _serviceProvider.CreateAsyncScope();
        await using (scope.ConfigureAwait(false))
        {
            try
            {
                // Check if the module can proceed with execution
                // Returns false if constraints (e.g., NotInParallel) prevent execution
                if (!scheduler.MarkModuleStarted(moduleType))
                {
                    _logger.LogDebug("Module {ModuleName} deferred due to constraint check failure", moduleName);
                    return; // Module will be rescheduled by the scheduler
                }

                _logger.LogDebug("Starting module {ModuleName}", moduleName);

                if (!skipDependencyWait)
                {
                    await _dependencyWaiter.WaitForDependenciesAsync(moduleState, scheduler, scope.ServiceProvider).ConfigureAwait(false);
                }
                else
                {
                    _logger.LogDebug("Skipping dependency wait for late-started AlwaysRun module: {ModuleName}", moduleName);
                }

                var executionContext = CreateExecutionContext(module, moduleType);
                ApplyDependencySkip(moduleState, executionContext);
                executionContext.AllowHistoricalResultWhenSkipped =
                    !await HasRunnableArtifactConsumerAsync(
                            moduleType,
                            scheduler,
                            cancellationToken)
                        .ConfigureAwait(false);

                await ExecuteModuleWithPipeline(
                        moduleState,
                        scheduler,
                        scope.ServiceProvider,
                        executionContext,
                        cancellationToken)
                    .ConfigureAwait(false);

                scheduler.MarkModuleCompleted(moduleType, true, statusOverride: moduleState.Result?.ModuleStatus);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Module {ModuleName} failed", moduleName);
                var isDependencyFailure = ex is DependencyFailedException;
                scheduler.MarkModuleCompleted(
                    moduleType,
                    false,
                    ex,
                    isDependencyFailure ? Enums.Status.DependencyFailed : null);

                if (moduleState.Result == null)
                {
                    if (isDependencyFailure)
                    {
                        _resultRegistrar.RegisterDependencyFailedResult(module, moduleType, ex);
                    }
                    else
                    {
                        _resultRegistrar.RegisterTerminatedResult(module, moduleType, ex);
                    }
                }

                if (_pipelineOptions.Value.ExecutionMode == ExecutionMode.StopOnFirstException)
                {
                    throw;
                }
            }
        }
    }

    private async Task UploadProducedArtifactsAsync(
        Type moduleType,
        IModuleScheduler scheduler,
        CancellationToken cancellationToken)
    {
        if (!_manageArtifactsLocally
            || !_localArtifactConsumers.ContainsKey(moduleType))
        {
            return;
        }

        var demandPlan = await GetArtifactDemandPlanAsync(
                scheduler,
                cancellationToken)
            .ConfigureAwait(false);
        if (!demandPlan.RequiredArtifactsByProducer.TryGetValue(moduleType, out var artifactNames)
            || artifactNames.Count == 0)
        {
            return;
        }

        await _artifactLifecycleManager
            .UploadProducedArtifactsAsync(moduleType, artifactNames, cancellationToken)
            .ConfigureAwait(false);
    }

    private async Task<bool> HasRunnableArtifactConsumerAsync(
        Type producerType,
        IModuleScheduler scheduler,
        CancellationToken cancellationToken)
    {
        if (!_localArtifactConsumers.ContainsKey(producerType))
        {
            return false;
        }

        var demandPlan = await GetArtifactDemandPlanAsync(
                scheduler,
                cancellationToken)
            .ConfigureAwait(false);
        return demandPlan.RequiredProducerTypes.Contains(producerType);
    }

    private Task<ArtifactDemandPlan> GetArtifactDemandPlanAsync(
        IModuleScheduler scheduler,
        CancellationToken cancellationToken)
    {
        return _artifactDemandPlanCache.GetAsync(
            () => _registeredModuleTypes
                .Where(moduleType => scheduler.GetModuleCompletionTask(moduleType)?.IsCompleted == true)
                .ToHashSet(),
            async () =>
            {
                var requiredProducerTypes = await ArtifactDemandPlanner.ResolveAsync(async currentDemand =>
                {
                    var nextRequiredProducerTypes = new HashSet<Type>();
                    foreach (var (producerType, consumersByArtifact) in _localArtifactConsumers)
                    {
                        foreach (var consumerType in consumersByArtifact.Values.SelectMany(consumers => consumers))
                        {
                            if (await IsRunnableArtifactConsumerAsync(
                                    consumerType,
                                    scheduler,
                                    cancellationToken,
                                    currentDemand)
                                .ConfigureAwait(false))
                            {
                                nextRequiredProducerTypes.Add(producerType);
                                break;
                            }
                        }
                    }

                    return nextRequiredProducerTypes;
                }).ConfigureAwait(false);

                var requiredArtifactsByProducer = new Dictionary<Type, IReadOnlySet<string>>();
                foreach (var producerType in requiredProducerTypes)
                {
                    if (!_localArtifactConsumers.TryGetValue(producerType, out var consumersByArtifact))
                    {
                        continue;
                    }

                    var requiredArtifactNames = new HashSet<string>(StringComparer.Ordinal);
                    foreach (var (artifactName, consumerTypes) in consumersByArtifact)
                    {
                        foreach (var consumerType in consumerTypes)
                        {
                            if (!await IsRunnableArtifactConsumerAsync(
                                    consumerType,
                                    scheduler,
                                    cancellationToken,
                                    requiredProducerTypes)
                                .ConfigureAwait(false))
                            {
                                continue;
                            }

                            requiredArtifactNames.Add(artifactName);
                            break;
                        }
                    }

                    if (requiredArtifactNames.Count > 0)
                    {
                        requiredArtifactsByProducer.Add(producerType, requiredArtifactNames);
                    }
                }

                return new ArtifactDemandPlan(requiredProducerTypes, requiredArtifactsByProducer);
            },
            cancellationToken);
    }

    private async Task<bool> IsRunnableArtifactConsumerAsync(
        Type consumerType,
        IModuleScheduler scheduler,
        CancellationToken cancellationToken,
        IReadOnlySet<Type> requiredProducerTypes)
    {
        if (scheduler.GetModuleState(consumerType) is not { State: not ModuleExecutionState.Completed } moduleState
            || moduleState.SkipResult.ShouldSkip)
        {
            return false;
        }

        var dependencyDemand = await GetRequiredDependencyDemandAsync(
                moduleState,
                scheduler,
                cancellationToken,
                [consumerType],
                requiredProducerTypes)
            .ConfigureAwait(false);
        if (dependencyDemand.IsUnrecoverable)
        {
            return false;
        }

        if (dependencyDemand.HasPendingDependency)
        {
            return true;
        }

        var skipDecision = await EvaluatePlanningSkipAsync(moduleState, cancellationToken).ConfigureAwait(false);
        if (skipDecision?.ShouldSkip != true)
        {
            return true;
        }

        moduleState.TrySetSkipResult(skipDecision);
        return false;
    }

    private async Task<SkipDecision?> EvaluatePlanningSkipAsync(
        ModuleState moduleState,
        CancellationToken cancellationToken)
    {
        var evaluation = _planningSkipEvaluations.GetOrAdd(
            moduleState.Module.GetType(),
            _ => new Lazy<Task<SkipDecision?>>(
                () => _modulePlanningSkipEvaluator.EvaluateAsync(moduleState.Module, cancellationToken),
                LazyThreadSafetyMode.ExecutionAndPublication));
        return await evaluation.Value.ConfigureAwait(false);
    }

    private async Task<RequiredDependencyDemand> GetRequiredDependencyDemandAsync(
        ModuleState moduleState,
        IModuleScheduler scheduler,
        CancellationToken cancellationToken,
        HashSet<Type> visited,
        IReadOnlySet<Type> requiredProducerTypes)
    {
        var hasPendingDependency = false;
        foreach (var dependency in moduleState.Dependencies.Where(static dependency => !dependency.Value))
        {
            if (_resultRegistry.GetResult(dependency.Key)?.ModuleStatus == Enums.Status.Skipped)
            {
                return new RequiredDependencyDemand(
                    IsUnrecoverable: true,
                    HasPendingDependency: hasPendingDependency);
            }

            if (scheduler.GetModuleState(dependency.Key) is not { } dependencyState
                || dependencyState.State == ModuleExecutionState.Completed
                || !visited.Add(dependency.Key))
            {
                continue;
            }

            try
            {
                if (dependencyState.SkipResult.ShouldSkip)
                {
                    if (await IsSkippedDependencyUnrecoverableAsync(
                            dependency.Key,
                            dependencyState,
                            requiredProducerTypes)
                        .ConfigureAwait(false))
                    {
                        return new RequiredDependencyDemand(
                            IsUnrecoverable: true,
                            HasPendingDependency: hasPendingDependency);
                    }

                    continue;
                }

                var transitiveDemand = await GetRequiredDependencyDemandAsync(
                        dependencyState,
                        scheduler,
                        cancellationToken,
                        visited,
                        requiredProducerTypes)
                    .ConfigureAwait(false);
                if (transitiveDemand.IsUnrecoverable)
                {
                    return new RequiredDependencyDemand(
                        IsUnrecoverable: true,
                        HasPendingDependency: true);
                }

                if (transitiveDemand.HasPendingDependency)
                {
                    hasPendingDependency = true;
                    continue;
                }

                var skipDecision = await EvaluatePlanningSkipAsync(dependencyState, cancellationToken)
                    .ConfigureAwait(false);
                if (skipDecision?.ShouldSkip == true)
                {
                    dependencyState.TrySetSkipResult(skipDecision);
                    if (await IsSkippedDependencyUnrecoverableAsync(
                            dependency.Key,
                            dependencyState,
                            requiredProducerTypes)
                        .ConfigureAwait(false))
                    {
                        return new RequiredDependencyDemand(
                            IsUnrecoverable: true,
                            HasPendingDependency: hasPendingDependency);
                    }

                    continue;
                }

                hasPendingDependency = true;
            }
            finally
            {
                visited.Remove(dependency.Key);
            }
        }

        return new RequiredDependencyDemand(
            IsUnrecoverable: false,
            HasPendingDependency: hasPendingDependency);
    }

    private async Task<bool> IsSkippedDependencyUnrecoverableAsync(
        Type dependencyType,
        ModuleState dependencyState,
        IReadOnlySet<Type> requiredProducerTypes) =>
        requiredProducerTypes.Contains(dependencyType)
        || !await HasHistoricalResultAsync(dependencyState).ConfigureAwait(false);

    private async Task<bool> HasHistoricalResultAsync(ModuleState moduleState)
    {
        var evaluation = _historicalResultAvailability.GetOrAdd(
            moduleState.ModuleType,
            _ => new Lazy<Task<bool>>(
                async () => await _resultHistoryProvider
                        .TryGetAsync(moduleState.Module, _pipelineContextProvider.GetModuleContext())
                        .ConfigureAwait(false) is not null,
                LazyThreadSafetyMode.ExecutionAndPublication));
        return await evaluation.Value.ConfigureAwait(false);
    }

    private readonly record struct RequiredDependencyDemand(
        bool IsUnrecoverable,
        bool HasPendingDependency);

    private static IReadOnlyDictionary<Type, IReadOnlyDictionary<string, IReadOnlySet<Type>>>
        GetLocalArtifactConsumers(IEnumerable<IModule> modules)
    {
        return modules
            .SelectMany(module => module.GetType()
                .GetCustomAttributes(typeof(ConsumesArtifactAttribute), inherit: true)
                .Cast<ConsumesArtifactAttribute>()
                .Select(attribute => (ConsumerType: module.GetType(), Attribute: attribute)))
            .GroupBy(item => item.Attribute.ProducerModule)
            .ToDictionary(
                producerGroup => producerGroup.Key,
                producerGroup => (IReadOnlyDictionary<string, IReadOnlySet<Type>>) producerGroup
                    .GroupBy(item => item.Attribute.ArtifactName, StringComparer.Ordinal)
                    .ToDictionary(
                        artifactGroup => artifactGroup.Key,
                        artifactGroup => (IReadOnlySet<Type>) artifactGroup
                            .Select(item => item.ConsumerType)
                            .ToHashSet(),
                        StringComparer.Ordinal));
    }

    private async Task ExecuteModuleWithPipeline(
        ModuleState moduleState,
        IModuleScheduler scheduler,
        IServiceProvider scopedServiceProvider,
        ModuleExecutionContext executionContext,
        CancellationToken cancellationToken)
    {
        var module = moduleState.Module;
        var moduleType = moduleState.ModuleType;

        var pipelineContext = scopedServiceProvider.GetRequiredService<IPipelineContext>();

        // Create module-specific context
        var logger = GetModuleLogger(scopedServiceProvider, moduleType);
        var moduleContext = new ModuleContext(
            pipelineContext,
            module,
            executionContext,
            logger,
            _mediator,
            _moduleEstimatedTimeProvider);

        var telemetryStart = Stopwatch.GetTimestamp();
        var telemetryStatus = "Failed";
        using var activity = ModuleActivityTracing.StartModuleActivity(moduleType);
        executionContext.ModuleActivity = activity;

        // Set up logging and module type context using scope wrapper for proper cleanup
        await using var loggerScope = new ModuleLoggerScope(logger, moduleType);

        try
        {
            await ExecuteModuleLifecycle(
                    moduleState,
                    scheduler,
                    scopedServiceProvider,
                    pipelineContext,
                    executionContext,
                    moduleContext,
                    cancellationToken)
                .ConfigureAwait(false);

            // Record success, skip, or ignored failure status on the Activity
            if (executionContext.Status == Enums.Status.Skipped)
            {
                telemetryStatus = "Skipped";
                ModuleActivityTracing.RecordSkipped(activity);
            }
            else if (executionContext.Status == Enums.Status.IgnoredFailure)
            {
                telemetryStatus = "IgnoredFailure";
                activity?.SetTag(ModuleActivityTracing.ModuleStatusTag, telemetryStatus);
                activity?.SetStatus(ActivityStatusCode.Ok, "Module failed but failure was ignored");
            }
            else if (executionContext.Status == Enums.Status.UsedHistory)
            {
                telemetryStatus = "UsedHistory";
                ModuleActivityTracing.RecordUsedHistory(activity);
            }
            else if (executionContext.Status == Enums.Status.CachedResult)
            {
                telemetryStatus = "CachedResult";
                ModuleActivityTracing.RecordCachedResult(activity);
            }
            else if (executionContext.Status == Enums.Status.PipelineTerminated)
            {
                telemetryStatus = "PipelineTerminated";
                ModuleActivityTracing.RecordPipelineTerminated(activity);
            }
            else
            {
                telemetryStatus = "Successful";
                ModuleActivityTracing.RecordSuccess(activity);
            }
        }
        catch (Exception ex)
        {
            var obfuscatedMessage = _secretObfuscator.Obfuscate(ex.Message, null);
            if (executionContext.Status == Enums.Status.TimedOut)
            {
                telemetryStatus = "TimedOut";
                ModuleActivityTracing.RecordTimedOut(activity, ex, obfuscatedMessage);
            }
            else
            {
                ModuleActivityTracing.RecordFailure(activity, ex, obfuscatedMessage);
            }

            throw;
        }
        finally
        {
            ModuleActivityTracing.RecordModuleMetrics(
                moduleType,
                telemetryStatus,
                Stopwatch.GetElapsedTime(telemetryStart));
        }
    }

    private async Task ExecuteModuleLifecycle(
        ModuleState moduleState,
        IModuleScheduler scheduler,
        IServiceProvider scopedServiceProvider,
        IPipelineContext pipelineContext,
        ModuleExecutionContext executionContext,
        IModuleContext moduleContext,
        CancellationToken cancellationToken)
    {
        var module = moduleState.Module;
        var moduleType = moduleState.ModuleType;

        // Before module hooks - module is ready (dependencies satisfied)
        await _pipelineSetupExecutor.OnModuleReadyAsync(moduleState).ConfigureAwait(false);
        await _pipelineSetupExecutor.OnModuleStartAsync(moduleState).ConfigureAwait(false);

        var estimatedDuration = await _moduleEstimatedTimeProvider.GetModuleEstimatedTimeAsync(moduleType).ConfigureAwait(false);
        await _mediator.Publish(
                new ModuleStartedNotification(moduleState, estimatedDuration),
                CancellationToken.None)
            .ConfigureAwait(false);

        using var semaphoreHandle = await _parallelLimitHandler.AcquireParallelLimitAsync(moduleType).ConfigureAwait(false);
        using var executionTypeHandle = await _parallelLimitHandler.AcquireExecutionTypeLimitAsync(moduleState).ConfigureAwait(false);

        // Track start time for lifecycle events
        var startTime = DateTimeOffset.UtcNow;
        var moduleAttributes = _moduleAttributeEventService.GetAttributes(moduleType);
        var lifecycleContext = new ModuleLifecycleContext(
            module,
            moduleType,
            moduleAttributes,
            startTime,
            pipelineContext,
            scopedServiceProvider,
            cancellationToken)
        {
            ReadyTime = moduleState.ReadyTime ?? startTime,
        };

        try
        {
            await ExecuteModuleBodyAsync(
                    moduleState,
                    scheduler,
                    executionContext,
                    moduleContext,
                    lifecycleContext,
                    cancellationToken)
                .ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            await HandleModuleFailureAsync(moduleState, executionContext, lifecycleContext, ex).ConfigureAwait(false);
            throw;
        }
        finally
        {
            // Store execution context results in module state
            moduleState.TrySetSkipResult(executionContext.SkipResult);

            if (!_pipelineOptions.Value.Console.ShowProgress)
            {
                await _moduleDisposer.DisposeAsync(moduleState).ConfigureAwait(false);
            }
        }
    }

    private async Task ExecuteModuleBodyAsync(
        ModuleState moduleState,
        IModuleScheduler scheduler,
        ModuleExecutionContext executionContext,
        IModuleContext moduleContext,
        ModuleLifecycleContext lifecycleContext,
        CancellationToken cancellationToken)
    {
        // Invoke OnModuleReady lifecycle event (dependencies satisfied, about to execute)
        await _lifecycleEventInvoker.InvokeReadyEventAsync(lifecycleContext).ConfigureAwait(false);

        // Invoke OnModuleStart lifecycle event
        await _lifecycleEventInvoker.InvokeStartEventAsync(lifecycleContext).ConfigureAwait(false);

        // Execute through generated typed metadata when available.
        var result = await ExecuteTypedModule(
                moduleState.Module,
                scheduler,
                executionContext,
                moduleContext,
                (moduleResult, token) => FinalizeModuleAsync(
                    moduleState,
                    scheduler,
                    executionContext,
                    lifecycleContext,
                    moduleResult,
                    token),
                cancellationToken)
            .ConfigureAwait(false);

        PublishModuleResult(moduleState, executionContext, result);

        if (executionContext.Status == Enums.Status.Skipped)
        {
            await _mediator.Publish(
                    new ModuleSkippedNotification(moduleState, executionContext.SkipResult),
                    CancellationToken.None)
                .ConfigureAwait(false);
            return;
        }

        var isSuccessful = executionContext.Status is
            Enums.Status.Successful or Enums.Status.UsedHistory or Enums.Status.CachedResult;
        await _mediator.Publish(
                new ModuleCompletedNotification(moduleState, isSuccessful),
                CancellationToken.None)
            .ConfigureAwait(false);
    }

    private async Task HandleModuleFailureAsync(
        ModuleState moduleState,
        ModuleExecutionContext executionContext,
        ModuleLifecycleContext lifecycleContext,
        Exception exception)
    {
        executionContext.Exception = exception;
        if (executionContext.Status is Enums.Status.Successful
            or Enums.Status.UsedHistory
            or Enums.Status.CachedResult
            or Enums.Status.NotYetStarted
            or Enums.Status.Processing)
        {
            executionContext.Status = Enums.Status.Failed;
        }

        var result = executionContext.ExecutionTask.IsCompletedSuccessfully
            ? await executionContext.ExecutionTask.ConfigureAwait(false)
            : CreateFailureResult(moduleState.Module, moduleState.ModuleType, executionContext, exception);
        PublishModuleResult(moduleState, executionContext, result);

        try
        {
            // Invoke OnModuleFailed lifecycle event
            await _lifecycleEventInvoker.InvokeFailedEventAsync(lifecycleContext, exception).ConfigureAwait(false);

            await _pipelineSetupExecutor.OnModuleFailureAsync(moduleState).ConfigureAwait(false);
        }
        finally
        {
            await _mediator.Publish(new ModuleCompletedNotification(moduleState, false)).ConfigureAwait(false);
        }
    }

    private async Task FinalizeModuleAsync(
        ModuleState moduleState,
        IModuleScheduler scheduler,
        ModuleExecutionContext executionContext,
        ModuleLifecycleContext lifecycleContext,
        IModuleResult result,
        CancellationToken cancellationToken)
    {
        moduleState.Result = result;

        if (executionContext.Status == Enums.Status.Skipped)
        {
            await _lifecycleEventInvoker.InvokeSkippedEventAsync(
                    lifecycleContext,
                    Enums.Status.Skipped,
                    executionContext.SkipResult!)
                .ConfigureAwait(false);
            await _pipelineSetupExecutor.OnModuleSkippedAsync(moduleState).ConfigureAwait(false);
            return;
        }

        if (executionContext.Status is Enums.Status.Successful or Enums.Status.IgnoredFailure)
        {
            await _moduleEstimatedTimeProvider.SaveModuleTimeAsync(
                    moduleState.ModuleType,
                    executionContext.Duration)
                .ConfigureAwait(false);
        }

        await _pipelineSetupExecutor.OnModuleEndAsync(moduleState).ConfigureAwait(false);
        await _lifecycleEventInvoker.InvokeEndEventAsync(lifecycleContext, executionContext.Status, result).ConfigureAwait(false);

        if (!_manageArtifactsLocally
            || executionContext.Status is not (
                Enums.Status.Successful or Enums.Status.UsedHistory or Enums.Status.CachedResult))
        {
            return;
        }

        try
        {
            await UploadProducedArtifactsAsync(moduleState.ModuleType, scheduler, cancellationToken)
                .ConfigureAwait(false);
        }
        catch (Exception exception) when (exception is not ModuleFailedException)
        {
            throw new ModuleFailedException(moduleState.ModuleType, exception);
        }
    }

    private void PublishModuleResult(
        ModuleState moduleState,
        ModuleExecutionContext executionContext,
        IModuleResult result)
    {
        moduleState.Result = result;
        executionContext.SetResult(result);
        _resultRegistry.RegisterResult(moduleState.ModuleType, result);

        if (GeneratedModuleMetadata.TryGetRuntime(moduleState.ModuleType, out var runtime))
        {
            runtime.SetCompletionSource(moduleState.Module, result);
            return;
        }

        CompletionSourceSetterCache.GetOrCreate(moduleState.Module.ResultType)(moduleState.Module, result);
    }

    private static IModuleResult CreateFailureResult(
        IModule module,
        Type moduleType,
        ModuleExecutionContext executionContext,
        Exception exception)
    {
        return GeneratedModuleMetadata.TryGetRuntime(moduleType, out var runtime)
            ? runtime.CreateFailure(exception, executionContext)
            : ModuleResultFactory.CreateException(module.ResultType, exception, executionContext);
    }

    private ModuleExecutionContext CreateExecutionContext(IModule module, Type moduleType)
    {
        // Use compiled delegate factory instead of Activator.CreateInstance
        return ExecutionContextFactory.Create(module, moduleType);
    }

    private void ApplyDependencySkip(ModuleState moduleState, ModuleExecutionContext executionContext)
    {
        if (moduleState.SkipResult.ShouldSkip)
        {
            executionContext.SkipResult = moduleState.SkipResult;
            return;
        }

        var skippedDependencies = moduleState.Dependencies
            .Where(dependency => !dependency.Value)
            .Select(dependency => (
                Type: dependency.Key,
                Result: _resultRegistry.GetResult(dependency.Key)))
            .Where(dependency => dependency.Result?.ModuleStatus == Enums.Status.Skipped)
            .OrderBy(dependency => dependency.Type.FullName, StringComparer.Ordinal)
            .ToArray();

        if (skippedDependencies.Length == 0)
        {
            return;
        }

        executionContext.SkipResult = DependencySkipDecisionFactory.Create(
            skippedDependencies
                .Select(dependency => (
                    ModuleType: dependency.Type,
                    SkipDecision: dependency.Result!.SkipDecisionOrDefault))
                .ToArray());
    }

    private async Task<IModuleResult> ExecuteTypedModule(
        IModule module,
        IModuleScheduler scheduler,
        ModuleExecutionContext executionContext,
        IModuleContext moduleContext,
        Func<IModuleResult, CancellationToken, Task> finalizeExecutionAsync,
        CancellationToken cancellationToken)
    {
        Func<CancellationToken, Task>? prepareExecutionAsync = _manageArtifactsLocally
            ? token => _artifactLifecycleManager.DownloadConsumedArtifactsAsync(
                module.GetType(),
                failIfMissing: true,
                token)
            : null;
        if (GeneratedModuleMetadata.TryGetRuntime(module.GetType(), out var runtime))
        {
            return await runtime.ExecuteAsync(
                    _executionPipeline,
                    module,
                    executionContext,
                    moduleContext,
                    prepareExecutionAsync,
                    finalizeExecutionAsync,
                    completeModule: false,
                    cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        // Dynamic/plugin modules retain the annotated reflection fallback.
        var executor = ModuleExecutionDelegateFactory.GetExecutor(module.ResultType);
        return await executor(
                _executionPipeline,
                module,
                executionContext,
                moduleContext,
                prepareExecutionAsync,
                finalizeExecutionAsync,
                completeModule: false,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    [UnconditionalSuppressMessage(
        "AOT",
        "IL3050",
        Justification = "Generated runtime metadata handles statically known modules; MakeGenericType is the documented fallback for dynamic modules.")]
    private static IModuleLogger GetModuleLogger(
        IServiceProvider serviceProvider,
        Type moduleType)
    {
        if (GeneratedModuleMetadata.TryGetRuntime(moduleType, out var runtime))
        {
            return runtime.GetLogger(serviceProvider);
        }

        var loggerType = typeof(ModuleLogger<>).MakeGenericType(moduleType);
        return (IModuleLogger) serviceProvider.GetRequiredService(loggerType);
    }
}
