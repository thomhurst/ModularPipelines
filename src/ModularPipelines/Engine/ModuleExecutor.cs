using System.Reflection;
using Mediator;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Attributes.Events;
using ModularPipelines.Context;
using ModularPipelines.Engine.Attributes;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Events;
using ModularPipelines.Exceptions;
using ModularPipelines.Helpers;
using ModularPipelines.Logging;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;

namespace ModularPipelines.Engine;

internal class ModuleExecutor : IModuleExecutor
{
    private readonly IPipelineSetupExecutor _pipelineSetupExecutor;
    private readonly IOptions<PipelineOptions> _pipelineOptions;
    private readonly ISafeModuleEstimatedTimeProvider _moduleEstimatedTimeProvider;
    private readonly IModuleDisposer _moduleDisposer;
    private readonly IEnumerable<IModule> _allModules;
    private readonly ISecondaryExceptionContainer _secondaryExceptionContainer;
    private readonly IParallelLimitProvider _parallelLimitProvider;
    private readonly IMediator _mediator;
    private readonly ILogger<ModuleExecutor> _logger;
    private readonly IModuleSchedulerFactory _schedulerFactory;
    private readonly IModuleExecutionPipeline _executionPipeline;
    private readonly IServiceProvider _serviceProvider;
    private readonly IModuleLoggerContainer _loggerContainer;
    private readonly EngineCancellationToken _engineCancellationToken;
    private readonly IModuleResultRegistry _resultRegistry;
    private readonly IRegistrationEventExecutor _registrationEventExecutor;
    private readonly IModuleAttributeEventService _attributeEventService;
    private readonly IAttributeEventInvoker _attributeEventInvoker;
    private readonly IModuleMetadataRegistry _metadataRegistry;
    private readonly IMetricsCollector _metricsCollector;

    public ModuleExecutor(
        IPipelineSetupExecutor pipelineSetupExecutor,
        IOptions<PipelineOptions> pipelineOptions,
        ISafeModuleEstimatedTimeProvider moduleEstimatedTimeProvider,
        IModuleDisposer moduleDisposer,
        IEnumerable<IModule> allModules,
        ISecondaryExceptionContainer secondaryExceptionContainer,
        IParallelLimitProvider parallelLimitProvider,
        IMediator mediator,
        ILogger<ModuleExecutor> logger,
        IModuleSchedulerFactory schedulerFactory,
        IModuleExecutionPipeline executionPipeline,
        IServiceProvider serviceProvider,
        IModuleLoggerContainer loggerContainer,
        EngineCancellationToken engineCancellationToken,
        IModuleResultRegistry resultRegistry,
        IRegistrationEventExecutor registrationEventExecutor,
        IModuleAttributeEventService attributeEventService,
        IAttributeEventInvoker attributeEventInvoker,
        IModuleMetadataRegistry metadataRegistry,
        IMetricsCollector metricsCollector)
    {
        _pipelineSetupExecutor = pipelineSetupExecutor;
        _pipelineOptions = pipelineOptions;
        _moduleEstimatedTimeProvider = moduleEstimatedTimeProvider;
        _moduleDisposer = moduleDisposer;
        _allModules = allModules;
        _secondaryExceptionContainer = secondaryExceptionContainer;
        _parallelLimitProvider = parallelLimitProvider;
        _mediator = mediator;
        _logger = logger;
        _schedulerFactory = schedulerFactory;
        _executionPipeline = executionPipeline;
        _serviceProvider = serviceProvider;
        _loggerContainer = loggerContainer;
        _engineCancellationToken = engineCancellationToken;
        _resultRegistry = resultRegistry;
        _registrationEventExecutor = registrationEventExecutor;
        _attributeEventService = attributeEventService;
        _attributeEventInvoker = attributeEventInvoker;
        _metadataRegistry = metadataRegistry;
        _metricsCollector = metricsCollector;
    }

