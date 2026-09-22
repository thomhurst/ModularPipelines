using System.Collections.Concurrent;

namespace ModularPipelines.Distributed.SignalR.Hub;

/// <summary>
/// Shared mutable state for the SignalR master coordinator and hub.
/// Thread-safe via concurrent collections and atomic operations.
/// </summary>
internal class SignalRMasterState
{
    private readonly Lock _pendingReconnectLock = new();
    private readonly Dictionary<ModuleId, PendingReconnect> _pendingReconnects = [];
    private readonly Dictionary<ModuleId, int> _admittedWorkerResults = [];
    private readonly ConcurrentDictionary<ModuleId, SemaphoreSlim> _assignmentDeliveryFences = new();
    private readonly ConcurrentDictionary<int, object> _workerStateLocks = new();

    /// <summary>
    /// Connected workers indexed by SignalR connection ID.
    /// </summary>
    public ConcurrentDictionary<string, WorkerState> Workers { get; } = new();

    /// <summary>
    /// Worker registrations indexed by worker index.
    /// </summary>
    public ConcurrentDictionary<int, WorkerRegistration> Registrations { get; } = new();

    /// <summary>
    /// Latest status reported by each registered worker.
    /// </summary>
    public ConcurrentDictionary<int, WorkerStatus> WorkerStatuses { get; } = new();

    /// <summary>
    /// Status received before a reconnecting connection finishes registration.
    /// Entries remain connection-scoped until registration proves worker ownership.
    /// </summary>
    public ConcurrentDictionary<string, WorkerStatus> PendingWorkerStatuses { get; } = new();

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
    /// Result waiters: module identifier -> TCS that completes when the result arrives.
    /// </summary>
    public ConcurrentDictionary<ModuleId, TaskCompletionSource<SerializedModuleResult>> ResultWaiters { get; } = new();

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

    public WorkerState? RegisterWorker(WorkerState worker)
    {
        var registration = worker.Registration;
        lock (GetWorkerStateLock(registration.WorkerIndex))
        {
            var supersededWorker = Workers.Values.FirstOrDefault(candidate =>
                candidate.Registration.WorkerIndex == registration.WorkerIndex);
            if (supersededWorker is not null)
            {
                Workers.TryRemove(supersededWorker.ConnectionId, out _);
            }

            Registrations[registration.WorkerIndex] = registration;
            Workers[worker.ConnectionId] = worker;
            var pendingStatus = PendingWorkerStatuses.TryRemove(worker.ConnectionId, out var status)
                                && IsStatusForRegistration(status, registration)
                ? status
                : null;
            var initialStatus = pendingStatus ?? new WorkerStatus(registration.WorkerIndex)
            {
                RunId = registration.RunId,
            };
            WorkerStatuses.AddOrUpdate(
                registration.WorkerIndex,
                initialStatus,
                (_, currentStatus) => string.Equals(
                    currentStatus.RunId,
                    registration.RunId,
                    StringComparison.Ordinal)
                    ? pendingStatus ?? currentStatus
                    : initialStatus);
            Heartbeats[registration.WorkerIndex] = DateTimeOffset.UtcNow;
            return supersededWorker;
        }
    }

    public void TryRecordHeartbeat(WorkerState worker, WorkerStatus status)
    {
        var registration = worker.Registration;
        lock (GetWorkerStateLock(registration.WorkerIndex))
        {
            if (!Registrations.TryGetValue(status.WorkerIndex, out var currentRegistration)
                || !ReferenceEquals(currentRegistration, registration)
                || !IsStatusForRegistration(status, registration))
            {
                return;
            }

            WorkerStatuses[status.WorkerIndex] = status;
            Heartbeats[status.WorkerIndex] = DateTimeOffset.UtcNow;
        }
    }

    internal object GetWorkerStateLock(int workerIndex) =>
        _workerStateLocks.GetOrAdd(workerIndex, static _ => new object());

    public PendingReconnect? TrackPendingReconnect(
        WorkerState disconnectedWorker,
        ModuleAssignment assignment)
    {
        PendingReconnect pending;
        PendingReconnect? previous;

        lock (_pendingReconnectLock)
        {
            if (HasAcceptedResult(assignment.ModuleId))
            {
                return null;
            }

            _pendingReconnects.TryGetValue(assignment.ModuleId, out previous);

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
            _pendingReconnects[assignment.ModuleId] = pending;
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
        ModuleId? resumingModuleId,
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
                    && candidate.Assignment.ModuleId == resumingModuleId);

            if (pending is not null)
            {
                if (ResultWaiters.TryGetValue(pending.Assignment.ModuleId, out var waiter)
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
                            worker.TryCompleteAssignment(assignment.ModuleId);
                            assignment = null;
                        }
                    }
                }
                else if (_pendingReconnects.Remove(
                             pending.Assignment.ModuleId,
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
            if (HasAcceptedResult(assignment.ModuleId))
            {
                return false;
            }

            if (!_pendingReconnects.TryGetValue(assignment.ModuleId, out var pending))
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
            if (HasAcceptedResult(assignment.ModuleId))
            {
                return false;
            }

            return !_pendingReconnects.TryGetValue(
                       assignment.ModuleId,
                       out var pending)
                   || pending.TryReturnToQueue(worker);
        }
    }

