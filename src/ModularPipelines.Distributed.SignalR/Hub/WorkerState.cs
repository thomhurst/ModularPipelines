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
    public int BusyFlag;

    private ModuleAssignment? _currentAssignment;

    public bool TryMarkBusy() => Interlocked.CompareExchange(ref BusyFlag, 1, 0) == 0;
    public void MarkIdle() => Interlocked.Exchange(ref BusyFlag, 0);
    public bool IsIdle => Volatile.Read(ref BusyFlag) == 0;

    /// <summary>
    /// The assignment this worker is currently executing, if any. Set when work is
    /// dispatched to the worker and cleared when its result is published. Read on
    /// disconnect so in-flight work can be re-queued instead of lost.
    /// </summary>
    public ModuleAssignment? CurrentAssignment => Volatile.Read(ref _currentAssignment);

    public void SetAssignment(ModuleAssignment assignment) => Volatile.Write(ref _currentAssignment, assignment);

    /// <summary>
    /// Atomically clears and returns the current assignment.
    /// </summary>
    public ModuleAssignment? ClearAssignment() => Interlocked.Exchange(ref _currentAssignment, null);
}
