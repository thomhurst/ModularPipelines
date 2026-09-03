using System.Collections.Concurrent;
using Microsoft.Extensions.Options;
using ModularPipelines.Distributed;
using ModularPipelines.Distributed.Capabilities;

namespace ModularPipelines.Distributed.Coordination;

internal class InMemoryDistributedCoordinator(IOptions<DistributedOptions>? options = null) : IDistributedMasterCoordinator
{
    private readonly List<ModuleAssignment> _workQueue = [];
    private readonly SemaphoreSlim _workAvailable = new(0);
    private readonly Lock _queueLock = new();
    private readonly ConcurrentDictionary<string, TaskCompletionSource<SerializedModuleResult>> _results = new();
    private readonly ConcurrentDictionary<int, WorkerRegistration> _workers = new();
    private readonly ConcurrentDictionary<int, WorkerStatus> _workerStatuses = new();
    private readonly ConcurrentDictionary<int, DateTimeOffset> _heartbeats = new();
    private readonly TaskCompletionSource _cancellationRequested = new(
        TaskCreationOptions.RunContinuationsAsynchronously);
    private readonly TimeSpan _workerTimeout = options?.Value.WorkerTimeout ?? TimeSpan.FromSeconds(30);
    private volatile bool _completed;

    public Task EnqueueModuleAsync(ModuleAssignment assignment, CancellationToken cancellationToken)
    {
        lock (_queueLock)
        {
            _workQueue.Add(assignment);
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
                    for (var i = 0; i < _workQueue.Count; i++)
                    {
                        if (CapabilityMatcher.CanExecute(_workQueue[i], workerCapabilities))
                        {
                            var assignment = _workQueue[i];
                            _workQueue.RemoveAt(i);
                            return assignment;
                        }
                    }

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
        var initialStatus = new WorkerStatus(registration.WorkerIndex)
        {
            RunIdentifier = registration.RunIdentifier,
        };
        _workerStatuses.AddOrUpdate(
            registration.WorkerIndex,
            initialStatus,
            (_, currentStatus) => string.Equals(
                currentStatus.RunIdentifier,
                registration.RunIdentifier,
                StringComparison.Ordinal)
                ? currentStatus
                : initialStatus);
        _heartbeats[registration.WorkerIndex] = DateTimeOffset.UtcNow;
        return Task.CompletedTask;
    }

    public Task SendHeartbeatAsync(WorkerStatus status, CancellationToken cancellationToken)
    {
        _workerStatuses[status.WorkerIndex] = status;
        _heartbeats[status.WorkerIndex] = DateTimeOffset.UtcNow;

        return Task.CompletedTask;
    }

    public Task<IReadOnlyList<WorkerRegistration>> GetRegisteredWorkersAsync(CancellationToken cancellationToken)
    {
        var oldestLiveHeartbeat = DateTimeOffset.UtcNow - _workerTimeout;
        IReadOnlyList<WorkerRegistration> result =
        [
            .. _workers.Values.Where(worker =>
                WorkerStatus.IsLive(
                    _workerStatuses.GetValueOrDefault(worker.WorkerIndex),
                    _heartbeats.TryGetValue(worker.WorkerIndex, out var heartbeat)
                    && heartbeat >= oldestLiveHeartbeat)),
        ];
        return Task.FromResult(result);
    }

    public Task<IReadOnlyList<WorkerStatus>> GetWorkerStatusesAsync(CancellationToken cancellationToken)
    {
        IReadOnlyList<WorkerStatus> result = [.. _workerStatuses.Values];
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
}
