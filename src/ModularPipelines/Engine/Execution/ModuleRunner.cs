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
using ModularPipelines.Events;
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
    private readonly IModuleConditionHandler _moduleConditionHandler;
    private readonly ArtifactLifecycleManager _artifactLifecycleManager;
    private readonly bool _manageArtifactsLocally;
    private readonly IReadOnlyDictionary<Type, IReadOnlyDictionary<string, IReadOnlySet<Type>>>
        _localArtifactConsumers;
    private readonly ConcurrentDictionary<Type, Lazy<Task<SkipDecision?>>>
        _artifactConsumerSkipEvaluations = new();

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
        IModuleConditionHandler moduleConditionHandler,
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
        _moduleConditionHandler = moduleConditionHandler;
        _artifactLifecycleManager = artifactLifecycleManager;
        _manageArtifactsLocally = !distributedOptions.Value.Enabled
                                  || distributedOptions.Value.TotalInstances <= 1;
        _localArtifactConsumers = GetLocalArtifactConsumers(modules);
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
                        scope.ServiceProvider,
                        executionContext,
                        cancellationToken)
                    .ConfigureAwait(false);

                if (moduleState.Result?.ModuleStatus is Enums.Status.Successful or Enums.Status.UsedHistory)
                {
                    await UploadProducedArtifactsAsync(moduleType, scheduler, cancellationToken).ConfigureAwait(false);
                }

                scheduler.MarkModuleCompleted(moduleType, true, statusOverride: moduleState.Result?.ModuleStatus);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Module {ModuleName} failed", moduleName);
                scheduler.MarkModuleCompleted(moduleType, false, ex);

                // Register a PipelineTerminated result for this module if no result was registered yet
                if (moduleState.Result == null)
                {
                    _resultRegistrar.RegisterTerminatedResult(module, moduleType, ex);
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
            || !_localArtifactConsumers.TryGetValue(moduleType, out var consumersByArtifact))
        {
            return;
        }

        var artifactNames = new HashSet<string>(StringComparer.Ordinal);
        foreach (var (artifactName, consumerTypes) in consumersByArtifact)
        {
            foreach (var consumerType in consumerTypes)
            {
                if (await IsRunnableArtifactConsumerAsync(
                        consumerType,
                        scheduler,
                        cancellationToken)
                    .ConfigureAwait(false))
                {
                    artifactNames.Add(artifactName);
                    break;
                }
            }
        }

        if (artifactNames.Count == 0)
        {
            return;
        }

        try
        {
            await _artifactLifecycleManager
                .UploadProducedArtifactsAsync(moduleType, artifactNames, cancellationToken)
                .ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to upload artifacts for module {Module}", moduleType.Name);
        }
    }

    private async Task<bool> HasRunnableArtifactConsumerAsync(
        Type producerType,
        IModuleScheduler scheduler,
        CancellationToken cancellationToken)
    {
        if (!_localArtifactConsumers.TryGetValue(producerType, out var consumersByArtifact))
        {
            return false;
        }

        foreach (var consumerType in consumersByArtifact.Values.SelectMany(consumers => consumers))
        {
            if (await IsRunnableArtifactConsumerAsync(
                    consumerType,
                    scheduler,
                    cancellationToken)
                .ConfigureAwait(false))
            {
                return true;
            }
        }

        return false;
    }

    private async Task<bool> IsRunnableArtifactConsumerAsync(
        Type consumerType,
        IModuleScheduler scheduler,
        CancellationToken cancellationToken)
    {
        if (scheduler.GetModuleState(consumerType) is not { State: not ModuleExecutionState.Completed } moduleState
            || moduleState.SkipResult.ShouldSkip)
        {
            return false;
        }

        var evaluation = _artifactConsumerSkipEvaluations.GetOrAdd(
            consumerType,
            _ => new Lazy<Task<SkipDecision?>>(
                () => EvaluateArtifactConsumerSkipAsync(moduleState, cancellationToken),
                LazyThreadSafetyMode.ExecutionAndPublication));
        var skipDecision = await evaluation.Value.ConfigureAwait(false);
        if (skipDecision?.ShouldSkip != true)
        {
            return true;
        }

        moduleState.SkipResult = skipDecision;
        return false;
    }

    private async Task<SkipDecision?> EvaluateArtifactConsumerSkipAsync(
        ModuleState moduleState,
        CancellationToken cancellationToken)
    {
        var module = moduleState.Module;
        var (shouldIgnore, attributeDecision) = await _moduleConditionHandler
            .ShouldIgnoreForPlanning(module, cancellationToken)
            .ConfigureAwait(false);
        if (shouldIgnore)
        {
            return attributeDecision ?? SkipDecision.Skip("Module was ignored");
        }

        var planningSkipCondition = module.Configuration.PlanningSkipCondition;
        if (planningSkipCondition is null)
        {
            return null;
        }

        await using var scope = _serviceProvider.CreateAsyncScope();
        var scopedServiceProvider = scope.ServiceProvider;
        var executionContext = CreateExecutionContext(module, moduleState.ModuleType);
        try
        {
            var moduleContext = new ModuleContext(
                scopedServiceProvider.GetRequiredService<IPipelineContext>(),
                module,
                executionContext,
                GetModuleLogger(scopedServiceProvider, moduleState.ModuleType),
                _mediator,
                _moduleEstimatedTimeProvider,
                moduleResultAccessAllowed: false);
            using var planningResultAccess = PlanningModuleResultAccess.Enter();
            return await planningSkipCondition(moduleContext, cancellationToken).ConfigureAwait(false);
        }
        finally
        {
            executionContext.ModuleCancellationTokenSource.Dispose();
        }
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

        // Set up logging and module type context using scope wrapper for proper cleanup
        await using var loggerScope = new ModuleLoggerScope(logger, moduleType);

        try
        {
            await ExecuteModuleLifecycle(
                    moduleState,
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
        await _mediator.Publish(new ModuleStartedNotification(moduleState, estimatedDuration)).ConfigureAwait(false);

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
            // Invoke OnModuleReady lifecycle event (dependencies satisfied, about to execute)
            await _lifecycleEventInvoker.InvokeReadyEventAsync(lifecycleContext).ConfigureAwait(false);

            // Invoke OnModuleStart lifecycle event
            await _lifecycleEventInvoker.InvokeStartEventAsync(lifecycleContext).ConfigureAwait(false);

            // Execute through generated typed metadata when available.
            var result = await ExecuteTypedModule(
                    module,
                    executionContext,
                    moduleContext,
                    cancellationToken)
                .ConfigureAwait(false);

            moduleState.Result = result;
            _resultRegistry.RegisterResult(moduleType, result);

            if (executionContext.Status == Enums.Status.Skipped)
            {
                // Invoke OnModuleSkipped lifecycle event
                await _lifecycleEventInvoker.InvokeSkippedEventAsync(lifecycleContext, Enums.Status.Skipped, executionContext.SkipResult!).ConfigureAwait(false);

                await _pipelineSetupExecutor.OnModuleSkippedAsync(moduleState).ConfigureAwait(false);
                await _mediator.Publish(new ModuleSkippedNotification(moduleState, executionContext.SkipResult)).ConfigureAwait(false);
                return;
            }

            if (executionContext.Status is Enums.Status.Successful or Enums.Status.IgnoredFailure)
            {
                await _moduleEstimatedTimeProvider.SaveModuleTimeAsync(moduleType, executionContext.Duration).ConfigureAwait(false);
            }

            await _pipelineSetupExecutor.OnModuleEndAsync(moduleState).ConfigureAwait(false);

            // Invoke OnModuleEnd lifecycle event
            await _lifecycleEventInvoker.InvokeEndEventAsync(lifecycleContext, executionContext.Status, result).ConfigureAwait(false);

            var isSuccessful = executionContext.Status is Enums.Status.Successful or Enums.Status.UsedHistory;
            await _mediator.Publish(new ModuleCompletedNotification(moduleState, isSuccessful)).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Even when an exception is thrown, we need to register the result if one was set
            if (executionContext.ExecutionTask.IsCompleted && !executionContext.ExecutionTask.IsFaulted && !executionContext.ExecutionTask.IsCanceled)
            {
                // Use GetAwaiter().GetResult() instead of .Result to avoid wrapping in AggregateException
                var result = executionContext.ExecutionTask.GetAwaiter().GetResult();
                moduleState.Result = result;
                _resultRegistry.RegisterResult(moduleType, result);
            }

            try
            {
                // Invoke OnModuleFailed lifecycle event
                await _lifecycleEventInvoker.InvokeFailedEventAsync(lifecycleContext, ex).ConfigureAwait(false);

                await _pipelineSetupExecutor.OnModuleFailureAsync(moduleState).ConfigureAwait(false);
            }
            finally
            {
                await _mediator.Publish(new ModuleCompletedNotification(moduleState, false)).ConfigureAwait(false);
            }

            throw;
        }
        finally
        {
            // Store execution context results in module state
            moduleState.SkipResult = executionContext.SkipResult;

            if (!_pipelineOptions.Value.ShowProgressInConsole)
            {
                await _moduleDisposer.DisposeAsync(moduleState).ConfigureAwait(false);
            }
        }
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
        ModuleExecutionContext executionContext,
        IModuleContext moduleContext,
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
                    cancellationToken)
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
                cancellationToken)
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
