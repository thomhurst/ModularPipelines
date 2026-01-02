using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

/// <summary>
/// Manages module state transitions during pipeline execution.
/// </summary>
/// <remarks>
/// This interface encapsulates the state management responsibility previously
/// embedded in ModuleScheduler, following the Single Responsibility Principle.
/// State transitions include:
/// - Marking modules as started (Pending/Queued -> Executing)
/// - Marking modules as completed (Executing -> Completed)
/// - Cancelling pending modules during pipeline cancellation
/// </remarks>
internal interface IModuleStateTracker
{
    /// <summary>
    /// Marks a module as started execution.
    /// </summary>
    /// <param name="moduleType">The type of the module to mark as started.</param>
    /// <returns>True if the module can proceed with execution, false if constraints prevent execution.</returns>
    bool MarkModuleStarted(Type moduleType);

    /// <summary>
    /// Marks a module as completed and notifies dependents.
    /// </summary>
    /// <param name="moduleType">The type of the module to mark as completed.</param>
    /// <param name="success">True if execution succeeded, false otherwise.</param>
    /// <param name="exception">Optional exception if execution failed.</param>
    void MarkModuleCompleted(Type moduleType, bool success, Exception? exception = null);

    /// <summary>
    /// Cancels all modules that are queued or pending (not yet executing).
    /// This is used when the pipeline is cancelled to ensure TaskCompletionSources are properly completed.
    /// Note: AlwaysRun modules are not cancelled as they should be allowed to complete.
    /// </summary>
    void CancelPendingModules();

    /// <summary>
    /// Gets the state for a specific module.
    /// </summary>
    /// <param name="moduleType">The type of the module.</param>
    /// <returns>The module state if found, null otherwise.</returns>
    ModuleState? GetModuleState(Type moduleType);

    /// <summary>
    /// Gets the completion task for a specific module.
    /// </summary>
    /// <param name="moduleType">The type of the module.</param>
    /// <returns>A task that completes when the module completes, or null if the module is not found.</returns>
    Task<IModule>? GetModuleCompletionTask(Type moduleType);
}
