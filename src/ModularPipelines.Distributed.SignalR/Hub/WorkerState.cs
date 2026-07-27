namespace ModularPipelines.Distributed.SignalR.Hub;

/// <summary>
/// Tracks the state of a connected worker in the master hub.
/// </summary>
internal class WorkerState
{
    public required string ConnectionId { get; init; }

    public required WorkerRegistration Registration { get; init; }

    /// <summary>
    /// 0 = idle, 1 = busy. Updated via <see cref="System.Threading.Interlocked.CompareExchange(ref int, int, int)"/>.
    /// </summary>
    private int _busyFlag;

    private ModuleAssignment? _currentAssignment;

    public bool IsIdle => Volatile.Read(ref _busyFlag) == 0;

    /// <summary>
    /// The assignment this worker is currently executing, if any. Set when work is
    /// dispatched to the worker and cleared when its result is published. Read on
    /// disconnect so in-flight work can be re-queued instead of lost.
    /// </summary>
    public ModuleAssignment? CurrentAssignment => Volatile.Read(ref _currentAssignment);

    /// <summary>
    /// Atomically claims this worker and records the assignment.
    /// </summary>
    public bool TryAssign(ModuleAssignment assignment)
    {
        if (Interlocked.CompareExchange(ref _busyFlag, 1, 0) != 0)
        {
            return false;
        }

        if (Interlocked.CompareExchange(ref _currentAssignment, assignment, null) is null)
        {
            return true;
        }

        Interlocked.Exchange(ref _busyFlag, 0);
        return false;
    }

    /// <summary>
    /// Atomically clears and returns the current assignment.
    /// </summary>
    public ModuleAssignment? ClearAssignment() => Interlocked.Exchange(ref _currentAssignment, null);

    /// <summary>
    /// Clears this worker only when the result belongs to its tracked assignment.
    /// </summary>
    public bool TryCompleteAssignment(string moduleTypeName)
    {
        while (true)
        {
            var assignment = CurrentAssignment;
            if (assignment is null
                || !string.Equals(
                    assignment.ModuleTypeName,
                    moduleTypeName,
                    StringComparison.Ordinal))
            {
                return false;
            }

            if (Interlocked.CompareExchange(ref _currentAssignment, null, assignment) == assignment)
            {
                Interlocked.Exchange(ref _busyFlag, 0);
                return true;
            }
        }
    }
}
