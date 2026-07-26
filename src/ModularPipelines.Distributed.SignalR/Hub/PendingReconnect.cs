namespace ModularPipelines.Distributed.SignalR.Hub;

internal enum PendingReconnectState
{
    WaitingForReconnect,
    AvailableForRedispatch,
    Resumed,
    Redispatched,
    Completed,
}

/// <summary>
/// Coordinates the ownership handoff between a reconnecting worker and a retry
/// dispatcher. A queued retry can still be reclaimed until a dispatcher claims it.
/// </summary>
internal sealed class PendingReconnect(
    int workerIndex,
    ModuleAssignment assignment) : IDisposable
{
    private readonly CancellationTokenSource _delayCancellation = new();
    private readonly HashSet<WorkerState> _trackedWorkers = [];
    private int _state = (int)PendingReconnectState.WaitingForReconnect;
    private int _disposed;

    public int WorkerIndex { get; } = workerIndex;

    public ModuleAssignment Assignment { get; } = assignment;

    public CancellationToken DelayToken => _delayCancellation.Token;

    private PendingReconnectState State =>
        (PendingReconnectState)Volatile.Read(ref _state);

    public bool TryMakeAvailableForRedispatch()
    {
        return TryTransition(
            PendingReconnectState.WaitingForReconnect,
            PendingReconnectState.AvailableForRedispatch);
    }

    public bool TryResume()
    {
        while (true)
        {
            var state = State;
            if (state == PendingReconnectState.Resumed)
            {
                return true;
            }

            if (state is not (PendingReconnectState.WaitingForReconnect
                or PendingReconnectState.AvailableForRedispatch))
            {
                return false;
            }

            if (TryTransition(state, PendingReconnectState.Resumed))
            {
                return true;
            }
        }
    }

    public bool TryClaimRedispatch()
    {
        return TryTransition(
            PendingReconnectState.AvailableForRedispatch,
            PendingReconnectState.Redispatched);
    }

    public bool TryReturnToQueue()
    {
        return TryTransition(
            PendingReconnectState.Redispatched,
            PendingReconnectState.AvailableForRedispatch);
    }

    public IReadOnlyList<WorkerState> Complete()
    {
        Interlocked.Exchange(ref _state, (int)PendingReconnectState.Completed);
        return _trackedWorkers.ToArray();
    }

    public void TrackWorker(WorkerState worker)
    {
        _trackedWorkers.Add(worker);
    }

    public void CancelDelay()
    {
        if (Volatile.Read(ref _disposed) != 0)
        {
            return;
        }

        try
        {
            _delayCancellation.Cancel();
        }
        catch (ObjectDisposedException)
        {
            // Completion won the race with reconnect cancellation.
        }
    }

    public void Dispose()
    {
        if (Interlocked.Exchange(ref _disposed, 1) == 0)
        {
            _delayCancellation.Dispose();
        }
    }

    private bool TryTransition(PendingReconnectState from, PendingReconnectState to)
    {
        return Interlocked.CompareExchange(ref _state, (int)to, (int)from) == (int)from;
    }
}
