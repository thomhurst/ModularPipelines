namespace ModularPipelines.Engine;

/// <summary>
/// Maintains scheduler state counts while the scheduler state lock is held.
/// </summary>
internal sealed class ModuleStateCounters
{
    public int Total { get; private set; }

    public int Queued { get; private set; }

    public int Executing { get; private set; }

    public int Completed { get; private set; }

    public int Pending { get; private set; }

    public void AddPendingModule()
    {
        Total++;
        Pending++;
    }

    public void Transition(ModuleExecutionState from, ModuleExecutionState to)
    {
        if (from == to)
        {
            return;
        }

        Decrement(from);
        Increment(to);
    }

    public ModuleStateSnapshot CreateSnapshot()
    {
        return new ModuleStateSnapshot
        {
            Total = Total,
            Queued = Queued,
            Executing = Executing,
            Completed = Completed,
            Pending = Pending,
        };
    }

    private void Increment(ModuleExecutionState state)
    {
        switch (state)
        {
            case ModuleExecutionState.Pending:
                Pending++;
                break;
            case ModuleExecutionState.Queued:
                Queued++;
                break;
            case ModuleExecutionState.Executing:
                Executing++;
                break;
            case ModuleExecutionState.Completed:
                Completed++;
                break;
        }
    }

    private void Decrement(ModuleExecutionState state)
    {
        switch (state)
        {
            case ModuleExecutionState.Pending:
                Pending--;
                break;
            case ModuleExecutionState.Queued:
                Queued--;
                break;
            case ModuleExecutionState.Executing:
                Executing--;
                break;
            case ModuleExecutionState.Completed:
                Completed--;
                break;
        }
    }
}
