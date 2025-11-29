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
    private readonly IModuleLoggerProvider _loggerProvider;
    private readonly EngineCancellationToken _engineCancellationToken;

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
        IModuleLoggerProvider loggerProvider,
        EngineCancellationToken engineCancellationToken)
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
        _loggerProvider = loggerProvider;
        _engineCancellationToken = engineCancellationToken;
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

        if (ShouldWaitForAlwaysRunModule(moduleState))
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
            _logger.LogDebug("Skipping AlwaysRun module {ModuleName} as it never started (State={State})",
                moduleType.Name, moduleState.State);
        }
    }

    private static bool ShouldWaitForAlwaysRunModule(ModuleState moduleState)
    {
        return moduleState.State == ModuleExecutionState.Executing || moduleState.State == ModuleExecutionState.Completed;
    }

    private async Task ExecuteModule(ModuleState moduleState, IModuleScheduler scheduler, CancellationToken cancellationToken)
    {
        var module = moduleState.Module;
        var moduleType = moduleState.ModuleType;
        var moduleName = MarkupFormatter.FormatModuleName(moduleType.Name);

        try
        {
            scheduler.MarkModuleStarted(moduleType);

            _logger.LogDebug("{Icon} Starting module {ModuleName}", MarkupFormatter.PlayIcon, moduleName);

            await WaitForDependenciesAsync(moduleState, scheduler);

            await ExecuteModuleWithPipeline(moduleState, cancellationToken);

            scheduler.MarkModuleCompleted(moduleType, true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Module {ModuleName} failed", moduleName);
            scheduler.MarkModuleCompleted(moduleType, false, ex);

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

        // Get the pipeline context
        var pipelineContext = _serviceProvider.GetRequiredService<IPipelineContext>();

        // Create module-specific context
        var executionContext = CreateExecutionContext(module, moduleType);
        var moduleContext = new ModuleContext(pipelineContext, module, executionContext, _loggerProvider);

        // Set up logging
        var logger = _loggerProvider.GetLogger(moduleType);
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
            moduleState.SkipResult = executionContext.SkipResult;

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
        finally
        {
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
                    var logger = _loggerProvider.GetLogger(moduleState.ModuleType);
                    _secondaryExceptionContainer.RegisterException(new AlwaysRunPostponedException(
                        $"{dependencyType.Name} threw an exception when {moduleState.ModuleType.Name} was waiting for it as a dependency",
                        e));
                    logger.LogError(e, "Ignoring Exception due to 'AlwaysRun' set");
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
}
