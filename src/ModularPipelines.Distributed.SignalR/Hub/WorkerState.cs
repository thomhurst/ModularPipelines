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

    public bool TryMarkBusy() => Interlocked.CompareExchange(ref BusyFlag, 1, 0) == 0;
    public void MarkIdle() => Interlocked.Exchange(ref BusyFlag, 0);
    public bool IsIdle => Volatile.Read(ref BusyFlag) == 0;
}
