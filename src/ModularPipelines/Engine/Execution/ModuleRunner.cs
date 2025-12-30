using System.Reflection;
using Mediator;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Context;
using ModularPipelines.Events;
using ModularPipelines.Helpers;
using ModularPipelines.Logging;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;

namespace ModularPipelines.Engine.Execution;

/// <summary>
/// Responsible for executing a single module with proper scoping and coordination.
/// </summary>
internal class ModuleRunner : IModuleRunner
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IModuleExecutionPipeline _executionPipeline;
    private readonly IModuleLoggerContainer _loggerContainer;
    private readonly IPipelineSetupExecutor _pipelineSetupExecutor;
    private readonly IMediator _mediator;
    private readonly ISafeModuleEstimatedTimeProvider _moduleEstimatedTimeProvider;
    private readonly IModuleDisposer _moduleDisposer;
    private readonly IModuleResultRegistry _resultRegistry;
    private readonly IOptions<PipelineOptions> _pipelineOptions;
    private readonly ILogger<ModuleRunner> _logger;
    private readonly IDependencyWaiter _dependencyWaiter;
    private readonly IParallelLimitHandler _parallelLimitHandler;
    private readonly IModuleLifecycleEventInvoker _lifecycleEventInvoker;
    private readonly IModuleResultRegistrar _resultRegistrar;

    public ModuleRunner(
        IServiceProvider serviceProvider,
        IModuleExecutionPipeline executionPipeline,
        IModuleLoggerContainer loggerContainer,
        IPipelineSetupExecutor pipelineSetupExecutor,
        IMediator mediator,
        ISafeModuleEstimatedTimeProvider moduleEstimatedTimeProvider,
        IModuleDisposer moduleDisposer,
        IModuleResultRegistry resultRegistry,
        IOptions<PipelineOptions> pipelineOptions,
        ILogger<ModuleRunner> logger,
        IDependencyWaiter dependencyWaiter,
        IParallelLimitHandler parallelLimitHandler,
        IModuleLifecycleEventInvoker lifecycleEventInvoker,
        IModuleResultRegistrar resultRegistrar)
    {
        _serviceProvider = serviceProvider;
        _executionPipeline = executionPipeline;
        _loggerContainer = loggerContainer;
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
        _resultRegistrar = resultRegistrar;
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
                    await _dependencyWaiter.WaitForDependenciesAsync(moduleState, scheduler, scope.ServiceProvider).ConfigureAwait(false);
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

    private async Task ExecuteModuleWithPipeline(ModuleState moduleState, IServiceProvider scopedServiceProvider, CancellationToken cancellationToken)
    {
        var module = moduleState.Module;
        var moduleType = moduleState.ModuleType;

        var pipelineContext = scopedServiceProvider.GetRequiredService<IPipelineContext>();

        // Create module-specific context
        var executionContext = CreateExecutionContext(module, moduleType);
        var logger = GetOrCreateLogger(moduleType, scopedServiceProvider);
        var moduleContext = new ModuleContext(pipelineContext, module, executionContext, logger);

        // Set up logging and module type context - use try/finally to ensure cleanup of AsyncLocal context
        // Assignments MUST be inside try block to guarantee cleanup even if an exception
        // occurs immediately after assignment
        try
        {
            ModuleLogger.Values.Value = logger;
            ModuleLogger.CurrentModuleType.Value = moduleType;
            await ExecuteModuleLifecycle(moduleState, scopedServiceProvider, pipelineContext, executionContext, moduleContext, cancellationToken).ConfigureAwait(false);
        }
        finally
        {
            // Clear AsyncLocal to prevent potential leaks in edge cases (thread pool reuse, long-running contexts)
            ModuleLogger.Values.Value = null;
            ModuleLogger.CurrentModuleType.Value = null;
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

        // Before module hooks
        await _pipelineSetupExecutor.OnBeforeModuleStartAsync(moduleState).ConfigureAwait(false);

        var estimatedDuration = await _moduleEstimatedTimeProvider.GetModuleEstimatedTimeAsync(moduleType).ConfigureAwait(false);
        await _mediator.Publish(new ModuleStartedNotification(moduleState, estimatedDuration)).ConfigureAwait(false);

        using var semaphoreHandle = await _parallelLimitHandler.AcquireParallelLimitAsync(moduleType).ConfigureAwait(false);
        using var executionTypeHandle = await _parallelLimitHandler.AcquireExecutionTypeLimitAsync(moduleState).ConfigureAwait(false);

        // Track start time for lifecycle events
        var startTime = DateTimeOffset.UtcNow;
        var moduleAttributes = moduleType.GetCustomAttributes(inherit: true).OfType<Attribute>().ToList();

        var lifecycleContext = new ModuleLifecycleContext(
            module,
            moduleType,
            moduleAttributes,
            startTime,
            pipelineContext,
            scopedServiceProvider,
            cancellationToken)
        {
            ReadyTime = moduleState.ReadyTime ?? startTime
        };

        try
        {
            // Invoke OnModuleReady lifecycle event (dependencies satisfied, about to execute)
            await _lifecycleEventInvoker.InvokeReadyEventAsync(lifecycleContext).ConfigureAwait(false);

            // Invoke OnModuleStart lifecycle event
            await _lifecycleEventInvoker.InvokeStartEventAsync(lifecycleContext).ConfigureAwait(false);

            // Execute via the ModuleExecutionPipeline using reflection to handle the generic type
            var result = await ExecuteTypedModule(module, executionContext, moduleContext, cancellationToken).ConfigureAwait(false);

            moduleState.Result = result;
            _resultRegistry.RegisterResult(moduleType, result);

            if (executionContext.Status == Enums.Status.Skipped)
            {
                // Invoke OnModuleSkipped lifecycle event
                await _lifecycleEventInvoker.InvokeSkippedEventAsync(lifecycleContext, Enums.Status.Skipped, executionContext.SkipResult!).ConfigureAwait(false);

                await _mediator.Publish(new ModuleSkippedNotification(moduleState, executionContext.SkipResult)).ConfigureAwait(false);
                return;
            }

            await _moduleEstimatedTimeProvider.SaveModuleTimeAsync(moduleType, executionContext.Duration).ConfigureAwait(false);
            await _pipelineSetupExecutor.OnAfterModuleEndAsync(moduleState).ConfigureAwait(false);

            // Invoke OnModuleEnd lifecycle event
            await _lifecycleEventInvoker.InvokeEndEventAsync(lifecycleContext, executionContext.Status, result).ConfigureAwait(false);

            var isSuccessful = executionContext.Status == Enums.Status.Successful;
            await _mediator.Publish(new ModuleCompletedNotification(moduleState, isSuccessful)).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Even when an exception is thrown, we need to register the result if one was set
            if (executionContext.ExecutionTask.IsCompleted && !executionContext.ExecutionTask.IsFaulted && !executionContext.ExecutionTask.IsCanceled)
            {
                var result = executionContext.ExecutionTask.Result;
                moduleState.Result = result;
                _resultRegistry.RegisterResult(moduleType, result);
            }

            // Invoke OnModuleFailed lifecycle event
            await _lifecycleEventInvoker.InvokeFailedEventAsync(lifecycleContext, ex).ConfigureAwait(false);

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

    private async Task<IModuleResult> ExecuteTypedModule(
        IModule module,
        ModuleExecutionContext executionContext,
        IModuleContext moduleContext,
        CancellationToken cancellationToken)
    {
        // Use compiled delegate instead of MakeGenericMethod + Invoke + GetProperty("Result")
        var executor = ModuleExecutionDelegateFactory.GetExecutor(module.ResultType);
        return await executor(_executionPipeline, module, executionContext, moduleContext, cancellationToken)
            .ConfigureAwait(false);
    }

    private IModuleLogger GetOrCreateLogger(Type moduleType, IServiceProvider scopedServiceProvider)
    {
        var loggerType = typeof(ModuleLogger<>).MakeGenericType(moduleType);
        return _loggerContainer.GetLogger(loggerType)
            ?? (IModuleLogger)scopedServiceProvider.GetRequiredService(loggerType);
    }
}
