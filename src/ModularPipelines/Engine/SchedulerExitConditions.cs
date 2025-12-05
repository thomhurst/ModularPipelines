namespace ModularPipelines.Engine;

/// <summary>
/// Determines when the module scheduler should exit its execution loop.
/// </summary>
/// <remarks>
/// Extracted for testability and clarity. Contains the logic for detecting
/// completion and deadlock conditions in the scheduler.
/// </remarks>
internal class SchedulerExitConditions
{
    /// <summary>
    /// Determines if the scheduler should exit based on current state.
    /// </summary>
    /// <param name="snapshot">Current state snapshot.</param>
    /// <param name="queuedCount">Number of modules queued in this iteration.</param>
    /// <returns>True if scheduler should exit, false if it should continue.</returns>
    public bool ShouldExit(ModuleStateSnapshot snapshot, int queuedCount)
    {
        // Exit if all work is done
        if (snapshot.AllCompleted)
        {
            return true;
        }

        // Exit if we're deadlocked (nothing active, work pending, but nothing could be queued)
        if (IsDeadlocked(snapshot, queuedCount))
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Detects deadlock condition: modules are pending but cannot make progress.
    /// </summary>
    /// <param name="snapshot">Current state snapshot.</param>
    /// <param name="queuedCount">Number of modules queued in this iteration.</param>
    /// <returns>True if deadlocked, false otherwise.</returns>
    /// <remarks>
    /// Deadlock occurs when:
    /// - No modules are currently active (executing or queued)
    /// - Modules are still pending (waiting for dependencies)
    /// - No modules could be queued (dependencies will never complete)
    ///
    /// This typically indicates circular dependencies or missing module registrations.
    /// </remarks>
    public bool IsDeadlocked(ModuleStateSnapshot snapshot, int queuedCount)
    {
        return !snapshot.HasActiveModules &&
               snapshot.HasPendingModules &&
               queuedCount == 0;
    }
}
