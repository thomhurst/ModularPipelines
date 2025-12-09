namespace ModularPipelines.Engine;

/// <summary>
/// Represents a point-in-time snapshot of module execution states.
/// </summary>
/// <remarks>
/// This class provides a single-pass aggregation of module states to avoid
/// multiple iterations over the module collection in hot paths like the scheduler loop.
/// </remarks>
internal class ModuleStateSnapshot
{
    /// <summary>
    /// Gets total number of modules being tracked.
    /// </summary>
    public int Total { get; init; }

    /// <summary>
    /// Gets number of modules that are queued but not yet executing.
    /// </summary>
    public int Queued { get; init; }

    /// <summary>
    /// Gets number of modules currently executing.
    /// </summary>
    public int Executing { get; init; }

    /// <summary>
    /// Gets number of modules that have completed execution.
    /// </summary>
    public int Completed { get; init; }

    /// <summary>
    /// Gets number of modules pending (not queued, executing, or completed).
    /// </summary>
    public int Pending { get; init; }

    /// <summary>
    /// Gets a value indicating whether whether all modules have completed.
    /// </summary>
    public bool AllCompleted => Completed == Total;

    /// <summary>
    /// Gets a value indicating whether whether any modules are currently active (executing or queued).
    /// </summary>
    public bool HasActiveModules => Executing > 0 || Queued > 0;

    /// <summary>
    /// Gets a value indicating whether whether any modules are pending execution.
    /// </summary>
    public bool HasPendingModules => Pending > 0;

    /// <summary>
    /// Creates a snapshot from current module states.
    /// </summary>
    public static ModuleStateSnapshot Create(IEnumerable<ModuleState> moduleStates)
    {
        var total = 0;
        var queued = 0;
        var executing = 0;
        var completed = 0;
        var pending = 0;

        foreach (var state in moduleStates)
        {
            total++;

            switch (state.State)
            {
                case ModuleExecutionState.Completed:
                    completed++;
                    break;
                case ModuleExecutionState.Executing:
                    executing++;
                    break;
                case ModuleExecutionState.Queued:
                    queued++;
                    break;
                case ModuleExecutionState.Pending:
                    pending++;
                    break;
            }
        }

        return new ModuleStateSnapshot
        {
            Total = total,
            Queued = queued,
            Executing = executing,
            Completed = completed,
            Pending = pending,
        };
    }
}