    /// <summary>
    /// Executes a collection of modules using eager parallel scheduling.
    /// </summary>
    public async Task<IEnumerable<IModule>> ExecuteAsync(IReadOnlyList<IModule> modules)
    {
        ArgumentNullException.ThrowIfNull(modules);

        if (modules.Count == 0)
        {
            _logger.LogDebug("No modules to execute");
            return modules;
        }

        IModuleScheduler? scheduler = null;

        try
        {
            scheduler = await InitializeSchedulerAsync(modules).ConfigureAwait(false);
            return await ExecuteWithSchedulerAsync(modules, scheduler).ConfigureAwait(false);
        }
        catch (Exception outerEx)
        {
            _logger.LogDebug("Outer catch block entered with exception: {ExceptionType}", outerEx.GetType().Name);

            if (scheduler != null)
            {
                await WaitForAlwaysRunModulesAsync(scheduler, modules).ConfigureAwait(false);
            }

            _logger.LogDebug("Outer catch block rethrowing exception");
            throw;
        }
        finally
        {
            scheduler?.Dispose();
        }
    }

    private async Task<IModuleScheduler> InitializeSchedulerAsync(IReadOnlyList<IModule> modules)
    {
        _logger.LogDebug("Initializing unified scheduler for {Count} modules", modules.Count);

        // Record pipeline start time for metrics
        _metricsCollector.SetPipelineStartTime(DateTimeOffset.UtcNow);

        // Invoke registration events before dependency resolution
        await _registrationEventExecutor.InvokeRegistrationEventsAsync(modules).ConfigureAwait(false);

        var scheduler = _schedulerFactory.Create();
        scheduler.InitializeModules(modules);

        return scheduler;
    }

    private async Task<IEnumerable<IModule>> ExecuteWithSchedulerAsync(
        IReadOnlyList<IModule> modules,
        IModuleScheduler scheduler)
    {
        using var cancellationTokenSource = new CancellationTokenSource();

        RegisterCancellationCallback(cancellationTokenSource, scheduler);

        var schedulerTask = scheduler.RunSchedulerAsync(cancellationTokenSource.Token);

        Exception? firstException;

        try
        {
            firstException = await ExecuteWorkerPoolAsync(scheduler, cancellationTokenSource).ConfigureAwait(false);
        }
        finally
        {
            EnsureCancellation(cancellationTokenSource);
        }

        await schedulerTask.ConfigureAwait(false);

        _logger.LogDebug("All modules completed");

        // Register PipelineTerminated results for modules that were cancelled before they started
        if (firstException != null)
        {
            RegisterTerminatedResultsForCancelledModules(modules, firstException);
        }

        RethrowFirstExceptionIfPresent(firstException);

        _logger.LogDebug("ExecuteAsync returning normally with {Count} modules", modules.Count);
        return modules;
    }

    private void RegisterCancellationCallback(CancellationTokenSource cancellationTokenSource, IModuleScheduler scheduler)
    {
        cancellationTokenSource.Token.Register(() =>
        {
            _logger.LogDebug("Cancellation triggered - cancelling all pending modules");
            scheduler.CancelPendingModules();
        });
    }

    private async Task<Exception?> ExecuteWorkerPoolAsync(
        IModuleScheduler scheduler,
        CancellationTokenSource cancellationTokenSource)
    {
        var maxDegreeOfParallelism = _parallelLimitProvider.GetMaxDegreeOfParallelism();

        _logger.LogDebug("Starting worker pool with MaxDegreeOfParallelism = {MaxDegreeOfParallelism}",
            maxDegreeOfParallelism);

        var parallelOptions = new ParallelOptions
        {
            MaxDegreeOfParallelism = maxDegreeOfParallelism,
            CancellationToken = cancellationTokenSource.Token,
        };

        Exception? firstException = null;

        try
        {
            await Parallel.ForEachAsync(
                scheduler.ReadyModules.ReadAllAsync(cancellationTokenSource.Token),
                parallelOptions,
                async (moduleState, ct) =>
                {
                    try
                    {
                        await ExecuteModule(moduleState, scheduler, ct).ConfigureAwait(false);
                    }
                    catch (Exception ex) when (_pipelineOptions.Value.ExecutionMode == ExecutionMode.StopOnFirstException)
                    {
                        Interlocked.CompareExchange(ref firstException, ex, null);
                        cancellationTokenSource.Cancel();
                    }
                }).ConfigureAwait(false);
        }
        catch (OperationCanceledException) when (firstException != null)
        {
            // Expected when we cancelled due to StopOnFirstException
        }
        catch (Exception ex) when (_pipelineOptions.Value.ExecutionMode != ExecutionMode.StopOnFirstException)
        {
            _logger.LogDebug(ex, "Module execution failed but continuing due to ExecutionMode.WaitForAllModules");
        }

        return firstException;
    }

