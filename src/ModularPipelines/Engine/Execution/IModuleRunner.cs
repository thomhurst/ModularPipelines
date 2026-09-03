namespace ModularPipelines.Engine.Execution;

/// <summary>
/// Responsible for executing a single module with proper scoping, dependency waiting,
/// lifecycle events, and result registration.
/// </summary>
internal interface IModuleRunner
{
    /// <summary>
    /// Executes a module, waiting for its dependencies first.
    /// </summary>
    /// <param name="moduleState">The state of the module to execute.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task ExecuteAsync(ModuleState moduleState, CancellationToken cancellationToken);

    /// <summary>
    /// Executes a module without waiting for dependencies.
    /// Used for late-started AlwaysRun modules where dependencies may never complete.
    /// </summary>
    /// <param name="moduleState">The state of the module to execute.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task ExecuteWithoutDependencyWaitAsync(ModuleState moduleState, CancellationToken cancellationToken);
}
