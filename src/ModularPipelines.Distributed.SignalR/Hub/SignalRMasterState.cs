using System.Collections.Concurrent;

namespace ModularPipelines.Distributed.SignalR.Hub;

/// <summary>
/// Shared mutable state for the SignalR master coordinator and hub.
/// Thread-safe via concurrent collections and atomic operations.
/// </summary>
internal class SignalRMasterState
{
    private readonly object _pendingReconnectLock = new();
    private readonly Dictionary<string, PendingReconnect> _pendingReconnects = new();

    /// <summary>
    /// Connected workers indexed by SignalR connection ID.
    /// </summary>
    public ConcurrentDictionary<string, WorkerState> Workers { get; } = new();

    /// <summary>
    /// Worker registrations indexed by worker index.
    /// </summary>
    public ConcurrentDictionary<int, WorkerRegistration> Registrations { get; } = new();

    /// <summary>
    /// Pending work assignments waiting for an idle worker.
    /// </summary>
    public ConcurrentQueue<ModuleAssignment> PendingAssignments { get; } = new();

    /// <summary>
    /// Result waiters: module type name -> TCS that completes when the result arrives.
    /// </summary>
    public ConcurrentDictionary<string, TaskCompletionSource<SerializedModuleResult>> ResultWaiters { get; } = new();

    /// <summary>
    /// How long to wait for a disconnected worker to reconnect before re-enqueuing its
    /// in-flight work. Should exceed the client's total auto-reconnect window.
    /// </summary>
    public TimeSpan ReconnectGracePeriod { get; set; } = TimeSpan.FromSeconds(45);

    /// <summary>
    /// Volatile completion flag.
    /// </summary>
    public volatile bool IsCompleted;

    /// <summary>
    /// Signals when work is added to <see cref="PendingAssignments"/> or when completion is signalled.
    /// Used by <see cref="Coordination.SignalRMasterCoordinator.DequeueModuleAsync"/> to avoid polling.
    /// </summary>
    public SemaphoreSlim WorkAvailable { get; } = new(0);

    public PendingReconnect TrackPendingReconnect(int workerIndex, ModuleAssignment assignment)
    {
        var pending = new PendingReconnect(workerIndex, assignment);
        PendingReconnect? previous;

        lock (_pendingReconnectLock)
        {
            _pendingReconnects.TryGetValue(assignment.ModuleTypeName, out previous);
            previous?.Complete();
            _pendingReconnects[assignment.ModuleTypeName] = pending;
        }

        previous?.CancelDelay();
        previous?.Dispose();
        return pending;
    }

    public PendingReconnect? GetPendingReconnect(int workerIndex)
    {
        lock (_pendingReconnectLock)
        {
            return _pendingReconnects.Values
                .FirstOrDefault(pending => pending.WorkerIndex == workerIndex);
        }
    }

    public bool TryRestoreReconnect(
        WorkerState worker,
        out ModuleAssignment? assignment,
        out bool resumed)
    {
        PendingReconnect? pending;
        PendingReconnect? completedPending = null;
        var restored = false;
        assignment = null;
        resumed = false;

        lock (_pendingReconnectLock)
        {
            pending = _pendingReconnects.Values
                .FirstOrDefault(candidate =>
                    candidate.WorkerIndex == worker.Registration.WorkerIndex);

            if (pending is not null)
            {
                if (ResultWaiters.TryGetValue(pending.Assignment.ModuleTypeName, out var waiter)
                    && !waiter.Task.IsCompleted)
                {
                    resumed = pending.TryResume();
                    assignment = pending.Assignment;

                    if (worker.TryAssign(assignment))
                    {
                        pending.TrackWorker(worker);
                        restored = true;
                    }
                }
                else if (_pendingReconnects.Remove(
                             pending.Assignment.ModuleTypeName,
                             out completedPending))
                {
                    completedPending.Complete();
                }
            }
        }

        pending?.CancelDelay();
        completedPending?.Dispose();
        return restored;
    }

    public bool TryClaimRedispatch(ModuleAssignment assignment)
    {
        PendingReconnect? pending;
        lock (_pendingReconnectLock)
        {
            _pendingReconnects.TryGetValue(assignment.ModuleTypeName, out pending);
        }

        return pending is null || pending.TryClaimRedispatch();
    }

    public void ReturnRedispatchToQueue(ModuleAssignment assignment)
    {
        PendingReconnect? pending;
        lock (_pendingReconnectLock)
        {
            _pendingReconnects.TryGetValue(assignment.ModuleTypeName, out pending);
        }

        pending?.TryReturnToQueue();
    }

    public void CompletePendingReconnect(string moduleTypeName)
    {
        PendingReconnect? pending;
        lock (_pendingReconnectLock)
        {
            if (!_pendingReconnects.Remove(moduleTypeName, out pending))
            {
                return;
            }

            pending.Complete();
        }

        pending.CancelDelay();
        pending.Dispose();
    }

    public IReadOnlyList<WorkerState> CompleteResult(SerializedModuleResult result)
    {
        PendingReconnect? pending = null;
        IReadOnlyList<WorkerState> trackedWorkers = [];

        lock (_pendingReconnectLock)
        {
            if (ResultWaiters.TryGetValue(result.ModuleTypeName, out var waiter))
            {
                waiter.TrySetResult(result);
            }

            if (_pendingReconnects.Remove(result.ModuleTypeName, out pending))
            {
                trackedWorkers = pending.Complete();
            }
        }

        pending?.CancelDelay();
        pending?.Dispose();
        return trackedWorkers;
    }
}
