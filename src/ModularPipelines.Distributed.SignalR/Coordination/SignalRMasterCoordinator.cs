using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using ModularPipelines.Distributed.Capabilities;
using ModularPipelines.Distributed.SignalR.Hub;

namespace ModularPipelines.Distributed.SignalR.Coordination;

/// <summary>
/// Master-side <see cref="IDistributedMasterCoordinator"/> backed by SignalR.
/// Push model: tries to assign work to idle workers immediately, queues otherwise.
/// </summary>
internal class SignalRMasterCoordinator : IDistributedMasterCoordinator
{
    private readonly IHubContext<DistributedPipelineHub> _hubContext;
    private readonly SignalRMasterState _state;
    private readonly ILogger<SignalRMasterCoordinator> _logger;

    public SignalRMasterCoordinator(
        IHubContext<DistributedPipelineHub> hubContext,
        SignalRMasterState state,
        ILogger<SignalRMasterCoordinator> logger)
    {
        _hubContext = hubContext;
        _state = state;
        _logger = logger;
    }

    /// <summary>
    /// Exposes internal state for the hub to access.
    /// </summary>
    internal SignalRMasterState State => _state;

    public async Task EnqueueModuleAsync(ModuleAssignment assignment, CancellationToken cancellationToken)
    {
        // Pre-create the result waiter
        _state.ResultWaiters.GetOrAdd(assignment.ModuleTypeName,
            _ => new TaskCompletionSource<SerializedModuleResult>(TaskCreationOptions.RunContinuationsAsynchronously));

        // Try to push directly to an idle worker with matching capabilities
        var assigned = await TryPushToIdleWorker(assignment);
        if (!assigned)
        {
            // No idle worker available — queue for later
            _state.PendingAssignments.Enqueue(assignment);
            _state.WorkAvailable.Release();
            _logger.LogDebug("Queued {Module} — no idle worker with matching capabilities", assignment.ModuleTypeName);
        }
    }

    public async Task<ModuleAssignment?> DequeueModuleAsync(IReadOnlySet<Capability> workerCapabilities, CancellationToken cancellationToken)
    {
        // The master's worker loop dequeues from the pending queue.
        // Uses a semaphore signal instead of polling to avoid busy-waiting.
        while (!cancellationToken.IsCancellationRequested)
        {
            if (_state.IsCompleted && _state.PendingAssignments.IsEmpty)
            {
                return null;
            }

            // Try scanning existing items first (before waiting)
            var found = TryScanPendingQueue(workerCapabilities);
            if (found is not null)
            {
                return found;
            }

            try
            {
                await _state.WorkAvailable.WaitAsync(cancellationToken);
            }
            catch (OperationCanceledException)
            {
                return null;
            }

            if (_state.IsCompleted && _state.PendingAssignments.IsEmpty)
            {
                return null;
            }

            found = TryScanPendingQueue(workerCapabilities);
            if (found is not null)
            {
                return found;
            }
        }

        return null;
    }

    private ModuleAssignment? TryScanPendingQueue(IReadOnlySet<Capability> workerCapabilities)
    {
        var pendingCount = _state.PendingAssignments.Count;
        for (var i = 0; i < pendingCount; i++)
        {
            if (!_state.PendingAssignments.TryDequeue(out var assignment))
            {
                break;
            }

            // Skip work whose result already arrived (e.g. a disconnect re-enqueue that
            // raced the original worker's result) so it isn't executed a second time.
            if (_state.ResultWaiters.TryGetValue(assignment.ModuleTypeName, out var existingWaiter)
                && existingWaiter.Task.IsCompleted)
            {
                continue;
            }

            if (!CapabilityMatcher.CanExecute(assignment, workerCapabilities))
            {
                // Re-enqueue — master can't handle this module
                _state.PendingAssignments.Enqueue(assignment);
                continue;
            }

            if (!_state.TryClaimRedispatch(assignment))
            {
                continue;
            }

            return assignment;
        }

        return null;
    }

    public async Task PublishResultAsync(SerializedModuleResult result, CancellationToken cancellationToken)
    {
        // Master receives results through the hub's PublishResult method.
        // This is called when the master itself produces a result (e.g., modules executed locally by the master's worker loop).
        foreach (var worker in await _state.CompleteResultAsync(result))
        {
            worker.TryCompleteAssignment(result.ModuleTypeName);
        }
    }

