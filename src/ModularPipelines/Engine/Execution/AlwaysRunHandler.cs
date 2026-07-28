using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Helpers;
using ModularPipelines.Logging;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;

namespace ModularPipelines.Engine.Execution;

/// <summary>
/// Responsible for handling AlwaysRun modules that must complete even after pipeline failure.
/// </summary>
internal class AlwaysRunHandler(
    IModuleRunner moduleRunner,
    IParallelLimitProvider parallelLimitProvider,
    IOptions<PipelineOptions> pipelineOptions,
    ILogger<AlwaysRunHandler> logger) : IAlwaysRunHandler
{
    private readonly IModuleRunner _moduleRunner = moduleRunner;
    private readonly IParallelLimitProvider _parallelLimitProvider = parallelLimitProvider;
    private readonly TimeSpan _schedulerProgressTimeout = pipelineOptions.Value.DefaultModuleTimeout;
    private readonly ILogger<AlwaysRunHandler> _logger = logger;

    /// <inheritdoc />
    public async Task WaitForAlwaysRunModulesAsync(IModuleScheduler scheduler, IReadOnlyList<IModule> modules)
    {
        var alwaysRunModules = modules.Where(x => x.ModuleRunType == ModuleRunType.AlwaysRun).ToList();
        _logger.LogDebug("Found {Count} AlwaysRun modules", alwaysRunModules.Count);

        var exceptions = new ConcurrentQueue<Exception>();
        var remainingModules = alwaysRunModules;

        while (remainingModules.Count > 0)
        {
            var modulesToProcess = GetDependencyReadyModules(scheduler, remainingModules);
            if (modulesToProcess.Count == 0)
            {
                exceptions.Enqueue(new InvalidOperationException(
                    "AlwaysRun modules could not make progress because their dependency graph has no ready modules."));
                break;
            }

            await ProcessAlwaysRunModulesAsync(scheduler, modulesToProcess, exceptions).ConfigureAwait(false);

            var deferredModules = modulesToProcess
                .Where(module => scheduler.GetModuleState(module.GetType())?.State == ModuleExecutionState.Pending)
                .ToList();
            var processedModules = modulesToProcess.Except(deferredModules).ToHashSet();
            remainingModules.RemoveAll(processedModules.Contains);

            if (deferredModules.Count > 0 &&
                !await WaitForSchedulerProgressAsync(scheduler, modules, deferredModules, exceptions).ConfigureAwait(false))
            {
                break;
            }
        }

        if (!exceptions.IsEmpty)
        {
            throw new AggregateException("One or more AlwaysRun modules failed", exceptions);
        }
    }

    private static List<IModule> GetDependencyReadyModules(
        IModuleScheduler scheduler,
        IReadOnlyCollection<IModule> remainingModules)
    {
        var remainingModuleTypes = remainingModules.Select(module => module.GetType()).ToHashSet();

        return
        [
            .. remainingModules.Where(module =>
            {
                var moduleState = scheduler.GetModuleState(module.GetType());
                return moduleState == null ||
                       moduleState.Dependencies.Keys.All(dependencyType => !remainingModuleTypes.Contains(dependencyType));
            }),
        ];
    }

    private async Task<bool> WaitForSchedulerProgressAsync(
        IModuleScheduler scheduler,
        IReadOnlyCollection<IModule> modules,
        IReadOnlyCollection<IModule> deferredModules,
        ConcurrentQueue<Exception> exceptions)
    {
        var deferredModuleTypes = deferredModules.Select(module => module.GetType()).ToHashSet();
        var deferredModuleNames = string.Join(", ", deferredModuleTypes.Select(type => type.Name));
        var activeModuleTasks = modules
            .Select(module => scheduler.GetModuleState(module.GetType()))
            .Where(state => state is { State: ModuleExecutionState.Executing } &&
                            !deferredModuleTypes.Contains(state.ModuleType))
            .Select(state => scheduler.GetModuleCompletionTask(state!.ModuleType))
            .Where(task => task != null)
            .Select(task => task!)
            .ToList();

        if (activeModuleTasks.Count == 0)
        {
            var exception = new InvalidOperationException(
                $"AlwaysRun modules were deferred with no active module able to release their constraints: {deferredModuleNames}.");
            _logger.LogWarning(exception, "AlwaysRun modules could not be retried");
            exceptions.Enqueue(exception);
            return false;
        }

        try
        {
            var schedulerProgress = Task.WhenAny(activeModuleTasks);
            var completedTask = _schedulerProgressTimeout > TimeSpan.Zero
                ? await schedulerProgress.WaitAsync(_schedulerProgressTimeout).ConfigureAwait(false)
                : await schedulerProgress.ConfigureAwait(false);

            _ = completedTask.Exception;
            return true;
        }
        catch (TimeoutException timeoutException)
        {
            var exception = new TimeoutException(
                $"Timed out waiting for scheduler progress before retrying AlwaysRun modules: {deferredModuleNames}.",
                timeoutException);
            _logger.LogWarning(exception, "AlwaysRun modules could not be retried");
            exceptions.Enqueue(exception);
            return false;
        }
    }

    private async Task ProcessAlwaysRunModulesAsync(
        IModuleScheduler scheduler,
        IReadOnlyCollection<IModule> modules,
        ConcurrentQueue<Exception> exceptions)
    {
        var parallelOptions = new ParallelOptions
        {
            MaxDegreeOfParallelism = _parallelLimitProvider.GetMaxDegreeOfParallelism(),
        };

        await Parallel.ForEachAsync(
            modules,
            parallelOptions,
            async (module, _) =>
            {
                var exception = await WaitForSingleAlwaysRunModuleAsync(scheduler, module).ConfigureAwait(false);
                if (exception != null)
                {
                    exceptions.Enqueue(exception);
                }
            }).ConfigureAwait(false);
    }

    private async Task<Exception?> WaitForSingleAlwaysRunModuleAsync(IModuleScheduler scheduler, IModule module)
    {
        var moduleType = module.GetType();
        var moduleState = scheduler.GetModuleState(moduleType);
        var moduleTask = scheduler.GetModuleCompletionTask(moduleType);

        if (moduleTask == null || moduleState == null)
        {
            return null;
        }

        // If the AlwaysRun module is still pending (never started), execute it now
        // Skip dependency waiting to prevent deadlocks - dependencies may never complete
        if (moduleState.State == ModuleExecutionState.Pending)
        {
            _logger.LogDebug("Starting pending AlwaysRun module: {ModuleName}", moduleType.Name);

            try
            {
                await _moduleRunner.ExecuteWithoutDependencyWaitAsync(moduleState, scheduler, CancellationToken.None).ConfigureAwait(false);

                if (moduleState.State == ModuleExecutionState.Pending)
                {
                    _logger.LogDebug(
                        "AlwaysRun module {ModuleName} was deferred and will be retried",
                        moduleType.Name);
                    return null;
                }

                _logger.LogDebug("AlwaysRun module {ModuleName} completed after late start", moduleType.Name);
            }
            catch (Exception alwaysRunEx)
            {
                _logger.LogWarning(alwaysRunEx, "AlwaysRun module {ModuleName} failed after late start",
                    moduleType.Name);
                return alwaysRunEx;
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
                return alwaysRunEx;
            }
        }
        else
        {
            _logger.LogDebug("Skipping AlwaysRun module {ModuleName} (State={State})",
                moduleType.Name, moduleState.State);
        }

        return null;
    }

    private static bool ShouldWaitForAlwaysRunModule(ModuleState moduleState)
    {
        return moduleState.State == ModuleExecutionState.Executing || moduleState.State == ModuleExecutionState.Completed;
    }
}
