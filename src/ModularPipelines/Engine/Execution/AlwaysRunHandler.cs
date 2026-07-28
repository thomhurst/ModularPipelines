using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using ModularPipelines.Helpers;
using ModularPipelines.Logging;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine.Execution;

/// <summary>
/// Responsible for handling AlwaysRun modules that must complete even after pipeline failure.
/// </summary>
internal class AlwaysRunHandler(
    IModuleRunner moduleRunner,
    IParallelLimitProvider parallelLimitProvider,
    ILogger<AlwaysRunHandler> logger) : IAlwaysRunHandler
{
    private readonly IModuleRunner _moduleRunner = moduleRunner;
    private readonly IParallelLimitProvider _parallelLimitProvider = parallelLimitProvider;
    private readonly ILogger<AlwaysRunHandler> _logger = logger;

    /// <inheritdoc />
    public async Task WaitForAlwaysRunModulesAsync(IModuleScheduler scheduler, IReadOnlyList<IModule> modules)
    {
        var alwaysRunModules = modules.Where(x => x.ModuleRunType == ModuleRunType.AlwaysRun).ToList();
        _logger.LogDebug("Found {Count} AlwaysRun modules", alwaysRunModules.Count);

        var exceptions = new ConcurrentQueue<Exception>();
        var modulesToProcess = alwaysRunModules;

        while (modulesToProcess.Count > 0)
        {
            await ProcessAlwaysRunModulesAsync(scheduler, modulesToProcess, exceptions).ConfigureAwait(false);

            // Constraint checks can defer a late start. Each pass waits for active modules,
            // then retries only modules that remained pending.
            modulesToProcess = [.. modulesToProcess.Where(module => scheduler.GetModuleState(module.GetType())?.State == ModuleExecutionState.Pending)];
        }

        if (!exceptions.IsEmpty)
        {
            throw new AggregateException("One or more AlwaysRun modules failed", exceptions);
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