    public async Task<SerializedModuleResult> WaitForResultAsync(string moduleTypeName, CancellationToken cancellationToken)
    {
        var tcs = _state.ResultWaiters.GetOrAdd(moduleTypeName,
            _ => new TaskCompletionSource<SerializedModuleResult>(TaskCreationOptions.RunContinuationsAsynchronously));

        await using var reg = cancellationToken.Register(() => tcs.TrySetCanceled(cancellationToken));
        return await tcs.Task;
    }

    public Task RegisterWorkerAsync(WorkerRegistration registration, CancellationToken cancellationToken)
    {
        // Workers register through the hub. This is for the interface contract.
        _state.Registrations[registration.WorkerIndex] = registration;
        _state.Heartbeats[registration.WorkerIndex] = DateTimeOffset.UtcNow;
        return Task.CompletedTask;
    }

    public Task SendHeartbeatAsync(int workerIndex, CancellationToken cancellationToken)
    {
        if (_state.Registrations.ContainsKey(workerIndex))
        {
            _state.Heartbeats[workerIndex] = DateTimeOffset.UtcNow;
        }

        return Task.CompletedTask;
    }

    public Task<IReadOnlyList<WorkerRegistration>> GetRegisteredWorkersAsync(CancellationToken cancellationToken)
    {
        var oldestLiveHeartbeat = DateTimeOffset.UtcNow - _state.WorkerTimeout;
        IReadOnlyList<WorkerRegistration> workers =
        [
            .. _state.Registrations.Values.Where(worker =>
                worker.UnattributedCommandCount.HasValue
                || (_state.Heartbeats.TryGetValue(worker.WorkerIndex, out var heartbeat)
                    && heartbeat >= oldestLiveHeartbeat)),
        ];
        return Task.FromResult(workers);
    }

    public async Task SignalCompletionAsync(CancellationToken cancellationToken)
    {
        _state.IsCompleted = true;

        // Wake any waiting dequeue loop
        _state.WorkAvailable.Release();

        // Cancel any pending result waiters
        foreach (var kvp in _state.ResultWaiters)
        {
            kvp.Value.TrySetCanceled();
        }

        // Broadcast completion to all workers
        try
        {
            await _hubContext.Clients.All.SendAsync(HubMethodNames.SignalCompletion, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to broadcast completion signal to workers");
        }
    }

    public async Task BroadcastCancellationAsync(CancellationToken cancellationToken)
    {
        _state.CancellationRequested.TrySetResult();
        await _hubContext.Clients.All.SendAsync(
            HubMethodNames.BroadcastCancellation,
            cancellationToken);
    }

    public Task WaitForCancellationAsync(CancellationToken cancellationToken) =>
        _state.CancellationRequested.Task.WaitAsync(cancellationToken);

    private async Task<bool> TryPushToIdleWorker(ModuleAssignment assignment)
    {
        // Don't dispatch work whose result already arrived.
        if (_state.ResultWaiters.TryGetValue(assignment.ModuleTypeName, out var existingWaiter)
            && existingWaiter.Task.IsCompleted)
        {
            return true;
        }

        foreach (var kvp in _state.Workers)
        {
            var worker = kvp.Value;

            // Check capability match
            if (!CapabilityMatcher.CanExecute(assignment, worker.Registration))
            {
                continue;
            }

            // Try to claim this worker
            if (worker.TryAssign(assignment))
            {
                _logger.LogDebug("Pushing {Module} to worker {Index}",
                    assignment.ModuleTypeName, worker.Registration.WorkerIndex);

                using var deliveryFence =
                    await _state.EnterAssignmentDeliveryFenceAsync(assignment.ModuleTypeName);
                if (!_state.TryClaimRedispatch(assignment, worker))
                {
                    worker.TryCompleteAssignment(assignment.ModuleTypeName);
                    continue;
                }

                try
                {
                    await _hubContext.Clients.Client(worker.ConnectionId)
                        .SendAsync(HubMethodNames.ReceiveAssignment, assignment);
                    return true;
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to push assignment to worker {Index}, marking idle",
                        worker.Registration.WorkerIndex);
                    worker.TryCompleteAssignment(assignment.ModuleTypeName);
                    if (!_state.TryReturnRedispatchToQueue(assignment, worker))
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }
}
