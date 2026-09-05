using System.Collections.Concurrent;
using Microsoft.Extensions.Options;
using ModularPipelines.Distributed;
using ModularPipelines.Distributed.Capabilities;

namespace ModularPipelines.Distributed.Coordination;

internal class InMemoryDistributedCoordinator(IOptions<DistributedOptions>? options = null) : IDistributedMasterCoordinator
{
    private readonly PriorityQueue<ModuleAssignment, AssignmentQueuePriority> _workQueue = new();
    private readonly SemaphoreSlim _workAvailable = new(0);
    private readonly Lock _queueLock = new();
    private readonly ConcurrentDictionary<string, TaskCompletionSource<SerializedModuleResult>> _results = new();
    private readonly ConcurrentDictionary<int, WorkerRegistration> _workers = new();
    private readonly ConcurrentDictionary<int, DateTimeOffset> _heartbeats = new();
    private readonly TaskCompletionSource _cancellationRequested = new(
        TaskCreationOptions.RunContinuationsAsynchronously);
    private readonly TimeSpan _workerTimeout = options?.Value.WorkerTimeout ?? TimeSpan.FromSeconds(30);
    private SchedulingWorker[] _priorityWorkerSnapshot = [];
    private bool _queuePrioritiesInitialized;
    private long _enqueueSequence;
    private volatile bool _completed;

    public Task EnqueueModuleAsync(ModuleAssignment assignment, CancellationToken cancellationToken)
    {
        lock (_queueLock)
        {
            var liveWorkers = RefreshQueuePrioritiesIfWorkerFleetChanged();
            _workQueue.Enqueue(
                assignment,
                AssignmentQueuePriority.Create(
                    assignment,
                    liveWorkers,
                    _enqueueSequence++));
        }

        _workAvailable.Release();

        // Pre-create the result TCS so WaitForResultAsync can be called before the result is published
        _results.GetOrAdd(assignment.ModuleTypeName, _ => new TaskCompletionSource<SerializedModuleResult>());
        return Task.CompletedTask;
    }

