namespace ModularPipelines.Engine.Execution;

/// <summary>
/// Responsible for waiting for module dependencies to complete before execution.
/// Handles AlwaysRun modules that should continue despite dependency failures.
/// </summary>
internal interface IDependencyWaiter
{
    /// <summary>
    /// Waits for all dependencies of the specified module to complete.
    /// </summary>
    /// <param name="moduleState">The state of the module waiting for dependencies.</param>
    /// <param name="scheduler">The scheduler to get dependency completion tasks from.</param>
    /// <param name="scopedServiceProvider">The scoped service provider for logging.</param>
    Task WaitForDependenciesAsync(ModuleState moduleState, IModuleScheduler scheduler, IServiceProvider scopedServiceProvider);
}