    private void EnsureCancellation(CancellationTokenSource cancellationTokenSource)
    {
        if (!cancellationTokenSource.IsCancellationRequested)
        {
            cancellationTokenSource.Cancel();
        }
    }

    private void RethrowFirstExceptionIfPresent(Exception? firstException)
    {
        if (firstException != null)
        {
            _logger.LogDebug("Rethrowing first exception: {ExceptionType}", firstException.GetType().Name);
            System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(firstException).Throw();
        }
    }

    private async Task WaitForAlwaysRunModulesAsync(IModuleScheduler scheduler, IReadOnlyList<IModule> modules)
    {
        var alwaysRunModules = modules.Where(x => x.ModuleRunType == ModuleRunType.AlwaysRun).ToList();
        _logger.LogDebug("Found {Count} AlwaysRun modules", alwaysRunModules.Count);

        foreach (var module in alwaysRunModules)
        {
            await WaitForSingleAlwaysRunModuleAsync(scheduler, module).ConfigureAwait(false);
        }
    }

    private async Task WaitForSingleAlwaysRunModuleAsync(IModuleScheduler scheduler, IModule module)
    {
        var moduleType = module.GetType();
        var moduleState = scheduler.GetModuleState(moduleType);
        var moduleTask = scheduler.GetModuleCompletionTask(moduleType);

        if (moduleTask == null || moduleState == null)
        {
            return;
        }

        // If the AlwaysRun module is still pending (never started), execute it now
        // Skip dependency waiting to prevent deadlocks - dependencies may never complete
        if (moduleState.State == ModuleExecutionState.Pending)
        {
            _logger.LogDebug("Starting pending AlwaysRun module: {ModuleName}", moduleType.Name);

            try
            {
                await ExecuteModuleCore(moduleState, scheduler, CancellationToken.None, skipDependencyWait: true).ConfigureAwait(false);
                _logger.LogDebug("AlwaysRun module {ModuleName} completed after late start", moduleType.Name);
            }
            catch (Exception alwaysRunEx)
            {
                _logger.LogWarning(alwaysRunEx, "AlwaysRun module {ModuleName} failed after late start",
                    moduleType.Name);
            }
        }
        else if (ShouldWaitForAlwaysRunModule(moduleState))
        {
            _logger.LogDebug("Awaiting AlwaysRun module: {ModuleName} (State={State})",
                moduleType.Name, moduleState.State);

            try
            {
                await moduleTask.ConfigureAwait(false);
                _logger.LogDebug("AlwaysRun module {ModuleName} completed", moduleType.Name);
            }
            catch (Exception alwaysRunEx)
            {
                _logger.LogWarning(alwaysRunEx, "AlwaysRun module {ModuleName} failed",
                    moduleType.Name);

                // Access Exception property to observe the exception and prevent TaskScheduler.UnobservedTaskException
                _ = moduleTask.Exception;
            }
        }
        else
        {
            _logger.LogDebug("Skipping AlwaysRun module {ModuleName} (State={State})",
                moduleType.Name, moduleState.State);
        }
    }

    private static bool ShouldWaitForAlwaysRunModule(ModuleState moduleState)
    {
        return moduleState.State == ModuleExecutionState.Executing || moduleState.State == ModuleExecutionState.Completed;
    }

    private Task ExecuteModule(ModuleState moduleState, IModuleScheduler scheduler, CancellationToken cancellationToken)
    {
        return ExecuteModuleCore(moduleState, scheduler, cancellationToken, skipDependencyWait: false);
    }