    public async Task<ModuleAssignment?> DequeueModuleAsync(IReadOnlySet<Capability> workerCapabilities, CancellationToken cancellationToken)
    {
        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await _workAvailable.WaitAsync(cancellationToken);

                if (_completed)
                {
                    // Wake the next waiting worker so they also see completion
                    _workAvailable.Release();
                    return null;
                }

                lock (_queueLock)
                {
                    RefreshQueuePrioritiesIfWorkerFleetChanged();
                    var skippedAssignments = new List<(ModuleAssignment Assignment, AssignmentQueuePriority Priority)>();
                    var queuedCount = _workQueue.Count;
                    for (var i = 0; i < queuedCount; i++)
                    {
                        if (!_workQueue.TryDequeue(out var assignment, out var priority))
                        {
                            break;
                        }

                        if (CapabilityMatcher.CanExecute(assignment, workerCapabilities))
                        {
                            RestoreSkippedAssignments(skippedAssignments);
                            return assignment;
                        }

                        skippedAssignments.Add((assignment, priority));
                    }

                    RestoreSkippedAssignments(skippedAssignments);

                    // No matching assignment found — the semaphore count was consumed but
                    // the item that triggered it didn't match our capabilities.
                    // Another worker with the right capabilities will pick it up.
                    // Release the semaphore back so other workers can try.
                    if (_workQueue.Count > 0)
                    {
                        _workAvailable.Release();
                    }
                }
            }
        }
        catch (OperationCanceledException)
        {
            // Expected when cancellation is requested
        }

        return null;
    }

    public Task PublishResultAsync(SerializedModuleResult result, CancellationToken cancellationToken)
    {
        var tcs = _results.GetOrAdd(result.ModuleTypeName, _ => new TaskCompletionSource<SerializedModuleResult>());
        tcs.TrySetResult(result);
        return Task.CompletedTask;
    }

    public async Task<SerializedModuleResult> WaitForResultAsync(string moduleTypeName, CancellationToken cancellationToken)
    {
        var tcs = _results.GetOrAdd(moduleTypeName, _ => new TaskCompletionSource<SerializedModuleResult>());
        using var reg = cancellationToken.Register(() => tcs.TrySetCanceled(cancellationToken));
        return await tcs.Task;
    }

    public Task RegisterWorkerAsync(WorkerRegistration registration, CancellationToken cancellationToken)
    {
        _workers[registration.WorkerIndex] = registration;
        _heartbeats[registration.WorkerIndex] = DateTimeOffset.UtcNow;
        return Task.CompletedTask;
    }

    public Task SendHeartbeatAsync(int workerIndex, CancellationToken cancellationToken)
    {
        if (_workers.ContainsKey(workerIndex))
        {
            _heartbeats[workerIndex] = DateTimeOffset.UtcNow;
        }

        return Task.CompletedTask;
    }

    public Task<IReadOnlyList<WorkerRegistration>> GetRegisteredWorkersAsync(CancellationToken cancellationToken)
    {
        var oldestLiveHeartbeat = DateTimeOffset.UtcNow - _workerTimeout;
        IReadOnlyList<WorkerRegistration> result =
        [
            .. _workers.Values.Where(worker =>
                worker.UnattributedCommandCount.HasValue
                || (_heartbeats.TryGetValue(worker.WorkerIndex, out var heartbeat)
                    && heartbeat >= oldestLiveHeartbeat)),
        ];
        return Task.FromResult(result);
    }

    public Task SignalCompletionAsync(CancellationToken cancellationToken)
    {
        _completed = true;
        _workAvailable.Release();
        return Task.CompletedTask;
    }

    public Task BroadcastCancellationAsync(CancellationToken cancellationToken)
    {
        _cancellationRequested.TrySetResult();
        return Task.CompletedTask;
    }

    public Task WaitForCancellationAsync(CancellationToken cancellationToken) =>
        _cancellationRequested.Task.WaitAsync(cancellationToken);

    private void RestoreSkippedAssignments(
        IEnumerable<(ModuleAssignment Assignment, AssignmentQueuePriority Priority)> assignments)
    {
        foreach (var (assignment, priority) in assignments)
        {
            _workQueue.Enqueue(assignment, priority);
        }
    }

    private WorkerRegistration[] RefreshQueuePrioritiesIfWorkerFleetChanged()
    {
        var liveWorkers = GetLiveWorkersForScheduling()
            .OrderBy(static worker => worker.WorkerIndex)
            .ToArray();
        var currentSnapshot = liveWorkers
            .Select(static worker => new SchedulingWorker(
                worker.WorkerIndex,
                worker.Capabilities.ToHashSet()))
            .ToArray();
        if (_queuePrioritiesInitialized
            && HasSameSchedulingWorkers(_priorityWorkerSnapshot, currentSnapshot))
        {
            return liveWorkers;
        }

        RefreshQueuePriorities(liveWorkers);
        _priorityWorkerSnapshot = currentSnapshot;
        _queuePrioritiesInitialized = true;
        return liveWorkers;
    }

    private void RefreshQueuePriorities(IReadOnlyCollection<WorkerRegistration> liveWorkers)
    {
        var assignments = new List<(ModuleAssignment Assignment, long Sequence)>(_workQueue.Count);
        while (_workQueue.TryDequeue(out var assignment, out var priority))
        {
            assignments.Add((assignment, priority.Sequence));
        }

        foreach (var (assignment, sequence) in assignments)
        {
            _workQueue.Enqueue(
                assignment,
                AssignmentQueuePriority.Create(assignment, liveWorkers, sequence));
        }
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
            if (previous[i].WorkerIndex != current[i].WorkerIndex
                || !previous[i].Capabilities.SetEquals(current[i].Capabilities))
            {
                return false;
            }
        }

        return true;
    }

    private IEnumerable<WorkerRegistration> GetLiveWorkersForScheduling()
    {
        var oldestLiveHeartbeat = DateTimeOffset.UtcNow - _workerTimeout;
        return _workers.Values.Where(worker =>
            _heartbeats.TryGetValue(worker.WorkerIndex, out var heartbeat)
            && heartbeat >= oldestLiveHeartbeat);
    }

    private readonly record struct SchedulingWorker(
        int WorkerIndex,
        IReadOnlySet<Capability> Capabilities);

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
                : workers.Count(worker => CapabilityMatcher.CanExecute(assignment, worker));

            return new AssignmentQueuePriority(
                assignment.Priority,
                eligibleWorkerCount,
                assignment.RequiredCapabilities.Count,
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
