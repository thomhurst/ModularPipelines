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
    private bool _previousSnapshotWasIdle;

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
            _previousSnapshotWasIdle = false;
            return true;
        }

        // A module can be returned from Queued to Pending when its execution-time
        // constraints change. Give that transient state one scheduling cycle to
        // requeue before treating consecutive idle snapshots as a deadlock.
        var isIdle = IsDeadlocked(snapshot, queuedCount);
        var shouldExit = isIdle && _previousSnapshotWasIdle;
        _previousSnapshotWasIdle = isIdle;

        return shouldExit;
    }

    /// <summary>
    /// Detects a potential deadlock condition: modules are pending but cannot make progress.
    /// </summary>
    /// <param name="snapshot">Current state snapshot.</param>
    /// <param name="queuedCount">Number of modules queued in this iteration.</param>
    /// <returns>True if the current snapshot is idle with pending work, false otherwise.</returns>
    /// <remarks>
    /// A potential deadlock occurs when:
    /// - No modules are currently active (executing or queued)
    /// - Modules are still pending (waiting for dependencies)
    /// - No modules could be queued (dependencies will never complete)
    ///
    /// Two consecutive potential deadlocks indicate circular dependencies or missing
    /// module registrations. A single idle snapshot can occur while a deferred module
    /// transitions from queued back to pending.
    /// </remarks>
    public bool IsDeadlocked(ModuleStateSnapshot snapshot, int queuedCount)
    {
        return !snapshot.HasActiveModules &&
               snapshot.HasPendingModules &&
               queuedCount == 0;
    }
}
