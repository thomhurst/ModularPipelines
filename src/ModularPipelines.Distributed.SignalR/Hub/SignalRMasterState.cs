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
    private readonly ConcurrentDictionary<string, SemaphoreSlim> _assignmentDeliveryFences = new();

    /// <summary>
    /// Connected workers indexed by SignalR connection ID.
    /// </summary>
    public ConcurrentDictionary<string, WorkerState> Workers { get; } = new();

    /// <summary>
    /// Worker registrations indexed by worker index.
    /// </summary>
    public ConcurrentDictionary<int, WorkerRegistration> Registrations { get; } = new();

    /// <summary>
    /// Latest heartbeat for each registered worker.
    /// </summary>
    public ConcurrentDictionary<int, DateTimeOffset> Heartbeats { get; } = new();

    /// <summary>
    /// Completes when distributed cancellation is requested.
    /// </summary>
    public TaskCompletionSource CancellationRequested { get; } = new(
        TaskCreationOptions.RunContinuationsAsynchronously);

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
    /// How long a registration remains live without a heartbeat.
    /// </summary>
    public TimeSpan WorkerTimeout { get; set; } = TimeSpan.FromSeconds(30);

    /// <summary>
    /// Volatile completion flag.
    /// </summary>
    public volatile bool IsCompleted;

    /// <summary>
    /// Signals when work is added to <see cref="PendingAssignments"/> or when completion is signalled.
    /// Used by <see cref="Coordination.SignalRMasterCoordinator.DequeueModuleAsync"/> to avoid polling.
    /// </summary>
    public SemaphoreSlim WorkAvailable { get; } = new(0);

    public PendingReconnect? TrackPendingReconnect(
        WorkerState disconnectedWorker,
        ModuleAssignment assignment)
    {
        PendingReconnect pending;
        PendingReconnect? previous;

        lock (_pendingReconnectLock)
        {
            if (ResultWaiters.TryGetValue(assignment.ModuleTypeName, out var waiter)
                && waiter.Task.IsCompleted)
            {
                return null;
            }

            _pendingReconnects.TryGetValue(assignment.ModuleTypeName, out previous);

            // A late original participant is not the owner of an active redispatch.
            // Its disconnect must not replace that claim and schedule a third execution.
            if (previous is { IsRedispatched: true }
                && previous.IsTracking(disconnectedWorker)
                && !previous.IsRedispatchClaimant(disconnectedWorker))
            {
                previous.UntrackWorker(disconnectedWorker);
                return null;
            }

            pending = new PendingReconnect(
                disconnectedWorker.Registration.WorkerIndex,
                assignment);
            var trackedWorkers = previous?.Complete() ?? [];
            pending.TrackWorkers(trackedWorkers.Where(worker => worker != disconnectedWorker));
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
        string? resumingModuleTypeName,
        out ModuleAssignment? assignment)
    {
        PendingReconnect? pending;
        PendingReconnect? completedPending = null;
        var restored = false;
        assignment = null;

        lock (_pendingReconnectLock)
        {
            pending = _pendingReconnects.Values
                .FirstOrDefault(candidate =>
                    candidate.WorkerIndex == worker.Registration.WorkerIndex
                    && candidate.Assignment.ModuleTypeName == resumingModuleTypeName);

            if (pending is not null)
            {
                if (ResultWaiters.TryGetValue(pending.Assignment.ModuleTypeName, out var waiter)
                    && !waiter.Task.IsCompleted)
                {
                    assignment = pending.Assignment;

                    if (worker.TryAssign(assignment))
                    {
                        if (pending.TryResume())
                        {
                            pending.TrackWorker(worker);
                            restored = true;
                        }
                        else
                        {
                            worker.TryCompleteAssignment(assignment.ModuleTypeName);
                            assignment = null;
                        }
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

        if (restored)
        {
            pending?.CancelDelay();
        }

        completedPending?.Dispose();
        return restored;
    }

    public bool TryClaimRedispatch(
        ModuleAssignment assignment,
        WorkerState? worker = null)
    {
        lock (_pendingReconnectLock)
        {
            if (ResultWaiters.TryGetValue(assignment.ModuleTypeName, out var waiter)
                && waiter.Task.IsCompleted)
            {
                return false;
            }

            if (!_pendingReconnects.TryGetValue(assignment.ModuleTypeName, out var pending))
            {
                return true;
            }

            if (!pending.TryClaimRedispatch())
            {
                return false;
            }

            if (worker is not null)
            {
                pending.TrackRedispatchClaimant(worker);
            }

            return true;
        }
    }

    public bool TryReturnRedispatchToQueue(
        ModuleAssignment assignment,
        WorkerState? worker = null)
    {
        lock (_pendingReconnectLock)
        {
            if (ResultWaiters.TryGetValue(assignment.ModuleTypeName, out var waiter)
                && waiter.Task.IsCompleted)
            {
                return false;
            }

            return !_pendingReconnects.TryGetValue(
                       assignment.ModuleTypeName,
                       out var pending)
                   || pending.TryReturnToQueue(worker);
        }
    }

    public async Task<IDisposable> EnterAssignmentDeliveryFenceAsync(string moduleTypeName)
    {
        var deliveryFence = _assignmentDeliveryFences.GetOrAdd(
            moduleTypeName,
            _ => new SemaphoreSlim(1, 1));
        await deliveryFence.WaitAsync();
        return new SemaphoreReleaser(deliveryFence);
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

    public async Task<IReadOnlyList<WorkerState>> CompleteResultAsync(SerializedModuleResult result)
    {
        using var deliveryFence = await EnterAssignmentDeliveryFenceAsync(result.ModuleTypeName);
        return CompleteResult(result);
    }

    private IReadOnlyList<WorkerState> CompleteResult(SerializedModuleResult result)
    {
        PendingReconnect? pending = null;
        IReadOnlyList<WorkerState> trackedWorkers = [];

        lock (_pendingReconnectLock)
        {
            var waiter = ResultWaiters.GetOrAdd(
                result.ModuleTypeName,
                static _ => new TaskCompletionSource<SerializedModuleResult>(
                    TaskCreationOptions.RunContinuationsAsynchronously));
            waiter.TrySetResult(result);

            if (_pendingReconnects.Remove(result.ModuleTypeName, out pending))
            {
                trackedWorkers = pending.Complete();
            }
        }

        pending?.CancelDelay();
        pending?.Dispose();
        return trackedWorkers;
    }

    private sealed class SemaphoreReleaser(SemaphoreSlim semaphore) : IDisposable
    {
        public void Dispose()
        {
            semaphore.Release();
        }
    }
}
