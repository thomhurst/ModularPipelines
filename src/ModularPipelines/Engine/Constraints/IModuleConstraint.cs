using Microsoft.Extensions.Logging;

namespace ModularPipelines.Engine.Constraints;

/// <summary>
/// Interface for checking module execution constraints.
/// </summary>
/// <remarks>
/// Constraints determine whether a module can execute based on current system state.
/// Examples include sequential execution requirements, lock key conflicts, etc.
/// </remarks>
internal interface IModuleConstraint
{
    /// <summary>
    /// Checks if a module can execute given current state.
    /// </summary>
    /// <param name="moduleState">The module to check.</param>
    /// <param name="stateQueries">Helper for querying module states.</param>
    /// <returns>True if constraint is satisfied and module can execute, false otherwise.</returns>
    /// <remarks>
    /// This method MUST be called while holding the scheduler's state lock.
    /// </remarks>
    bool CanExecute(ModuleState moduleState, ModuleStateQueries stateQueries);

    /// <summary>
    /// Logs why the constraint blocked execution (called only when CanExecute returns false).
    /// </summary>
    /// <param name="moduleState">The module that was blocked.</param>
    /// <param name="stateQueries">Helper for querying module states.</param>
    /// <param name="logger">Logger for output.</param>
    void LogViolation(ModuleState moduleState, ModuleStateQueries stateQueries, ILogger logger);
}
