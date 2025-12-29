using Microsoft.Extensions.Logging;
using ModularPipelines.Logging;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine.Execution;

/// <summary>
/// Responsible for handling AlwaysRun modules that must complete even after pipeline failure.
/// </summary>
internal class AlwaysRunHandler : IAlwaysRunHandler
{
    private readonly IModuleRunner _moduleRunner;
    private readonly ILogger<AlwaysRunHandler> _logger;

    public AlwaysRunHandler(
        IModuleRunner moduleRunner,
        ILogger<AlwaysRunHandler> logger)
    {
        _moduleRunner = moduleRunner;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task WaitForAlwaysRunModulesAsync(IModuleScheduler scheduler, IReadOnlyList<IModule> modules)
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
                await _moduleRunner.ExecuteWithoutDependencyWaitAsync(moduleState, scheduler, CancellationToken.None).ConfigureAwait(false);
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
}