    // Called while holding _pendingReconnectLock so admission and redispatch cannot race.
    private bool HasAcceptedResult(ModuleId moduleId) =>
        _admittedWorkerResults.ContainsKey(moduleId)
        || (ResultWaiters.TryGetValue(moduleId, out var waiter) && waiter.Task.IsCompleted);

    public async Task<IDisposable> EnterAssignmentDeliveryFenceAsync(
        ModuleId moduleId,
        CancellationToken cancellationToken = default)
    {
        var deliveryFence = _assignmentDeliveryFences.GetOrAdd(
            moduleId,
            _ => new SemaphoreSlim(1, 1));
        await deliveryFence.WaitAsync(cancellationToken).ConfigureAwait(false);
        return new SemaphoreReleaser(deliveryFence);
    }

    public void CompletePendingReconnect(ModuleId moduleId)
    {
        PendingReconnect? pending;
        lock (_pendingReconnectLock)
        {
            if (!_pendingReconnects.Remove(moduleId, out pending))
            {
                return;
            }

            pending.Complete();
        }

        pending.CancelDelay();
        pending.Dispose();
    }

    public async Task<IReadOnlyList<WorkerState>> CompleteResultAsync(
        SerializedModuleResult result,
        CancellationToken cancellationToken = default)
    {
        using var deliveryFence = await EnterAssignmentDeliveryFenceAsync(result.ModuleId, cancellationToken)
            .ConfigureAwait(false);
        return CompleteResult(result);
    }

    public async Task<(bool Accepted, IReadOnlyList<WorkerState> WorkersToRelease)>
        TryCompleteWorkerResultAsync(WorkerState worker, SerializedModuleResult result)
    {
        // Admit the result while the connection owns the assignment. A replacement may
        // revoke later submissions, but must not discard a result already awaiting delivery.
        var workerIndex = worker.Registration.WorkerIndex;
        lock (GetWorkerStateLock(workerIndex))
        {
            if (workerIndex != result.WorkerIndex
                || !string.Equals(
                    worker.CurrentAssignment?.ModuleId,
                    result.ModuleId,
                    StringComparison.Ordinal)
                || !Workers.TryGetValue(worker.ConnectionId, out var currentWorker)
                || !ReferenceEquals(currentWorker, worker)
                || !Registrations.TryGetValue(workerIndex, out var currentRegistration)
                || !ReferenceEquals(currentRegistration, worker.Registration))
            {
                return (false, []);
            }

            // Fence waits must not let reconnect recovery claim an already admitted result.
            lock (_pendingReconnectLock)
            {
                _admittedWorkerResults.TryGetValue(result.ModuleId, out var admissions);
                _admittedWorkerResults[result.ModuleId] = admissions + 1;
            }
        }

        try
        {
            using var deliveryFence = await EnterAssignmentDeliveryFenceAsync(result.ModuleId)
                .ConfigureAwait(false);
            return (true, CompleteResult(result));
        }
        finally
        {
            // A failed fence acquisition or result delivery must not leave a reservation
            // that prevents a later publication or reconnect recovery from making progress.
            lock (_pendingReconnectLock)
            {
                if (_admittedWorkerResults.TryGetValue(result.ModuleId, out var admissions) && admissions > 1)
                {
                    _admittedWorkerResults[result.ModuleId] = admissions - 1;
                }
                else
                {
                    _admittedWorkerResults.Remove(result.ModuleId);
                }
            }
        }
    }

    private IReadOnlyList<WorkerState> CompleteResult(SerializedModuleResult result)
    {
        PendingReconnect? pending = null;
        IReadOnlyList<WorkerState> trackedWorkers = [];

        lock (_pendingReconnectLock)
        {
            var waiter = ResultWaiters.GetOrAdd(
                result.ModuleId,
                static _ => new TaskCompletionSource<SerializedModuleResult>(
                    TaskCreationOptions.RunContinuationsAsynchronously));
            waiter.TrySetResult(result);
            _admittedWorkerResults.Remove(result.ModuleId);

            if (_pendingReconnects.Remove(result.ModuleId, out pending))
            {
                trackedWorkers = pending.Complete();
            }
        }

        pending?.CancelDelay();
        pending?.Dispose();
        return trackedWorkers;
    }

    private static bool IsStatusForRegistration(
        WorkerStatus status,
        WorkerRegistration registration) =>
        status.WorkerIndex == registration.WorkerIndex
        && string.Equals(
            status.RunId,
            registration.RunId,
            StringComparison.Ordinal);

    private sealed class SemaphoreReleaser(SemaphoreSlim semaphore) : IDisposable
    {
        public void Dispose()
        {
            semaphore.Release();
        }
    }
}