    private async Task ExecuteModuleCore(
        ModuleState moduleState,
        IModuleScheduler scheduler,
        CancellationToken cancellationToken,
        bool skipDependencyWait)
    {
        var module = moduleState.Module;
        var moduleType = moduleState.ModuleType;
        var moduleName = MarkupFormatter.FormatModuleName(moduleType.Name);

        // Create a scope to resolve scoped services like IPipelineContext and ModuleLogger<T>
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

                _logger.LogDebug("{Icon} Starting module {ModuleName}", MarkupFormatter.PlayIcon, moduleName);

                if (!skipDependencyWait)
                {
                    await WaitForDependenciesAsync(moduleState, scheduler, scope.ServiceProvider).ConfigureAwait(false);
                }
                else
                {
                    _logger.LogDebug("Skipping dependency wait for late-started AlwaysRun module: {ModuleName}", moduleName);
                }

                await ExecuteModuleWithPipeline(moduleState, scope.ServiceProvider, cancellationToken).ConfigureAwait(false);

                scheduler.MarkModuleCompleted(moduleType, true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Module {ModuleName} failed", moduleName);
                scheduler.MarkModuleCompleted(moduleType, false, ex);

                // Register a PipelineTerminated result for this module if no result was registered yet
                // This happens when the module was waiting for a dependency that failed
                if (moduleState.Result == null)
                {
                    RegisterTerminatedResult(module, moduleType, ex);
                }

                if (_pipelineOptions.Value.ExecutionMode == ExecutionMode.StopOnFirstException)
                {
                    throw;
                }
            }
        }
    }

    private async Task ExecuteModuleWithPipeline(ModuleState moduleState, IServiceProvider scopedServiceProvider, CancellationToken cancellationToken)
    {
        var module = moduleState.Module;
        var moduleType = moduleState.ModuleType;

        var pipelineContext = scopedServiceProvider.GetRequiredService<IPipelineContext>();

        // Create module-specific context
        var executionContext = CreateExecutionContext(module, moduleType);
        var logger = GetOrCreateLogger(moduleType, scopedServiceProvider);
        var moduleContext = new ModuleContext(pipelineContext, module, executionContext, logger);

        // Set up logging
        ModuleLogger.Values.Value = logger;

        // Before module hooks
        await _pipelineSetupExecutor.OnBeforeModuleStartAsync(moduleState).ConfigureAwait(false);

        var estimatedDuration = await _moduleEstimatedTimeProvider.GetModuleEstimatedTimeAsync(moduleType).ConfigureAwait(false);
        await _mediator.Publish(new ModuleStartedNotification(moduleState, estimatedDuration)).ConfigureAwait(false);

        using var semaphoreHandle = await WaitForParallelLimiter(moduleType).ConfigureAwait(false);
        using var executionTypeHandle = await WaitForExecutionTypeLimiter(moduleState).ConfigureAwait(false);

        // Track start time for lifecycle events
        var startTime = DateTimeOffset.UtcNow;
        var moduleAttributes = moduleType.GetCustomAttributes(inherit: true).OfType<Attribute>().ToList();

        try
        {
            // Invoke OnModuleReady lifecycle event (dependencies satisfied, about to execute)
            await InvokeReadyEventAsync(module, moduleType, moduleAttributes, moduleState.ReadyTime ?? startTime, pipelineContext, scopedServiceProvider, cancellationToken).ConfigureAwait(false);

            // Invoke OnModuleStart lifecycle event
            await InvokeStartEventAsync(module, moduleType, moduleAttributes, startTime, pipelineContext, scopedServiceProvider, cancellationToken).ConfigureAwait(false);

            // Execute via the ModuleExecutionPipeline using reflection to handle the generic type
            var result = await ExecuteTypedModule(module, executionContext, moduleContext, cancellationToken).ConfigureAwait(false);

            moduleState.Result = result;
            _resultRegistry.RegisterResult(moduleType, result);

            if (executionContext.Status == Enums.Status.Skipped)
            {
                // Invoke OnModuleSkipped lifecycle event
                await InvokeSkippedEventAsync(module, moduleType, moduleAttributes, startTime, Enums.Status.Skipped, pipelineContext, scopedServiceProvider, executionContext.SkipResult!, cancellationToken).ConfigureAwait(false);

                await _mediator.Publish(new ModuleSkippedNotification(moduleState, executionContext.SkipResult)).ConfigureAwait(false);
                return;
            }

            await _moduleEstimatedTimeProvider.SaveModuleTimeAsync(moduleType, executionContext.Duration).ConfigureAwait(false);
            await _pipelineSetupExecutor.OnAfterModuleEndAsync(moduleState).ConfigureAwait(false);

            // Invoke OnModuleEnd lifecycle event
            await InvokeEndEventAsync(module, moduleType, moduleAttributes, startTime, executionContext.Status, pipelineContext, scopedServiceProvider, result, cancellationToken).ConfigureAwait(false);

            var isSuccessful = executionContext.Status == Enums.Status.Successful;
            await _mediator.Publish(new ModuleCompletedNotification(moduleState, isSuccessful)).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Even when an exception is thrown, we need to register the result if one was set
            // The ModuleExecutionPipeline sets the result before throwing
            // Check IsCompleted instead of IsCompletedSuccessfully to handle all completion states
            if (executionContext.ExecutionTask.IsCompleted && !executionContext.ExecutionTask.IsFaulted && !executionContext.ExecutionTask.IsCanceled)
            {
                var result = executionContext.ExecutionTask.Result;
                moduleState.Result = result;
                _resultRegistry.RegisterResult(moduleType, result);
            }

            // Invoke OnModuleFailed lifecycle event
            await InvokeFailedEventAsync(module, moduleType, moduleAttributes, startTime, pipelineContext, scopedServiceProvider, ex, cancellationToken).ConfigureAwait(false);

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
        // Use reflection to create the typed execution context
        var resultType = module.ResultType;
        var contextType = typeof(ModuleExecutionContext<>).MakeGenericType(resultType);
        return (ModuleExecutionContext) Activator.CreateInstance(contextType, module, moduleType)!;
    }

    private async Task<IModuleResult> ExecuteTypedModule(
        IModule module,
        ModuleExecutionContext executionContext,
        IModuleContext moduleContext,
        CancellationToken cancellationToken)
    {
        var resultType = module.ResultType;

        // Use reflection to call the generic ExecuteAsync method
        var executeMethodInfo = typeof(IModuleExecutionPipeline).GetMethod(nameof(IModuleExecutionPipeline.ExecuteAsync))
            ?? throw new InvalidOperationException($"Method '{nameof(IModuleExecutionPipeline.ExecuteAsync)}' not found on type '{nameof(IModuleExecutionPipeline)}'.");

        var executeMethod = executeMethodInfo.MakeGenericMethod(resultType);

        var invokeResult = executeMethod.Invoke(_executionPipeline, new object[] { module, executionContext, moduleContext, cancellationToken })
            ?? throw new InvalidOperationException($"Invocation of '{nameof(IModuleExecutionPipeline.ExecuteAsync)}' returned null.");

        var task = (Task)invokeResult;
        await task.ConfigureAwait(false);

        // Get the result from the completed task
        var resultProperty = task.GetType().GetProperty("Result")
            ?? throw new InvalidOperationException($"Property 'Result' not found on task type '{task.GetType().Name}'.");

        var resultValue = resultProperty.GetValue(task)
            ?? throw new InvalidOperationException($"Property 'Result' returned null for task type '{task.GetType().Name}'.");

        return (IModuleResult)resultValue;
    }

    private async Task<IDisposable> WaitForParallelLimiter(Type moduleType)
    {
        var parallelLimitAttributeType =
            moduleType.GetCustomAttributes<ParallelLimiterAttribute>().FirstOrDefault()?.Type;

        if (parallelLimitAttributeType != null)
        {
            return await _parallelLimitProvider.GetLock(parallelLimitAttributeType).WaitAsync().ConfigureAwait(false);
        }

        return NoOpDisposable.Instance;
    }

    private async Task<IDisposable> WaitForExecutionTypeLimiter(ModuleState moduleState)
    {
        var executionTypeLock = _parallelLimitProvider.GetExecutionTypeLock(moduleState.ExecutionType);

        if (executionTypeLock != null)
        {
            _logger.LogDebug(
                "Module {ModuleName} waiting for {ExecutionType} execution slot",
                MarkupFormatter.FormatModuleName(moduleState.ModuleType.Name),
                moduleState.ExecutionType);

            return await executionTypeLock.WaitAsync().ConfigureAwait(false);
        }

        return NoOpDisposable.Instance;
    }

    private async Task WaitForDependenciesAsync(ModuleState moduleState, IModuleScheduler scheduler, IServiceProvider scopedServiceProvider)
    {
        var dependencies = ModuleDependencyResolver.GetDependencies(moduleState.ModuleType);

        foreach (var (dependencyType, ignoreIfNotRegistered) in dependencies)
        {
            var dependencyTask = scheduler.GetModuleCompletionTask(dependencyType);

            if (dependencyTask != null)
            {
                try
                {
                    await dependencyTask.ConfigureAwait(false);
                }
                catch (Exception e) when (moduleState.Module.ModuleRunType == ModuleRunType.AlwaysRun)
                {
                    var depLogger = GetOrCreateLogger(moduleState.ModuleType, scopedServiceProvider);
                    _secondaryExceptionContainer.RegisterException(new AlwaysRunPostponedException(
                        $"{dependencyType.Name} threw an exception when {moduleState.ModuleType.Name} was waiting for it as a dependency",
                        e));
                    depLogger.LogError(e, "Ignoring Exception due to 'AlwaysRun' set");
                }
            }
            else if (!ignoreIfNotRegistered)
            {
                var message = $"Module '{moduleState.ModuleType.Name}' depends on '{dependencyType.Name}', " +
                              $"but '{dependencyType.Name}' has not been registered in the pipeline.\n\n" +
                              $"Suggestions:\n" +
                              $"  1. Add '.AddModule<{dependencyType.Name}>()' to your pipeline configuration before '.AddModule<{moduleState.ModuleType.Name}>()'\n" +
                              $"  2. Use '[DependsOn<{dependencyType.Name}>(ignoreIfNotRegistered: true)]' if this dependency is optional\n" +
                              $"  3. Check for typos in the dependency type name\n" +
                              $"  4. Verify that '{dependencyType.Name}' is in a project referenced by your pipeline project";
                throw new ModuleNotRegisteredException(message, null);
            }
        }
    }

    private IModuleLogger GetOrCreateLogger(Type moduleType, IServiceProvider scopedServiceProvider)
    {
        var loggerType = typeof(ModuleLogger<>).MakeGenericType(moduleType);
        return _loggerContainer.GetLogger(loggerType)
            ?? (IModuleLogger) scopedServiceProvider.GetRequiredService(loggerType);
    }

    private void RegisterTerminatedResultsForCancelledModules(IReadOnlyList<IModule> modules, Exception exception)
    {
        foreach (var module in modules)
        {
            var moduleType = module.GetType();

            // Check if a result was already registered for this module
            if (_resultRegistry.GetResult(moduleType) != null)
            {
                continue;
            }

            _logger.LogDebug("Registering PipelineTerminated result for cancelled module {ModuleName}",
                MarkupFormatter.FormatModuleName(moduleType.Name));

            RegisterTerminatedResult(module, moduleType, exception);
        }
    }

    private void RegisterTerminatedResult(IModule module, Type moduleType, Exception exception)
    {
        var resultType = module.ResultType;

        // Create execution context with PipelineTerminated status
        var contextType = typeof(ModuleExecutionContext<>).MakeGenericType(resultType);
        var executionContext = (ModuleExecutionContext) Activator.CreateInstance(contextType, module, moduleType)!;
        executionContext.Status = Enums.Status.PipelineTerminated;
        executionContext.Exception = exception;

        // Create ModuleResult<T> with the exception
        var resultGenericType = typeof(ModuleResult<>).MakeGenericType(resultType);
        var result = (IModuleResult) Activator.CreateInstance(
            resultGenericType,
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance,
            null,
            new object[] { exception, executionContext },
            null)!;

        _resultRegistry.RegisterResult(moduleType, result);
    }

    private async Task InvokeReadyEventAsync(
        IModule module,
        Type moduleType,
        IReadOnlyList<Attribute> moduleAttributes,
        DateTimeOffset readyTime,
        IPipelineContext pipelineContext,
        IServiceProvider scopedServiceProvider,
        CancellationToken cancellationToken)
    {
        var receivers = _attributeEventService.GetReadyReceivers(moduleType);
        if (receivers.Count == 0)
        {
            return;
        }

        var pipelineStartTime = _metricsCollector.GetPipelineStartTime() ?? readyTime;

        var eventContext = new ModuleReadyContext(
            module,
            moduleType,
            moduleAttributes,
            readyTime,
            pipelineStartTime,
            pipelineContext,
            scopedServiceProvider,
            _metadataRegistry,
            cancellationToken);

        await _attributeEventInvoker.InvokeReadyReceiversAsync(receivers, eventContext).ConfigureAwait(false);
    }

    private async Task InvokeStartEventAsync(
        IModule module,
        Type moduleType,
        IReadOnlyList<Attribute> moduleAttributes,
        DateTimeOffset startTime,
        IPipelineContext pipelineContext,
        IServiceProvider scopedServiceProvider,
        CancellationToken cancellationToken)
    {
        var receivers = _attributeEventService.GetStartReceivers(moduleType);
        if (receivers.Count == 0)
        {
            return;
        }

        var eventContext = new ModuleEventContext(
            module,
            moduleType,
            moduleAttributes,
            startTime,
            Enums.Status.Processing,
            pipelineContext,
            scopedServiceProvider,
            _metadataRegistry,
            cancellationToken);

        await _attributeEventInvoker.InvokeStartReceiversAsync(receivers, eventContext).ConfigureAwait(false);
    }

    private async Task InvokeEndEventAsync(
        IModule module,
        Type moduleType,
        IReadOnlyList<Attribute> moduleAttributes,
        DateTimeOffset startTime,
        Enums.Status status,
        IPipelineContext pipelineContext,
        IServiceProvider scopedServiceProvider,
        IModuleResult result,
        CancellationToken cancellationToken)
    {
        var receivers = _attributeEventService.GetEndReceivers(moduleType);
        if (receivers.Count == 0)
        {
            return;
        }

        var eventContext = new ModuleEventContext(
            module,
            moduleType,
            moduleAttributes,
            startTime,
            status,
            pipelineContext,
            scopedServiceProvider,
            _metadataRegistry,
            cancellationToken);

        await _attributeEventInvoker.InvokeEndReceiversAsync(receivers, eventContext, (ModuleResult)result).ConfigureAwait(false);
    }

    private async Task InvokeFailedEventAsync(
        IModule module,
        Type moduleType,
        IReadOnlyList<Attribute> moduleAttributes,
        DateTimeOffset startTime,
        IPipelineContext pipelineContext,
        IServiceProvider scopedServiceProvider,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var receivers = _attributeEventService.GetFailureReceivers(moduleType);
        if (receivers.Count == 0)
        {
            return;
        }

        var eventContext = new ModuleEventContext(
            module,
            moduleType,
            moduleAttributes,
            startTime,
            Enums.Status.Failed,
            pipelineContext,
            scopedServiceProvider,
            _metadataRegistry,
            cancellationToken);

        await _attributeEventInvoker.InvokeFailureReceiversAsync(receivers, eventContext, exception).ConfigureAwait(false);
    }

    private async Task InvokeSkippedEventAsync(
        IModule module,
        Type moduleType,
        IReadOnlyList<Attribute> moduleAttributes,
        DateTimeOffset startTime,
        Enums.Status status,
        IPipelineContext pipelineContext,
        IServiceProvider scopedServiceProvider,
        SkipDecision skipReason,
        CancellationToken cancellationToken)
    {
        var receivers = _attributeEventService.GetSkippedReceivers(moduleType);
        if (receivers.Count == 0)
        {
            return;
        }

        var eventContext = new ModuleEventContext(
            module,
            moduleType,
            moduleAttributes,
            startTime,
            status,
            pipelineContext,
            scopedServiceProvider,
            _metadataRegistry,
            cancellationToken);

        await _attributeEventInvoker.InvokeSkippedReceiversAsync(receivers, eventContext, skipReason).ConfigureAwait(false);
    }
}
