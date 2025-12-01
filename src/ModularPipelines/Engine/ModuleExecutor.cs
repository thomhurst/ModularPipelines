using System.Reflection;
using Mediator;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
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
        IModuleResultRegistry resultRegistry)
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
            scheduler = InitializeScheduler(modules);
            return await ExecuteWithSchedulerAsync(modules, scheduler);
        }
        catch (Exception outerEx)
        {
            _logger.LogDebug("Outer catch block entered with exception: {ExceptionType}", outerEx.GetType().Name);

            if (scheduler != null)
            {
                await WaitForAlwaysRunModulesAsync(scheduler, modules);
            }

            _logger.LogDebug("Outer catch block rethrowing exception");
            throw;
        }
        finally
        {
            scheduler?.Dispose();
        }
    }

    private IModuleScheduler InitializeScheduler(IReadOnlyList<IModule> modules)
    {
        _logger.LogDebug("Initializing unified scheduler for {Count} modules", modules.Count);

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
            firstException = await ExecuteWorkerPoolAsync(scheduler, cancellationTokenSource);
        }
        finally
        {
            EnsureCancellation(cancellationTokenSource);
        }

        await schedulerTask;

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
            CancellationToken = cancellationTokenSource.Token
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
                        await ExecuteModule(moduleState, scheduler, ct);
                    }
                    catch (Exception ex) when (_pipelineOptions.Value.ExecutionMode == ExecutionMode.StopOnFirstException)
                    {
                        Interlocked.CompareExchange(ref firstException, ex, null);
                        cancellationTokenSource.Cancel();
                    }
                });
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
            await WaitForSingleAlwaysRunModuleAsync(scheduler, module);
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
                await ExecuteModuleCore(moduleState, scheduler, CancellationToken.None, skipDependencyWait: true);
                _logger.LogDebug("AlwaysRun module {ModuleName} completed after late start", moduleType.Name);
            }
            catch (Exception alwaysRunEx)
            {
                _logger.LogDebug("AlwaysRun module {ModuleName} threw after late start: {ExceptionType}",
                    moduleType.Name, alwaysRunEx.GetType().Name);
            }
        }
        else if (ShouldWaitForAlwaysRunModule(moduleState))
        {
            _logger.LogDebug("Awaiting AlwaysRun module: {ModuleName} (State={State})",
                moduleType.Name, moduleState.State);

            try
            {
                await moduleTask;
                _logger.LogDebug("AlwaysRun module {ModuleName} completed", moduleType.Name);
            }
            catch (Exception alwaysRunEx)
            {
                _logger.LogDebug("AlwaysRun module {ModuleName} threw: {ExceptionType}",
                    moduleType.Name, alwaysRunEx.GetType().Name);
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
                await WaitForDependenciesAsync(moduleState, scheduler);
            }
            else
            {
                _logger.LogDebug("Skipping dependency wait for late-started AlwaysRun module: {ModuleName}", moduleName);
            }

            await ExecuteModuleWithPipeline(moduleState, cancellationToken);

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

    private async Task ExecuteModuleWithPipeline(ModuleState moduleState, CancellationToken cancellationToken)
    {
        var module = moduleState.Module;
        var moduleType = moduleState.ModuleType;

        // Create a scope to resolve scoped services like IPipelineContext
        await using var scope = _serviceProvider.CreateAsyncScope();
        var pipelineContext = scope.ServiceProvider.GetRequiredService<IPipelineContext>();

        // Create module-specific context
        var executionContext = CreateExecutionContext(module, moduleType);
        var logger = GetOrCreateLogger(moduleType);
        var moduleContext = new ModuleContext(pipelineContext, module, executionContext, logger);

        // Set up logging
        ModuleLogger.Values.Value = logger;

        // Before module hooks
        await _pipelineSetupExecutor.OnBeforeModuleStartAsync(moduleState);

        var estimatedDuration = await _moduleEstimatedTimeProvider.GetModuleEstimatedTimeAsync(moduleType);
        await _mediator.Publish(new ModuleStartedNotification(moduleState, estimatedDuration));

        using var semaphoreHandle = await WaitForParallelLimiter(moduleType);

        try
        {
            // Execute via the ModuleExecutionPipeline using reflection to handle the generic type
            var result = await ExecuteTypedModule(module, executionContext, moduleContext, cancellationToken);

            moduleState.Result = result;
            _resultRegistry.RegisterResult(moduleType, result);

            if (executionContext.Status == Enums.Status.Skipped)
            {
                await _mediator.Publish(new ModuleSkippedNotification(moduleState, executionContext.SkipResult));
                return;
            }

            await _moduleEstimatedTimeProvider.SaveModuleTimeAsync(moduleType, executionContext.Duration);
            await _pipelineSetupExecutor.OnAfterModuleEndAsync(moduleState);

            var isSuccessful = executionContext.Status == Enums.Status.Successful;
            await _mediator.Publish(new ModuleCompletedNotification(moduleState, isSuccessful));
        }
        catch
        {
            // Even when an exception is thrown, we need to register the result if one was set
            // The ModuleExecutionPipeline sets the result before throwing
            if (executionContext.ExecutionTask.IsCompletedSuccessfully)
            {
                var result = executionContext.ExecutionTask.Result;
                moduleState.Result = result;
                _resultRegistry.RegisterResult(moduleType, result);
            }

            throw;
        }
        finally
        {
            // Store execution context results in module state
            moduleState.SkipResult = executionContext.SkipResult;

            if (!_pipelineOptions.Value.ShowProgressInConsole)
            {
                await _moduleDisposer.DisposeAsync(moduleState);
            }
        }
    }

    private ModuleExecutionContext CreateExecutionContext(IModule module, Type moduleType)
    {
        // Use reflection to create the typed execution context
        var resultType = module.ResultType;
        var contextType = typeof(ModuleExecutionContext<>).MakeGenericType(resultType);
        return (ModuleExecutionContext)Activator.CreateInstance(contextType, module, moduleType)!;
    }

    private async Task<IModuleResult> ExecuteTypedModule(
        IModule module,
        ModuleExecutionContext executionContext,
        IModuleContext moduleContext,
        CancellationToken cancellationToken)
    {
        var resultType = module.ResultType;

        // Use reflection to call the generic ExecuteAsync method
        var executeMethod = typeof(IModuleExecutionPipeline).GetMethod(nameof(IModuleExecutionPipeline.ExecuteAsync))!
            .MakeGenericMethod(resultType);

        var task = (Task)executeMethod.Invoke(_executionPipeline, new object[] { module, executionContext, moduleContext, cancellationToken })!;
        await task;

        // Get the result from the completed task
        var resultProperty = task.GetType().GetProperty("Result")!;
        return (IModuleResult)resultProperty.GetValue(task)!;
    }

    private async Task<IDisposable> WaitForParallelLimiter(Type moduleType)
    {
        var parallelLimitAttributeType =
            moduleType.GetCustomAttributes<ParallelLimiterAttribute>().FirstOrDefault()?.Type;

        if (parallelLimitAttributeType != null)
        {
            return await _parallelLimitProvider.GetLock(parallelLimitAttributeType).WaitAsync();
        }

        return NoOpDisposable.Instance;
    }

    private async Task WaitForDependenciesAsync(ModuleState moduleState, IModuleScheduler scheduler)
    {
        var dependencies = ModuleDependencyResolver.GetDependencies(moduleState.ModuleType);

        foreach (var (dependencyType, ignoreIfNotRegistered) in dependencies)
        {
            var dependencyTask = scheduler.GetModuleCompletionTask(dependencyType);

            if (dependencyTask != null)
            {
                try
                {
                    await dependencyTask;
                }
                catch (Exception e) when (moduleState.Module.ModuleRunType == ModuleRunType.AlwaysRun)
                {
                    var depLogger = GetOrCreateLogger(moduleState.ModuleType);
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

    private IModuleLogger GetOrCreateLogger(Type moduleType)
    {
        var loggerType = typeof(ModuleLogger<>).MakeGenericType(moduleType);
        return _loggerContainer.GetLogger(loggerType)
            ?? (IModuleLogger)_serviceProvider.GetRequiredService(loggerType);
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
        var executionContext = (ModuleExecutionContext)Activator.CreateInstance(contextType, module, moduleType)!;
        executionContext.Status = Enums.Status.PipelineTerminated;
        executionContext.Exception = exception;

        // Create ModuleResult<T> with the exception
        var resultGenericType = typeof(ModuleResult<>).MakeGenericType(resultType);
        var result = (IModuleResult)Activator.CreateInstance(
            resultGenericType,
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance,
            null,
            new object[] { exception, executionContext },
            null)!;

        _resultRegistry.RegisterResult(moduleType, result);
    }
}
