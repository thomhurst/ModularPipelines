using System.Reflection;
using System.Threading.Channels;
using Mediator;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Events;
using ModularPipelines.Exceptions;
using ModularPipelines.Extensions;
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
    private readonly IEnumerable<ModuleBase> _allModules;
    private readonly ISecondaryExceptionContainer _secondaryExceptionContainer;
    private readonly IParallelLimitProvider _parallelLimitProvider;
    private readonly IMediator _mediator;
    private readonly ILogger<ModuleExecutor> _logger;

    public ModuleExecutor(IPipelineSetupExecutor pipelineSetupExecutor,
        IOptions<PipelineOptions> pipelineOptions,
        ISafeModuleEstimatedTimeProvider moduleEstimatedTimeProvider,
        IModuleDisposer moduleDisposer,
        IEnumerable<ModuleBase> allModules,
        ISecondaryExceptionContainer secondaryExceptionContainer,
        IParallelLimitProvider parallelLimitProvider,
        IMediator mediator,
        ILogger<ModuleExecutor> logger)
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
    }

    public async Task<IEnumerable<ModuleBase>> ExecuteAsync(IReadOnlyList<ModuleBase> modules)
    {
        ArgumentNullException.ThrowIfNull(modules);

        IModuleScheduler? scheduler = null;

        try
        {
            if (modules.Count == 0)
            {
                _logger.LogDebug("No modules to execute");
                return modules;
            }

            _logger.LogDebug("Initializing unified scheduler for {Count} modules", modules.Count);

            scheduler = new ModuleScheduler(_logger);
            scheduler.InitializeModules(modules);

            var cancellationTokenSource = new CancellationTokenSource();
            var schedulerTask = scheduler.RunSchedulerAsync(cancellationTokenSource.Token);

            var maxDegreeOfParallelism = _parallelLimitProvider.GetMaxDegreeOfParallelism();

            _logger.LogDebug("Starting worker pool with MaxDegreeOfParallelism = {MaxDegreeOfParallelism}", maxDegreeOfParallelism);

            var parallelOptions = new ParallelOptions
            {
                MaxDegreeOfParallelism = maxDegreeOfParallelism,
                CancellationToken = cancellationTokenSource.Token
            };

            try
            {
                await Parallel.ForEachAsync(
                    scheduler.ReadyModules.ReadAllAsync(cancellationTokenSource.Token),
                    parallelOptions,
                    async (moduleState, ct) =>
                    {
                        await ExecuteModule(moduleState, scheduler, ct);
                    });
            }
            catch (Exception ex) when (_pipelineOptions.Value.ExecutionMode != ExecutionMode.StopOnFirstException)
            {
                _logger.LogDebug(ex, "Module execution failed but continuing due to ExecutionMode.WaitForAllModules");
            }

            await schedulerTask;

            _logger.LogDebug("All modules completed");

            return modules;
        }
        catch
        {
            if (scheduler != null)
            {
                foreach (var moduleBase in modules.Where(x => x.ModuleRunType == ModuleRunType.AlwaysRun))
                {
                    var moduleTask = scheduler.GetModuleCompletionTask(moduleBase.GetType());
                    if (moduleTask != null)
                    {
                        try
                        {
                            await moduleTask;
                        }
                        catch
                        {
                            _ = moduleTask.Exception;
                        }
                    }
                }
            }

            throw;
        }
        finally
        {
            scheduler?.Dispose();
        }
    }

    private async Task ExecuteModule(ModuleState moduleState, IModuleScheduler scheduler, CancellationToken cancellationToken)
    {
        var module = moduleState.Module;
        var moduleName = MarkupFormatter.FormatModuleName(module.GetType().Name);

        try
        {
            scheduler.MarkModuleStarted(module.GetType());

            _logger.LogDebug("{Icon} Starting module {ModuleName}", MarkupFormatter.PlayIcon, moduleName);

            await WaitForDependenciesAsync(module, scheduler);

            await module.GetOrStartExecutionTask(() => ExecuteModuleTaskAsync(module));

            scheduler.MarkModuleCompleted(module.GetType(), true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Module {ModuleName} failed", moduleName);
            scheduler.MarkModuleCompleted(module.GetType(), false, ex);

            if (_pipelineOptions.Value.ExecutionMode == ExecutionMode.StopOnFirstException)
            {
                throw;
            }
        }
    }

    private async Task ExecuteModuleCore(ModuleBase module)
    {
        ModuleLogger.Values.Value = module.Context.Logger;

        await _pipelineSetupExecutor.OnBeforeModuleStartAsync(module);

        var estimatedDuration = await _moduleEstimatedTimeProvider.GetModuleEstimatedTimeAsync(module.GetType());

        await _mediator.Publish(new ModuleStartedNotification(module, estimatedDuration));

        await module.StartInternal();

        if (module.Status == Enums.Status.Skipped)
        {
            await _mediator.Publish(new ModuleSkippedNotification(module, module.SkipResult));
            return;
        }

        await _moduleEstimatedTimeProvider.SaveModuleTimeAsync(module.GetType(), module.Duration);

        await _pipelineSetupExecutor.OnAfterModuleEndAsync(module);

        var isSuccessful = module.Status == Enums.Status.Successful;
        await _mediator.Publish(new ModuleCompletedNotification(module, isSuccessful));
    }

    private async Task ExecuteModuleTaskAsync(ModuleBase module)
    {
        using var semaphoreHandle = await WaitForParallelLimiter(module);

        try
        {
            await ExecuteModuleCore(module);
        }
        finally
        {
            if (!_pipelineOptions.Value.ShowProgressInConsole)
            {
                await _moduleDisposer.DisposeAsync(module);
            }
        }
    }

    private async Task<IDisposable> WaitForParallelLimiter(ModuleBase module)
    {
        var parallelLimitAttributeType =
            module.GetType().GetCustomAttributes<ParallelLimiterAttribute>().FirstOrDefault()?.Type;

        if (parallelLimitAttributeType != null)
        {
            return await _parallelLimitProvider.GetLock(parallelLimitAttributeType).WaitAsync();
        }

        return NoOpDisposable.Instance;
    }

    private async Task WaitForDependenciesAsync(ModuleBase module, IModuleScheduler scheduler)
    {
        var dependencies = module.GetModuleDependencies();

        foreach (var (dependencyType, ignoreIfNotRegistered) in dependencies)
        {
            var dependencyTask = scheduler.GetModuleCompletionTask(dependencyType);

            if (dependencyTask != null)
            {
                try
                {
                    await dependencyTask;
                }
                catch (Exception e) when (module.ModuleRunType == ModuleRunType.AlwaysRun)
                {
                    _secondaryExceptionContainer.RegisterException(new AlwaysRunPostponedException(
                        $"{dependencyType.Name} threw an exception when {module.GetType().Name} was waiting for it as a dependency",
                        e));
                    module.Context.Logger.LogError(e, "Ignoring Exception due to 'AlwaysRun' set");
                }
            }
            else if (!ignoreIfNotRegistered)
            {
                var message = $"Module '{module.GetType().Name}' depends on '{dependencyType.Name}', " +
                              $"but '{dependencyType.Name}' has not been registered in the pipeline. " +
                              $"Ensure all module dependencies are registered before executing the pipeline.";
                throw new ModuleNotRegisteredException(message, null);
            }
        }
    }
}
