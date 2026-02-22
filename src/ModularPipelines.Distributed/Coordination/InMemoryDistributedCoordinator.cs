using System.Collections.Concurrent;
using ModularPipelines.Distributed;

namespace ModularPipelines.Distributed.Coordination;

internal class InMemoryDistributedCoordinator : IDistributedCoordinator
{
    private readonly List<ModuleAssignment> _workQueue = [];
    private readonly SemaphoreSlim _workAvailable = new(0);
    private readonly Lock _queueLock = new();
    private readonly ConcurrentDictionary<string, TaskCompletionSource<SerializedModuleResult>> _results = new();
    private readonly ConcurrentDictionary<int, WorkerRegistration> _workers = new();
    private readonly ConcurrentDictionary<int, WorkerHeartbeat> _heartbeats = new();
    private volatile bool _queueCompleted;
    private volatile CancellationSignal? _cancellationSignal;

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

    public async Task<ModuleAssignment?> DequeueModuleAsync(IReadOnlySet<string> workerCapabilities, CancellationToken cancellationToken)
    {
        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await _workAvailable.WaitAsync(cancellationToken);

                lock (_queueLock)
                {
                    for (var i = 0; i < _workQueue.Count; i++)
                    {
                        if (_workQueue[i].RequiredCapabilities.IsSubsetOf(workerCapabilities))
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
        return Task.CompletedTask;
    }

    public Task SendHeartbeatAsync(int workerIndex, CancellationToken cancellationToken)
    {
        _heartbeats[workerIndex] = new WorkerHeartbeat(workerIndex, DateTimeOffset.UtcNow, null);

        if (_workers.TryGetValue(workerIndex, out var existing))
        {
            _workers[workerIndex] = existing with
            {
                Status = existing.Status == WorkerStatus.Connected ? WorkerStatus.Active : existing.Status,
            };
        }

        return Task.CompletedTask;
    }

    public Task<IReadOnlyList<WorkerRegistration>> GetRegisteredWorkersAsync(CancellationToken cancellationToken)
    {
        IReadOnlyList<WorkerRegistration> result = [.. _workers.Values];
        return Task.FromResult(result);
    }

    public Task<WorkerHeartbeat?> GetLastHeartbeatAsync(int workerIndex, CancellationToken cancellationToken)
    {
        _heartbeats.TryGetValue(workerIndex, out var heartbeat);
        return Task.FromResult(heartbeat);
    }

    public Task UpdateWorkerStatusAsync(int workerIndex, WorkerStatus status, CancellationToken cancellationToken)
    {
        if (_workers.TryGetValue(workerIndex, out var existing))
        {
            _workers[workerIndex] = existing with { Status = status };
        }

        return Task.CompletedTask;
    }

    public Task BroadcastCancellationAsync(string reason, CancellationToken cancellationToken)
    {
        _cancellationSignal = new CancellationSignal(reason, DateTimeOffset.UtcNow);
        return Task.CompletedTask;
    }

    public Task<CancellationSignal?> IsCancellationRequestedAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult(_cancellationSignal);
    }
}
