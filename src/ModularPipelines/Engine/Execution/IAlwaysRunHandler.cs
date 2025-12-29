using ModularPipelines.Modules;

namespace ModularPipelines.Engine.Execution;

/// <summary>
/// Responsible for handling AlwaysRun modules that must complete even after pipeline failure.
/// </summary>
internal interface IAlwaysRunHandler
{
    /// <summary>
    /// Waits for all AlwaysRun modules to complete, executing any that are still pending.
    /// </summary>
    /// <param name="scheduler">The scheduler managing module execution.</param>
    /// <param name="modules">All modules in the pipeline.</param>
    Task WaitForAlwaysRunModulesAsync(IModuleScheduler scheduler, IReadOnlyList<IModule> modules);
}
