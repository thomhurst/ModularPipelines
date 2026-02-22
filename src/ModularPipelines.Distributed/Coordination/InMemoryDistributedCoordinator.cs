using System.Collections.Concurrent;
using System.Threading.Channels;
using ModularPipelines.Distributed;

namespace ModularPipelines.Distributed.Coordination;

internal class InMemoryDistributedCoordinator : IDistributedCoordinator
{
    private readonly Channel<ModuleAssignment> _workQueue = Channel.CreateUnbounded<ModuleAssignment>();
    private readonly ConcurrentDictionary<string, TaskCompletionSource<SerializedModuleResult>> _results = new();
    private readonly ConcurrentDictionary<int, WorkerRegistration> _workers = new();
    private readonly object _dequeueLock = new();
    private volatile CancellationSignal? _cancellationSignal;

    public Task EnqueueModuleAsync(ModuleAssignment assignment, CancellationToken cancellationToken)
    {
        _workQueue.Writer.TryWrite(assignment);

        // Pre-create the result TCS so WaitForResultAsync can be called before the result is published
        _results.GetOrAdd(assignment.ModuleTypeName, _ => new TaskCompletionSource<SerializedModuleResult>());
        return Task.CompletedTask;
    }

    public async Task<ModuleAssignment?> DequeueModuleAsync(IReadOnlySet<string> workerCapabilities, CancellationToken cancellationToken)
    {
        // Simple approach: try to read from channel and check capabilities
        // If capabilities don't match, re-enqueue and return null
        try
        {
            while (await _workQueue.Reader.WaitToReadAsync(cancellationToken))
            {
                if (_workQueue.Reader.TryRead(out var assignment))
                {
                    if (assignment.RequiredCapabilities.IsSubsetOf(workerCapabilities))
                    {
                        return assignment;
                    }

                    // Re-enqueue if this worker can't handle it
                    _workQueue.Writer.TryWrite(assignment);

                    // Small delay to avoid tight loop
                    await Task.Delay(50, cancellationToken);
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
