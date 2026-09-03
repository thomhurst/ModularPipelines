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
using ModularPipelines.Generated;
using ModularPipelines.Helpers;
using ModularPipelines.Logging;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using ModularPipelines.Secrets;
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
    private readonly EngineCancellationToken _engineCancellationToken;
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
        EngineCancellationToken engineCancellationToken,
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
        _engineCancellationToken = engineCancellationToken;
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
        _manageArtifactsLocally = !distributedOptions.Value.Enabled;
        var registeredModules = modules.ToArray();
        _registeredModuleTypes = registeredModules.Select(static module => module.GetType()).ToArray();
        _localArtifactConsumers = GetLocalArtifactConsumers(registeredModules);
    }

    /// <inheritdoc />
    public Task ExecuteAsync(ModuleState moduleState, CancellationToken cancellationToken)
    {
        return ExecuteCore(moduleState, GetScheduler(moduleState, skipDependencyWait: false), cancellationToken, skipDependencyWait: false);
    }

    /// <inheritdoc />
    public Task ExecuteWithoutDependencyWaitAsync(ModuleState moduleState, CancellationToken cancellationToken)
    {
        return ExecuteCore(moduleState, GetScheduler(moduleState, skipDependencyWait: true), cancellationToken, skipDependencyWait: true);
    }

    private async Task ExecuteCore(
        ModuleState moduleState,
        IModuleScheduler? scheduler,
        CancellationToken cancellationToken,
        bool skipDependencyWait)
    {
        var module = moduleState.Module;
        var moduleType = moduleState.ModuleType;
        var moduleName = moduleType.Name;
        var limiterCancellationToken = default(CancellationToken);
        IInternalModuleLogger? readyLogger = null;

        // Create a scope to resolve scoped services like IModuleContext and ModuleLogger<T>
        var scope = _serviceProvider.CreateAsyncScope();
        await using (scope.ConfigureAwait(false))
        {
            try
            {
                if (!skipDependencyWait)
                {
                    await _dependencyWaiter.WaitForDependenciesAsync(
                            moduleState,
                            scheduler!,
                            scope.ServiceProvider,
                            cancellationToken)
                        .ConfigureAwait(false);
                }
                else
                {
                    _logger.LogDebug("Skipping dependency wait for late-started AlwaysRun module: {ModuleName}", moduleName);
                }

                var allowHistoricalResultWhenSkipped = !await HasRunnableArtifactConsumerAsync(
                        moduleType,
                        scheduler,
                        cancellationToken)
                    .ConfigureAwait(false);

                if (moduleState.TryStartReadyEvents())
                {
                    var pipelineContext = scope.ServiceProvider.GetRequiredService<IPipelineContext>();
                    var readyLifecycleContext = CreateLifecycleContext(
                        moduleState,
                        pipelineContext,
                        scope.ServiceProvider,
                        cancellationToken);
                    readyLogger = readyLifecycleContext.ConsoleWriter as IInternalModuleLogger;
                    await _pipelineSetupExecutor
                        .OnModuleReadyAsync(moduleState, readyLifecycleContext.ConsoleWriter)
                        .ConfigureAwait(false);
                    await InvokeReadyEventAsync(moduleState, readyLifecycleContext).ConfigureAwait(false);
                }

                using var limiterCancellationTokenSource = module.Configuration.AlwaysRun
                    ? null
                    : CancellationTokenSource.CreateLinkedTokenSource(
                        cancellationToken,
                        _engineCancellationToken.Token);
                limiterCancellationToken = module.Configuration.AlwaysRun
                    ? _engineCancellationToken.NonFailureCancellationToken
                    : limiterCancellationTokenSource!.Token;
                using var semaphoreHandle = await _parallelLimitHandler
                    .AcquireParallelLimitAsync(moduleType, limiterCancellationToken)
                    .ConfigureAwait(false);
                using var executionHintHandle = await _parallelLimitHandler
                    .AcquireExecutionHintLimitAsync(moduleState, limiterCancellationToken)
                    .ConfigureAwait(false);

                // Check constraints again after acquiring execution slots. Keeping the module queued
                // until this point prevents limiter wait time from being reported as execution time.
                if (!TryMarkModuleStarted(scheduler, moduleType))
                {
                    readyLogger ??= GetAmbientOrScopedModuleLogger(
                        scope.ServiceProvider,
                        moduleType) as IInternalModuleLogger;
                    readyLogger?.PreserveBufferForDeferredExecution();
                    _logger.LogDebug("Module {ModuleName} deferred due to constraint check failure", moduleName);
                    return; // Module will be rescheduled by the scheduler
                }

                _logger.LogDebug("Starting module {ModuleName}", moduleName);
                var executionContext = CreateExecutionContext(module, moduleType);
                ApplyDependencySkip(moduleState, executionContext);
                executionContext.AllowHistoricalResultWhenSkipped = allowHistoricalResultWhenSkipped;

                await ExecuteModuleWithPipeline(
                        moduleState,
                        scheduler,
                        scope.ServiceProvider,
                        executionContext,
                        cancellationToken)
                    .ConfigureAwait(false);

                scheduler?.MarkModuleCompleted(moduleType, true, statusOverride: moduleState.Result?.Status);
            }
            catch (Exception ex)
            {
                var handledException = NormalizeLimiterCancellation(
                    ex,
                    cancellationToken,
                    limiterCancellationToken);
                HandleExecutionFailure(
                    moduleState,
                    scheduler,
                    handledException,
                    cancellationToken);
                readyLogger ??= GetAmbientOrScopedModuleLogger(
                    scope.ServiceProvider,
                    moduleType) as IInternalModuleLogger;
                FinalizeReadyLoggerAfterFailure(
                    readyLogger,
                    moduleState,
                    moduleType,
                    handledException);

                if (_pipelineOptions.Value.FailureMode == FailureMode.FailFast)
                {
                    if (ReferenceEquals(handledException, ex))
                    {
                        throw;
                    }

                    throw handledException;
                }
            }
        }
    }

    private static IModuleScheduler? GetScheduler(ModuleState moduleState, bool skipDependencyWait)
    {
        if (!skipDependencyWait && moduleState.Scheduler is null)
        {
            throw new InvalidOperationException("Locally planned module execution requires an engine scheduler.");
        }

        return moduleState.Scheduler;
    }

    private static bool TryMarkModuleStarted(IModuleScheduler? scheduler, Type moduleType)
    {
        return scheduler?.MarkModuleStarted(moduleType) ?? true;
    }

    private void FinalizeReadyLoggerAfterFailure(
        IInternalModuleLogger? readyLogger,
        ModuleState moduleState,
        Type moduleType,
        Exception exception)
    {
        if (readyLogger is null)
        {
            return;
        }

        readyLogger.SetException(exception);
        readyLogger.SetStatus(
            moduleState.Result?.Status
            ?? _resultRegistry.GetResult(moduleType)?.Status
            ?? ModuleStatus.Failed);
    }

    internal static Exception NormalizeLimiterCancellation(
        Exception exception,
        CancellationToken workerCancellationToken,
        CancellationToken limiterCancellationToken)
    {
        if (limiterCancellationToken.IsCancellationRequested
            && exception is OperationCanceledException operationCanceledException
            && operationCanceledException.CancellationToken == limiterCancellationToken
            && limiterCancellationToken != workerCancellationToken)
        {
            return new NormalizedWorkerCancellationException(
                operationCanceledException.Message,
                operationCanceledException,
                workerCancellationToken);
        }

        return exception;
    }

    private async Task InvokeReadyEventAsync(
        ModuleState moduleState,
        ModuleLifecycleContext lifecycleContext)
    {
        try
        {
            await _lifecycleEventInvoker.InvokeReadyEventAsync(lifecycleContext).ConfigureAwait(false);
        }
        catch (Exception exception)
        {
            var executionContext = CreateExecutionContext(moduleState.Module, moduleState.ModuleType);
            ApplyDependencySkip(moduleState, executionContext);

            try
            {
                await HandleModuleFailureAsync(moduleState, executionContext, lifecycleContext, exception)
                    .ConfigureAwait(false);
            }
            finally
            {
                await CompleteModuleLifecycleAsync(moduleState, executionContext).ConfigureAwait(false);
            }

            throw;
        }
    }

    private void HandleExecutionFailure(
        ModuleState moduleState,
        IModuleScheduler? scheduler,
        Exception exception,
        CancellationToken workerCancellationToken)
    {
        var module = moduleState.Module;
        var moduleType = moduleState.ModuleType;
        var isDependencyFailure = exception is DependencyFailedException;
        var isPipelineCancellation = IsPipelineCancellation(
            exception,
            workerCancellationToken,
            _engineCancellationToken.IsCancelled);
        var registeredResult = _resultRegistry.GetResult(moduleType);
        var completionException = GetCompletionException(
            exception,
            isPipelineCancellation,
            registeredResult?.ExceptionOrDefault);
        if (isPipelineCancellation)
        {
            _logger.LogInformation(
                "Pipeline cancellation stopped module {ModuleName} before execution",
                moduleType.Name);
        }
        else
        {
            LogModuleFailure(_logger, moduleType.Name, exception);
        }

        var statusOverride = GetStatusOverride(
            isDependencyFailure,
            isPipelineCancellation,
            registeredResult?.Status);
        scheduler?.MarkModuleCompleted(
            moduleType,
            false,
            completionException,
            statusOverride);

        if (moduleState.Result is not null || registeredResult is not null)
        {
            return;
        }

        if (isDependencyFailure)
        {
            _resultRegistrar.RegisterDependencyFailedResult(module, moduleType, exception);
        }
        else
        {
            _resultRegistrar.RegisterTerminatedResult(module, moduleType, completionException);
        }
    }

    internal static bool IsPipelineCancellation(
        Exception exception,
        CancellationToken workerCancellationToken,
        bool isEngineCancelled) =>
        exception is OperationCanceledException
        && (isEngineCancelled
            || WorkerCancellationClassifier.IsExpected(exception, workerCancellationToken));

    private Exception GetCompletionException(
        Exception exception,
        bool isPipelineCancellation,
        Exception? registeredException) =>
        isPipelineCancellation
            ? registeredException ?? _engineCancellationToken.OriginalException ?? exception
            : exception;

    private static ModuleStatus? GetStatusOverride(
        bool isDependencyFailure,
        bool isPipelineCancellation,
        ModuleStatus? registeredStatus)
    {
        if (isDependencyFailure)
        {
            return ModuleStatus.DependencyFailed;
        }

        return isPipelineCancellation
            ? registeredStatus ?? ModuleStatus.Cancelled
            : null;
    }

    internal static void LogModuleFailure(
        ILogger logger,
        string moduleName,
        Exception exception)
    {
        switch (exception)
        {
            case DependencyFailedException dependencyFailedException:
                logger.LogInformation(
                    "Module {ModuleName} did not run because dependency {FailingModuleName} failed",
                    moduleName,
                    dependencyFailedException.FailingModuleName);
                break;
            case ModuleFailedException { WasLogged: true }:
                logger.LogDebug(
                    "Module {ModuleName} failure was recorded in its module output",
                    moduleName);
                break;
            default:
                logger.LogError(exception, "Module {ModuleName} failed", moduleName);
                break;
        }
    }

    private async Task UploadProducedArtifactsAsync(
        Type moduleType,
        IModuleScheduler? scheduler,
        CancellationToken cancellationToken)
    {
        if (!_manageArtifactsLocally
            || !_localArtifactConsumers.TryGetValue(moduleType, out var consumersByArtifact))
        {
            return;
        }

        var demandPlan = await GetArtifactDemandPlanAsync(
                scheduler ?? throw new InvalidOperationException("Local artifact management requires an engine scheduler."),
                cancellationToken)
            .ConfigureAwait(false);
        if (!demandPlan.RequiredArtifactsByProducer.TryGetValue(moduleType, out var requiredArtifactNames)
            || requiredArtifactNames.Count == 0)
        {
            return;
        }

        var demandedArtifactNames = new HashSet<string>(StringComparer.Ordinal);
        var runnableConsumersByArtifact = new Dictionary<string, List<Type>>(StringComparer.Ordinal);
        foreach (var (artifactName, consumerTypes) in consumersByArtifact
                     .Where(pair => requiredArtifactNames.Contains(pair.Key)))
        {
            foreach (var consumerType in consumerTypes)
            {
                var demand = await GetArtifactConsumerDemandAsync(
                        consumerType,
                        scheduler,
                        cancellationToken,
                        demandPlan.RequiredProducerTypes,
                        moduleType)
                    .ConfigureAwait(false);
                if (demand == ArtifactConsumerDemand.NotRunnable)
                {
                    continue;
                }

                demandedArtifactNames.Add(artifactName);
                if (demand != ArtifactConsumerDemand.Runnable)
                {
                    continue;
                }

                if (!runnableConsumersByArtifact.TryGetValue(artifactName, out var runnableConsumers))
                {
                    runnableConsumers = [];
                    runnableConsumersByArtifact.Add(artifactName, runnableConsumers);
                }

                runnableConsumers.Add(consumerType);
            }
        }

        if (demandedArtifactNames.Count == 0)
        {
            return;
        }

        var uploadedArtifacts = await _artifactLifecycleManager
            .UploadProducedArtifactsAsync(moduleType, demandedArtifactNames, cancellationToken)
            .ConfigureAwait(false);

        ThrowIfRequiredArtifactsWereNotProduced(
            moduleType,
            uploadedArtifacts,
            runnableConsumersByArtifact);
    }

    private static void ThrowIfRequiredArtifactsWereNotProduced(
        Type moduleType,
        IReadOnlyList<ArtifactReference> uploadedArtifacts,
        Dictionary<string, List<Type>> runnableConsumersByArtifact)
    {
        var uploadedArtifactNames = uploadedArtifacts
            .Select(artifact => artifact.Name)
            .ToHashSet(StringComparer.Ordinal);
        var missingArtifacts = moduleType
            .GetCustomAttributes(typeof(ProducesArtifactAttribute), inherit: true)
            .Cast<ProducesArtifactAttribute>()
            .Where(attribute => runnableConsumersByArtifact.ContainsKey(attribute.Name)
                                && !uploadedArtifactNames.Contains(attribute.Name))
            .OrderBy(attribute => attribute.Name, StringComparer.Ordinal)
            .ToList();

        if (missingArtifacts.Count == 0)
        {
            return;
        }

        var details = missingArtifacts.Select(attribute =>
        {
            var consumers = runnableConsumersByArtifact[attribute.Name]
                .Select(type => type.Name)
                .OrderBy(name => name, StringComparer.Ordinal);
            return $"Artifact '{attribute.Name}' matched no files for pattern '{attribute.PathPattern}'. "
                   + $"Runnable consumers: {string.Join(", ", consumers)}.";
        });

        throw new InvalidOperationException(
            $"Module '{moduleType.Name}' did not produce required artifacts:{Environment.NewLine}"
            + string.Join(Environment.NewLine, details));
    }

    private async Task<bool> HasRunnableArtifactConsumerAsync(
        Type producerType,
        IModuleScheduler? scheduler,
        CancellationToken cancellationToken)
    {
        if (!_manageArtifactsLocally || !_localArtifactConsumers.ContainsKey(producerType))
        {
            return false;
        }

        var demandPlan = await GetArtifactDemandPlanAsync(
                scheduler ?? throw new InvalidOperationException("Local artifact management requires an engine scheduler."),
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
                            if (await GetArtifactConsumerDemandAsync(
                                    consumerType,
                                    scheduler,
                                    cancellationToken,
                                    currentDemand)
                                .ConfigureAwait(false) != ArtifactConsumerDemand.NotRunnable)
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
                            if (await GetArtifactConsumerDemandAsync(
                                    consumerType,
                                    scheduler,
                                    cancellationToken,
                                    requiredProducerTypes)
                                .ConfigureAwait(false) == ArtifactConsumerDemand.NotRunnable)
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

    private async Task<ArtifactConsumerDemand> GetArtifactConsumerDemandAsync(
        Type consumerType,
        IModuleScheduler scheduler,
        CancellationToken cancellationToken,
        IReadOnlySet<Type> requiredProducerTypes,
        Type? producerTypeBeingFinalized = null)
    {
        if (scheduler.GetModuleState(consumerType) is not { State: not ModuleExecutionState.Completed } moduleState
            || moduleState.SkipResult.ShouldSkip)
        {
            return ArtifactConsumerDemand.NotRunnable;
        }

        var dependencyDemand = await GetRequiredDependencyDemandAsync(
                moduleState,
                scheduler,
                cancellationToken,
                [consumerType],
                requiredProducerTypes,
                producerTypeBeingFinalized)
            .ConfigureAwait(false);
        if (dependencyDemand.IsUnrecoverable)
        {
            return ArtifactConsumerDemand.NotRunnable;
        }

        if (dependencyDemand.HasPendingDependency)
        {
            return ArtifactConsumerDemand.Pending;
        }

        var skipDecision = await EvaluatePlanningSkipAsync(moduleState, cancellationToken).ConfigureAwait(false);
        if (skipDecision?.ShouldSkip != true)
        {
            return ArtifactConsumerDemand.Runnable;
        }

        moduleState.TrySetSkipResult(skipDecision);
        return ArtifactConsumerDemand.NotRunnable;
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
        IReadOnlySet<Type> requiredProducerTypes,
        Type? dependencyTypeBeingFinalized = null)
    {
        var hasPendingDependency = false;
        foreach (var dependency in moduleState.Dependencies.Where(
                     dependency => !dependency.Value && dependency.Key != dependencyTypeBeingFinalized))
        {
            var dependencyDemand = await GetRequiredDependencyDemandAsync(
                    dependency.Key,
                    scheduler,
                    cancellationToken,
                    visited,
                    requiredProducerTypes,
                    dependencyTypeBeingFinalized)
                .ConfigureAwait(false);
            hasPendingDependency |= dependencyDemand.HasPendingDependency;
            if (dependencyDemand.IsUnrecoverable)
            {
                return new RequiredDependencyDemand(
                    IsUnrecoverable: true,
                    HasPendingDependency: hasPendingDependency);
            }
        }

        return new RequiredDependencyDemand(
            IsUnrecoverable: false,
            HasPendingDependency: hasPendingDependency);
    }

    private async Task<RequiredDependencyDemand> GetRequiredDependencyDemandAsync(
        Type dependencyType,
        IModuleScheduler scheduler,
        CancellationToken cancellationToken,
        HashSet<Type> visited,
        IReadOnlySet<Type> requiredProducerTypes,
        Type? dependencyTypeBeingFinalized)
    {
        if (_resultRegistry.GetResult(dependencyType)?.Status == ModuleStatus.Skipped)
        {
            return new RequiredDependencyDemand(IsUnrecoverable: true, HasPendingDependency: false);
        }

        if (scheduler.GetModuleState(dependencyType) is not { } dependencyState
            || dependencyState.State == ModuleExecutionState.Completed
            || !visited.Add(dependencyType))
        {
            return default;
        }

        try
        {
            if (dependencyState.SkipResult.ShouldSkip)
            {
                return new RequiredDependencyDemand(
                    IsUnrecoverable: await IsSkippedDependencyUnrecoverableAsync(
                            dependencyType,
                            dependencyState,
                            requiredProducerTypes)
                        .ConfigureAwait(false),
                    HasPendingDependency: false);
            }

            var transitiveDemand = await GetRequiredDependencyDemandAsync(
                    dependencyState,
                    scheduler,
                    cancellationToken,
                    visited,
                    requiredProducerTypes,
                    dependencyTypeBeingFinalized)
                .ConfigureAwait(false);
            if (transitiveDemand.IsUnrecoverable)
            {
                return new RequiredDependencyDemand(IsUnrecoverable: true, HasPendingDependency: true);
            }

            if (transitiveDemand.HasPendingDependency)
            {
                return transitiveDemand;
            }

            var skipDecision = await EvaluatePlanningSkipAsync(dependencyState, cancellationToken)
                .ConfigureAwait(false);
            if (skipDecision?.ShouldSkip != true)
            {
                return new RequiredDependencyDemand(IsUnrecoverable: false, HasPendingDependency: true);
            }

            dependencyState.TrySetSkipResult(skipDecision);
            return new RequiredDependencyDemand(
                IsUnrecoverable: await IsSkippedDependencyUnrecoverableAsync(
                        dependencyType,
                        dependencyState,
                        requiredProducerTypes)
                    .ConfigureAwait(false),
                HasPendingDependency: false);
        }
        finally
        {
            visited.Remove(dependencyType);
        }
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

    private enum ArtifactConsumerDemand
    {
        NotRunnable,
        Pending,
        Runnable,
    }

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
        IModuleScheduler? scheduler,
        IServiceProvider scopedServiceProvider,
        ModuleExecutionContext executionContext,
        CancellationToken cancellationToken)
    {
        var module = moduleState.Module;
        var moduleType = moduleState.ModuleType;

        var pipelineContext = scopedServiceProvider.GetRequiredService<IPipelineContext>();

        // Create module-specific context
        var logger = GetAmbientOrScopedModuleLogger(scopedServiceProvider, moduleType);
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

        // Keep logging and raw console attribution in one scoped ambient context.
        using var outputScope = new ModuleOutputContextScope(moduleType, logger);

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
            if (executionContext.Status == ModuleStatus.Skipped)
            {
                telemetryStatus = "Skipped";
                ModuleActivityTracing.RecordSkipped(activity);
            }
            else if (executionContext.Status == ModuleStatus.FailureIgnored)
            {
                telemetryStatus = ModuleStatus.FailureIgnored.ToString();
                activity?.SetTag(ModuleActivityTracing.ModuleStatusTag, telemetryStatus);
                activity?.SetStatus(ActivityStatusCode.Ok, "Module failed but failure was ignored");
            }
            else if (executionContext.Status == ModuleStatus.RestoredFromHistory)
            {
                telemetryStatus = ModuleStatus.RestoredFromHistory.ToString();
                ModuleActivityTracing.RecordRestoredFromHistory(activity);
            }
            else if (executionContext.Status == ModuleStatus.RestoredFromCache)
            {
                telemetryStatus = ModuleStatus.RestoredFromCache.ToString();
                ModuleActivityTracing.RecordRestoredFromCache(activity);
            }
            else if (executionContext.Status == ModuleStatus.Cancelled)
            {
                telemetryStatus = ModuleStatus.Cancelled.ToString();
                ModuleActivityTracing.RecordCancelled(activity);
            }
            else
            {
                telemetryStatus = ModuleStatus.Succeeded.ToString();
                ModuleActivityTracing.RecordSuccess(activity);
            }
        }
        catch (Exception ex)
        {
            var obfuscatedMessage = _secretObfuscator.Obfuscate(ex.Message, null);
            if (executionContext.Status == ModuleStatus.TimedOut)
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
        IModuleScheduler? scheduler,
        IServiceProvider scopedServiceProvider,
        IPipelineContext pipelineContext,
        ModuleExecutionContext executionContext,
        IModuleContext moduleContext,
        CancellationToken cancellationToken)
    {
        var module = moduleState.Module;
        var moduleType = moduleState.ModuleType;

        // Before module hooks - module is starting execution.
        var lifecycleContext = CreateLifecycleContext(
            moduleState,
            pipelineContext,
            scopedServiceProvider,
            cancellationToken);

        await _pipelineSetupExecutor
            .OnModuleStartAsync(moduleState, lifecycleContext.ConsoleWriter)
            .ConfigureAwait(false);

        var estimatedDuration = await _moduleEstimatedTimeProvider.GetModuleEstimatedTimeAsync(moduleType).ConfigureAwait(false);
        await _mediator.Publish(
                new ModuleStartedNotification(moduleState, estimatedDuration),
                CancellationToken.None)
            .ConfigureAwait(false);

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
            await CompleteModuleLifecycleAsync(moduleState, executionContext).ConfigureAwait(false);
        }
    }

    private async Task CompleteModuleLifecycleAsync(
        ModuleState moduleState,
        ModuleExecutionContext executionContext)
    {
        moduleState.TrySetSkipResult(executionContext.SkipResult);
        if (!_pipelineOptions.Value.Console.ShowProgress)
        {
            await _moduleDisposer.DisposeAsync(moduleState).ConfigureAwait(false);
        }
    }

    private async Task ExecuteModuleBodyAsync(
        ModuleState moduleState,
        IModuleScheduler? scheduler,
        ModuleExecutionContext executionContext,
        IModuleContext moduleContext,
        ModuleLifecycleContext lifecycleContext,
        CancellationToken cancellationToken)
    {
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

        if (executionContext.Status == ModuleStatus.Skipped)
        {
            await _mediator.Publish(
                    new ModuleSkippedNotification(moduleState, executionContext.SkipResult),
                    CancellationToken.None)
                .ConfigureAwait(false);
            return;
        }

        var isSuccessful = executionContext.Status is
            ModuleStatus.Succeeded or ModuleStatus.RestoredFromHistory or ModuleStatus.RestoredFromCache;
        await _mediator.Publish(
                new ModuleCompletedNotification(moduleState, isSuccessful),
                CancellationToken.None)
            .ConfigureAwait(false);
    }

    private ModuleLifecycleContext CreateLifecycleContext(
        ModuleState moduleState,
        IPipelineContext pipelineContext,
        IServiceProvider scopedServiceProvider,
        CancellationToken cancellationToken)
    {
        var startTime = DateTimeOffset.UtcNow;
        return new ModuleLifecycleContext(
            moduleState.Module,
            moduleState.ModuleType,
            _moduleAttributeEventService.GetAttributes(moduleState.ModuleType),
            startTime,
            pipelineContext,
            GetAmbientOrScopedModuleLogger(scopedServiceProvider, moduleState.ModuleType) as IConsoleWriter
                ?? pipelineContext.Console,
            cancellationToken)
        {
            ReadyTime = moduleState.ReadyTime ?? startTime,
        };
    }

    private async Task HandleModuleFailureAsync(
        ModuleState moduleState,
        ModuleExecutionContext executionContext,
        ModuleLifecycleContext lifecycleContext,
        Exception exception)
    {
        executionContext.Exception = exception;
        if (executionContext.Status is ModuleStatus.Succeeded
            or ModuleStatus.RestoredFromHistory
            or ModuleStatus.RestoredFromCache
            or ModuleStatus.NotStarted
            or ModuleStatus.Running)
        {
            executionContext.Status = ModuleStatus.Failed;
        }

        var result = executionContext.ExecutionTask.IsCompletedSuccessfully
            ? await executionContext.ExecutionTask.ConfigureAwait(false)
            : CreateFailureResult(moduleState.Module, moduleState.ModuleType, executionContext, exception);
        PublishModuleResult(moduleState, executionContext, result);

        try
        {
            await _lifecycleEventInvoker.InvokeFailedEventAsync(lifecycleContext, result, exception).ConfigureAwait(false);
            await _pipelineSetupExecutor
                .OnModuleFailureAsync(moduleState, exception, lifecycleContext.ConsoleWriter)
                .ConfigureAwait(false);
        }
        finally
        {
            await _mediator.Publish(new ModuleCompletedNotification(moduleState, false)).ConfigureAwait(false);
        }
    }

    private async Task FinalizeModuleAsync(
        ModuleState moduleState,
        IModuleScheduler? scheduler,
        ModuleExecutionContext executionContext,
        ModuleLifecycleContext lifecycleContext,
        IModuleResult result,
        CancellationToken cancellationToken)
    {
        moduleState.Result = result;

        if (executionContext.Status == ModuleStatus.Skipped)
        {
            await _lifecycleEventInvoker.InvokeSkippedEventAsync(
                    lifecycleContext,
                    ModuleStatus.Skipped,
                    executionContext.SkipResult!)
                .ConfigureAwait(false);
            await _pipelineSetupExecutor
                .OnModuleSkippedAsync(
                    moduleState,
                    executionContext.SkipResult!,
                    lifecycleContext.ConsoleWriter)
                .ConfigureAwait(false);
            return;
        }

        if (executionContext.Status is ModuleStatus.Succeeded or ModuleStatus.FailureIgnored)
        {
            await _moduleEstimatedTimeProvider.SaveModuleTimeAsync(
                    moduleState.ModuleType,
                    executionContext.Duration)
                .ConfigureAwait(false);
        }

        await _pipelineSetupExecutor
            .OnModuleEndAsync(moduleState, result, lifecycleContext.ConsoleWriter)
            .ConfigureAwait(false);
        await _lifecycleEventInvoker.InvokeEndEventAsync(lifecycleContext, executionContext.Status, result).ConfigureAwait(false);

        if (!_manageArtifactsLocally
            || executionContext.Status is not (
                ModuleStatus.Succeeded or ModuleStatus.RestoredFromHistory or ModuleStatus.RestoredFromCache))
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
            .Where(dependency => dependency.Result?.Status == ModuleStatus.Skipped)
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
        IModuleScheduler? scheduler,
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

    private static IModuleLogger GetAmbientOrScopedModuleLogger(
        IServiceProvider serviceProvider,
        Type moduleType) =>
        AmbientModuleOutputContext.Current is { } outputContext
        && outputContext.ModuleType == moduleType
            ? outputContext.Logger ?? GetModuleLogger(serviceProvider, moduleType)
            : GetModuleLogger(serviceProvider, moduleType);
}

internal sealed class NormalizedWorkerCancellationException(
    string? message,
    OperationCanceledException innerException,
    CancellationToken workerCancellationToken)
    : OperationCanceledException(message, innerException, workerCancellationToken);
