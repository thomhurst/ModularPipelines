using System.Collections.Concurrent;
using Microsoft.Extensions.Options;

namespace ModularPipelines.Distributed.Coordination;

/// <summary>
/// Process-local coordinator. Single-instance runs use it directly, and the SignalR master uses it as
/// the authoritative queue, lease and result store behind its hub.
/// </summary>
internal class InMemoryDistributedCoordinator(
    IOptions<DistributedOptions>? options = null,
    TimeProvider? timeProvider = null) : IDistributedMasterCoordinator
{
    private readonly Lock _lock = new();
    private readonly PriorityQueue<ModuleAssignment, AssignmentQueuePriority> _workQueue = new();
    private readonly Dictionary<ModuleId, LeaseEntry> _leases = [];
    private readonly Dictionary<WorkerId, WorkerRegistration> _workers = [];
    private readonly Dictionary<WorkerId, WorkerStatus> _workerStatuses = [];
    private readonly Dictionary<WorkerId, DateTimeOffset> _heartbeats = [];
    private readonly ConcurrentDictionary<ModuleId, TaskCompletionSource<SerializedModuleResult>> _results = new();
    private readonly TaskCompletionSource<DistributedCancellationReason> _cancellation = new(
        TaskCreationOptions.RunContinuationsAsynchronously);
    private readonly TimeSpan _workerTimeout = options?.Value.WorkerTimeout ?? TimeSpan.FromSeconds(30);
    private readonly TimeProvider _timeProvider = timeProvider ?? TimeProvider.System;
    private TaskCompletionSource _stateChanged = NewSignal();
    private SchedulingWorker[] _priorityWorkerSnapshot = [];
    private bool _queuePrioritiesInitialized;
    private long _enqueueSequence;
    private bool _completed;

    public Task EnqueueModuleAsync(ModuleAssignment assignment, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(assignment);
        cancellationToken.ThrowIfCancellationRequested();

        // Pre-create the waiter so results can be awaited before they are published.
        GetResultSource(assignment.ModuleId);
        lock (_lock)
        {
            var liveWorkers = RefreshQueuePrioritiesIfWorkerFleetChanged();
            _workQueue.Enqueue(
                assignment,
                AssignmentQueuePriority.Create(assignment, liveWorkers, _enqueueSequence++));
            SignalStateChangedLocked();
        }

        return Task.CompletedTask;
    }

    public async Task<ModuleLease?> DequeueModuleAsync(
        WorkerId workerId,
        IReadOnlySet<Capability> workerCapabilities,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(workerCapabilities);
        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();
            Task stateChanged;
            lock (_lock)
            {
                if (IsDequeueClosedLocked())
                {
                    return null;
                }

                var lease = TryClaimLocked(workerId, workerCapabilities);
                if (lease is not null)
                {
                    return lease;
                }

                stateChanged = _stateChanged.Task;
            }

            // Wait for the queue, fleet, completion or cancellation state to change rather than
            // spinning over assignments this worker cannot run.
            await stateChanged.WaitAsync(cancellationToken).ConfigureAwait(false);
        }
    }

    public Task PublishResultAsync(
        SerializedModuleResult result,
        ModuleLease? lease,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(result);
        cancellationToken.ThrowIfCancellationRequested();

        // The first published result is final.
        GetResultSource(result.ModuleId).TrySetResult(result);
        lock (_lock)
        {
            _leases.Remove(result.ModuleId);
            RemoveQueuedLocked(result.ModuleId);
            SignalStateChangedLocked();
        }

        return Task.CompletedTask;
    }

    public Task<SerializedModuleResult> WaitForResultAsync(ModuleId moduleId, CancellationToken cancellationToken) =>
        GetResultSource(moduleId).Task.WaitAsync(cancellationToken);

    public Task RegisterWorkerAsync(WorkerRegistration registration, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(registration);
        cancellationToken.ThrowIfCancellationRequested();
        lock (_lock)
        {
            var now = _timeProvider.GetUtcNow();
            if (_workers.TryGetValue(registration.WorkerId, out var existing)
                && existing.RegisteredAt != registration.RegisteredAt
                && _heartbeats.TryGetValue(registration.WorkerId, out var heartbeat)
                && heartbeat >= now - _workerTimeout)
            {
                throw DuplicateWorker(registration.WorkerId);
            }

            _workers[registration.WorkerId] = registration;
            if (!_workerStatuses.TryGetValue(registration.WorkerId, out var currentStatus)
                || !string.Equals(currentStatus.RunId, registration.RunId, StringComparison.Ordinal)
                || existing?.RegisteredAt != registration.RegisteredAt)
            {
                _workerStatuses[registration.WorkerId] = new WorkerStatus
                {
                    WorkerId = registration.WorkerId,
                    RunId = registration.RunId,
                };
            }

            _heartbeats[registration.WorkerId] = now;
            SignalStateChangedLocked();
        }

        return Task.CompletedTask;
    }

    public Task SendHeartbeatAsync(WorkerStatus status, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(status);
        cancellationToken.ThrowIfCancellationRequested();
        lock (_lock)
        {
            var now = _timeProvider.GetUtcNow();
            _workerStatuses[status.WorkerId] = status;
            _heartbeats[status.WorkerId] = now;
            foreach (var moduleId in status.InFlightModules)
            {
                if (_leases.TryGetValue(moduleId, out var lease) && lease.Lease.WorkerId == status.WorkerId)
                {
                    _leases[moduleId] = lease with { ExpiresAt = now + _workerTimeout };
                }
            }
        }

        return Task.CompletedTask;
    }

    public Task<DistributedCancellationReason> WaitForCancellationAsync(CancellationToken cancellationToken) =>
        _cancellation.Task.WaitAsync(cancellationToken);

    public Task<bool> WithdrawAssignmentAsync(ModuleId moduleId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        lock (_lock)
        {
            return Task.FromResult(RemoveQueuedLocked(moduleId));
        }
    }

    public Task<IReadOnlyList<ModuleLease>> GetActiveLeasesAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        lock (_lock)
        {
            IReadOnlyList<ModuleLease> leases = [.. _leases.Values.Select(static entry => entry.Lease)];
            return Task.FromResult(leases);
        }
    }

    public Task<IReadOnlyList<ModuleId>> RequeueExpiredLeasesAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        lock (_lock)
        {
            var now = _timeProvider.GetUtcNow();
            var expired = _leases.Values.Where(entry => entry.ExpiresAt < now).ToArray();
            if (expired.Length == 0)
            {
                return Task.FromResult<IReadOnlyList<ModuleId>>([]);
            }

            var requeued = new List<ModuleId>(expired.Length);
            var liveWorkers = RefreshQueuePrioritiesIfWorkerFleetChanged();
            foreach (var entry in expired)
            {
                var assignment = entry.Lease.Assignment;
                _leases.Remove(assignment.ModuleId);
                if (HasResult(assignment.ModuleId))
                {
                    continue;
                }

                _workQueue.Enqueue(
                    assignment,
                    AssignmentQueuePriority.Create(assignment, liveWorkers, _enqueueSequence++));
                requeued.Add(assignment.ModuleId);
            }

            SignalStateChangedLocked();
            return Task.FromResult<IReadOnlyList<ModuleId>>(requeued);
        }
    }

    public Task<IReadOnlyList<WorkerRegistration>> GetRegisteredWorkersAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        lock (_lock)
        {
            var oldestLiveHeartbeat = _timeProvider.GetUtcNow() - _workerTimeout;
            IReadOnlyList<WorkerRegistration> result =
            [
                .. _workers.Values.Where(worker =>
                    WorkerStatus.IsLive(
                        _workerStatuses.GetValueOrDefault(worker.WorkerId),
                        _heartbeats.TryGetValue(worker.WorkerId, out var heartbeat)
                        && heartbeat >= oldestLiveHeartbeat)),
            ];
            return Task.FromResult(result);
        }
    }

    public Task<IReadOnlyList<WorkerStatus>> GetWorkerStatusesAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        lock (_lock)
        {
            IReadOnlyList<WorkerStatus> result = [.. _workerStatuses.Values];
            return Task.FromResult(result);
        }
    }

    public Task SignalCompletionAsync(CancellationToken cancellationToken)
    {
        lock (_lock)
        {
            _completed = true;
            SignalStateChangedLocked();
        }

        return Task.CompletedTask;
    }

    public Task BroadcastCancellationAsync(
        DistributedCancellationReason reason,
        CancellationToken cancellationToken)
    {
        // The first reason is durable; a later user stop cannot hide an earlier failure, and a
        // failure after a stop does not revive AlwaysRun dispatch.
        _cancellation.TrySetResult(reason);
        lock (_lock)
        {
            SignalStateChangedLocked();
        }

        return Task.CompletedTask;
    }

    internal bool IsRegistered(WorkerId workerId, DateTimeOffset registeredAt)
    {
        lock (_lock)
        {
            return _workers.TryGetValue(workerId, out var registration)
                   && registration.RegisteredAt == registeredAt;
        }
    }

    internal static InvalidOperationException DuplicateWorker(WorkerId workerId) =>
        new($"Worker '{workerId}' is already registered by another live process. "
            + "Give every distributed process a unique instance index.");

    private bool IsDequeueClosedLocked() =>
        _completed
        || (_cancellation.Task.IsCompletedSuccessfully
            && _cancellation.Task.Result == DistributedCancellationReason.Stopped);

    private ModuleLease? TryClaimLocked(WorkerId workerId, IReadOnlySet<Capability> workerCapabilities)
    {
        var alwaysRunOnly = _cancellation.Task.IsCompletedSuccessfully;
        RefreshQueuePrioritiesIfWorkerFleetChanged();
        var skippedAssignments = new List<(ModuleAssignment Assignment, AssignmentQueuePriority Priority)>();
        try
        {
            while (_workQueue.TryDequeue(out var assignment, out var priority))
            {
                if (HasResult(assignment.ModuleId) || _leases.ContainsKey(assignment.ModuleId))
                {
                    // A result or a live claim already exists; drop the stale copy.
                    continue;
                }

                if ((alwaysRunOnly && !assignment.AlwaysRun)
                    || !assignment.RequiredCapabilities.IsSatisfiedBy(workerCapabilities))
                {
                    skippedAssignments.Add((assignment, priority));
                    continue;
                }

                var lease = new ModuleLease
                {
                    LeaseId = Guid.NewGuid().ToString("N"),
                    WorkerId = workerId,
                    Assignment = assignment,
                };
                _leases[assignment.ModuleId] = new LeaseEntry(lease, _timeProvider.GetUtcNow() + _workerTimeout);
                return lease;
            }

            return null;
        }
        finally
        {
            foreach (var (assignment, priority) in skippedAssignments)
            {
                _workQueue.Enqueue(assignment, priority);
            }
        }
    }

    private bool RemoveQueuedLocked(ModuleId moduleId)
    {
        var removed = false;
        while (_workQueue.Remove(
                   new ModuleAssignment
                   {
                       ModuleId = moduleId,
                       RequiredCapabilities = CapabilityRequirement.None,
                       PipelineSchemaVersion = string.Empty,
                   },
                   out _,
                   out _,
                   ModuleIdComparer.Instance))
        {
            removed = true;
        }

        return removed;
    }

    private bool HasResult(ModuleId moduleId) =>
        _results.TryGetValue(moduleId, out var result) && result.Task.IsCompleted;

    private TaskCompletionSource<SerializedModuleResult> GetResultSource(ModuleId moduleId) =>
        _results.GetOrAdd(
            moduleId,
            static _ => new TaskCompletionSource<SerializedModuleResult>(
                TaskCreationOptions.RunContinuationsAsynchronously));

    private void SignalStateChangedLocked()
    {
        var previous = _stateChanged;
        _stateChanged = NewSignal();
        previous.TrySetResult();
    }

    private static TaskCompletionSource NewSignal() =>
        new(TaskCreationOptions.RunContinuationsAsynchronously);

    private WorkerRegistration[] RefreshQueuePrioritiesIfWorkerFleetChanged()
    {
        var oldestLiveHeartbeat = _timeProvider.GetUtcNow() - _workerTimeout;
        var liveWorkers = _workers.Values
            .Where(worker => _heartbeats.TryGetValue(worker.WorkerId, out var heartbeat)
                             && heartbeat >= oldestLiveHeartbeat)
            .OrderBy(static worker => worker.WorkerId.Value, StringComparer.Ordinal)
            .ToArray();
        var currentSnapshot = liveWorkers
            .Select(static worker => new SchedulingWorker(
                worker.WorkerId,
                worker.Capabilities.ToHashSet()))
            .ToArray();
        if (_queuePrioritiesInitialized
            && HasSameSchedulingWorkers(_priorityWorkerSnapshot, currentSnapshot))
        {
            return liveWorkers;
        }

        var assignments = new List<(ModuleAssignment Assignment, long Sequence)>(_workQueue.Count);
        while (_workQueue.TryDequeue(out var assignment, out var priority))
        {
            assignments.Add((assignment, priority.Sequence));
        }

        foreach (var (assignment, sequence) in assignments)
        {
            _workQueue.Enqueue(assignment, AssignmentQueuePriority.Create(assignment, liveWorkers, sequence));
        }

        _priorityWorkerSnapshot = currentSnapshot;
        _queuePrioritiesInitialized = true;
        return liveWorkers;
    }

    private static bool HasSameSchedulingWorkers(
        IReadOnlyList<SchedulingWorker> previous,
        IReadOnlyList<SchedulingWorker> current)
    {
        if (previous.Count != current.Count)
        {
            return false;
        }

        for (var i = 0; i < previous.Count; i++)
        {
            if (previous[i].WorkerId != current[i].WorkerId
                || !previous[i].Capabilities.SetEquals(current[i].Capabilities))
            {
                return false;
            }
        }

        return true;
    }

    private sealed record LeaseEntry(ModuleLease Lease, DateTimeOffset ExpiresAt);

    private readonly record struct SchedulingWorker(
        WorkerId WorkerId,
        IReadOnlySet<Capability> Capabilities);

    private sealed class ModuleIdComparer : IEqualityComparer<ModuleAssignment>
    {
        public static ModuleIdComparer Instance { get; } = new();

        public bool Equals(ModuleAssignment? x, ModuleAssignment? y) => x?.ModuleId == y?.ModuleId;

        public int GetHashCode(ModuleAssignment obj) => obj.ModuleId.GetHashCode();
    }

    /// <summary>
    /// Orders assignments by priority, then scarcity of eligible workers, then requirement
    /// specificity, then critical-path weight, then enqueue order.
    /// </summary>
    private readonly record struct AssignmentQueuePriority(
        ModulePriority Priority,
        int EligibleWorkerCount,
        int RequiredCapabilityCount,
        long CriticalPathTicks,
        long Sequence) : IComparable<AssignmentQueuePriority>
    {
        public static AssignmentQueuePriority Create(
            ModuleAssignment assignment,
            IReadOnlyCollection<WorkerRegistration> workers,
            long sequence)
        {
            var eligibleWorkerCount = workers.Count == 0
                ? int.MaxValue
                : workers.Count(worker => assignment.RequiredCapabilities.IsSatisfiedBy(worker.Capabilities));

            return new AssignmentQueuePriority(
                assignment.Priority,
                eligibleWorkerCount,
                assignment.RequiredCapabilities.Clauses.Count,
                assignment.CriticalPathWeight.Ticks,
                sequence);
        }

        public int CompareTo(AssignmentQueuePriority other)
        {
            var result = other.Priority.CompareTo(Priority);
            if (result != 0)
            {
                return result;
            }

            result = EligibleWorkerCount.CompareTo(other.EligibleWorkerCount);
            if (result != 0)
            {
                return result;
            }

            result = other.RequiredCapabilityCount.CompareTo(RequiredCapabilityCount);
            if (result != 0)
            {
                return result;
            }

            result = other.CriticalPathTicks.CompareTo(CriticalPathTicks);
            return result != 0 ? result : Sequence.CompareTo(other.Sequence);
        }
    }
}

/// <summary>
/// The process-local coordinator registered when no backend is configured. It cannot share work
/// between processes, so a multi-instance run that resolves it fails at startup.
/// </summary>
internal sealed class DefaultInMemoryDistributedCoordinator(
    IOptions<DistributedOptions>? options = null,
    TimeProvider? timeProvider = null) : InMemoryDistributedCoordinator(options, timeProvider);
