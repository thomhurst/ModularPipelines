using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using ModularPipelines.Distributed.Capabilities;

namespace ModularPipelines.Distributed.SignalR.Hub;

/// <summary>
/// SignalR hub that handles worker registration, work assignment, and result collection.
/// The master process hosts this hub; workers connect as clients.
/// </summary>
internal class DistributedPipelineHub(
    SignalRMasterState masterState,
    ILogger<DistributedPipelineHub> logger) : Microsoft.AspNetCore.SignalR.Hub
{
    private readonly SignalRMasterState _masterState = masterState;
    private readonly ILogger<DistributedPipelineHub> _logger = logger;

    /// <summary>
    /// Called by workers to register their capabilities.
    /// </summary>
    public async Task RegisterWorker(
        WorkerRegistration registration,
        string? resumingModuleTypeName)
    {
        var state = _masterState;
        var connectionId = Context.ConnectionId;

        var workerState = new WorkerState
        {
            ConnectionId = connectionId,
            Registration = registration,
        };

        // A reconnect may restore work only when the worker claims the same in-flight
        // module. The stable index alone is insufficient because a replacement process
        // can reuse it without owning the original execution.
        if (state.TryRestoreReconnect(
                workerState,
                resumingModuleTypeName,
                out var recoveredAssignment))
        {
            _logger.LogInformation(
                "Worker {Index} reclaimed in-flight {Module}",
                registration.WorkerIndex,
                recoveredAssignment!.ModuleTypeName);
        }

        state.Workers[connectionId] = workerState;
        state.Registrations[registration.WorkerIndex] = registration;
        state.Heartbeats[registration.WorkerIndex] = DateTimeOffset.UtcNow;

        // Cancellation is durable master state. A worker that was disconnected
        // during the original broadcast must receive it before registration returns.
        if (state.CancellationRequested.Task.IsCompletedSuccessfully)
        {
            await Clients.Caller.SendAsync(
                HubMethodNames.BroadcastCancellation,
                Context.ConnectionAborted);
        }

        _logger.LogInformation("Worker {Index} registered via connection {ConnectionId} with capabilities: {Capabilities}",
            registration.WorkerIndex, connectionId, string.Join(", ", registration.Capabilities));
    }

    /// <summary>
    /// Records liveness for a connected worker.
    /// </summary>
    public Task Heartbeat(int workerIndex)
    {
        if (_masterState.Workers.TryGetValue(Context.ConnectionId, out var worker)
            && worker.Registration.WorkerIndex == workerIndex)
        {
            _masterState.Heartbeats[workerIndex] = DateTimeOffset.UtcNow;
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// Called by workers to publish a completed module result.
    /// </summary>
    public async Task PublishResult(SerializedModuleResult result)
    {
        var state = _masterState;

        _logger.LogDebug("Received result for {Module} from worker {Worker}",
            result.ModuleTypeName, result.WorkerIndex);

        // 1. Complete the result and atomically capture workers involved in reconnect
        // recovery before a concurrent registration can start tracking the assignment.
        var workersToRelease = (await state.CompleteResultAsync(result)).ToHashSet();
        if (state.Workers.TryGetValue(Context.ConnectionId, out var sendingWorker))
        {
            workersToRelease.Add(sendingWorker);
        }

        // 2. Mark the sender and any reconnected original worker idle.
        foreach (var workerState in workersToRelease)
        {
            if (workerState.TryCompleteAssignment(result.ModuleTypeName))
            {
                await TryAssignPendingWork(workerState, state);
            }
        }
    }

    /// <summary>
    /// Returns a stored module result after it becomes available.
    /// </summary>
    public async Task<SerializedModuleResult> WaitForResult(string moduleTypeName)
    {
        var waiter = _masterState.ResultWaiters.GetOrAdd(
            moduleTypeName,
            static _ => new TaskCompletionSource<SerializedModuleResult>(
                TaskCreationOptions.RunContinuationsAsynchronously));

        return await waiter.Task.WaitAsync(Context.ConnectionAborted).ConfigureAwait(false);
    }

    /// <summary>
    /// Called by workers to request work when idle.
    /// </summary>
    public async Task RequestWork(HashSet<Capability> capabilities)
    {
        var state = _masterState;

        if (!state.Workers.TryGetValue(Context.ConnectionId, out var workerState))
        {
            return;
        }

        await TryAssignPendingWork(workerState, state);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        if (_masterState.Workers.TryRemove(Context.ConnectionId, out var workerState))
        {
            var workerIndex = workerState.Registration.WorkerIndex;
            _logger.LogWarning("Worker {Index} disconnected (connection {ConnectionId})",
                workerIndex, Context.ConnectionId);

            // Retain the final registration and heartbeat for run-report collection.
            // GetRegisteredWorkersAsync uses heartbeat age to exclude stale workers
            // from liveness checks without erasing their last published metrics here.

            // Don't re-enqueue in-flight work immediately: the worker may just be blipping
            // and auto-reconnecting, and re-running a module (with side effects) is unsafe.
            // Instead schedule a re-enqueue after a grace period; if the worker reconnects
            // within that window and claims the assignment, RegisterWorker cancels it.
            // If the result already came back, the assignment is null (cleared in
            // PublishResult) or its waiter is complete.
            var inflight = workerState.ClearAssignment();
            if (inflight is not null
                && _masterState.ResultWaiters.TryGetValue(inflight.ModuleTypeName, out var waiter)
                && !waiter.Task.IsCompleted)
            {
                var pending = _masterState.TrackPendingReconnect(workerState, inflight);
                if (pending is not null)
                {
                    _ = ReEnqueueAfterGraceAsync(_masterState, _logger, pending);
                }
            }
        }

        await base.OnDisconnectedAsync(exception);
    }

    /// <summary>
    /// After the reconnect grace period, re-enqueues a disconnected worker's in-flight
    /// module unless it reconnected and reclaimed the assignment or its result already
    /// arrived. Uses only shared state + logger so it is safe to run detached from the
    /// (transient) hub instance that scheduled it.
    /// </summary>
    private static async Task ReEnqueueAfterGraceAsync(
        SignalRMasterState state,
        ILogger logger,
        PendingReconnect pending)
    {
        try
        {
            await Task.Delay(state.ReconnectGracePeriod, pending.DelayToken);
        }
        catch (OperationCanceledException)
        {
            return; // Worker reconnected within the grace window — keep awaiting its result.
        }

        // Make the retry available without claiming it. A simultaneous reconnect may
        // still atomically reclaim it until an executor claims the queued assignment.
        if (!pending.TryMakeAvailableForRedispatch())
        {
            return;
        }

        // Skip if the result arrived in the meantime.
        if (state.ResultWaiters.TryGetValue(pending.Assignment.ModuleTypeName, out var waiter)
            && waiter.Task.IsCompleted)
        {
            state.CompletePendingReconnect(pending.Assignment.ModuleTypeName);
            return;
        }

        logger.LogWarning(
            "Reconnect grace elapsed for worker {Index}; re-enqueuing in-flight module {Module}",
            pending.WorkerIndex,
            pending.Assignment.ModuleTypeName);

        state.PendingAssignments.Enqueue(pending.Assignment);
        state.WorkAvailable.Release();
    }

    private async Task TryAssignPendingWork(WorkerState workerState, SignalRMasterState state)
    {
        // Try to dequeue and assign work that matches this worker's capabilities
        var pendingCount = state.PendingAssignments.Count;
        for (var i = 0; i < pendingCount; i++)
        {
            if (!state.PendingAssignments.TryDequeue(out var assignment))
            {
                break;
            }

            // Skip if this module's result already arrived (e.g. the original worker's
            // result raced a disconnect re-enqueue). Prevents dispatching - and re-running
            // the side effects of - work that is already complete.
            if (state.ResultWaiters.TryGetValue(assignment.ModuleTypeName, out var existingWaiter)
                && existingWaiter.Task.IsCompleted)
            {
                continue;
            }

            // Check capability match
            if (!CapabilityMatcher.CanExecute(assignment, workerState.Registration))
            {
                // Re-enqueue — this worker can't handle it
                state.PendingAssignments.Enqueue(assignment);
                continue;
            }

            // Assign to this worker
            if (workerState.TryAssign(assignment))
            {
                _logger.LogDebug("Assigning {Module} to worker {Index}",
                    assignment.ModuleTypeName, workerState.Registration.WorkerIndex);

                using var deliveryFence =
                    await state.EnterAssignmentDeliveryFenceAsync(assignment.ModuleTypeName);
                if (!state.TryClaimRedispatch(assignment, workerState))
                {
                    workerState.TryCompleteAssignment(assignment.ModuleTypeName);
                    continue;
                }

                try
                {
                    await Clients.Client(workerState.ConnectionId)
                        .SendAsync(HubMethodNames.ReceiveAssignment, assignment);
                }
                catch (Exception ex)
                {
                    // Send failed — undo the claim and re-queue so the module isn't lost.
                    _logger.LogWarning(ex, "Failed to assign {Module} to worker {Index}; re-queuing",
                        assignment.ModuleTypeName, workerState.Registration.WorkerIndex);
                    workerState.TryCompleteAssignment(assignment.ModuleTypeName);
                    if (state.TryReturnRedispatchToQueue(assignment, workerState))
                    {
                        state.PendingAssignments.Enqueue(assignment);

                        // Wake the master's dequeue loop so the re-queued work is picked up
                        // promptly instead of stalling until an unrelated event.
                        state.WorkAvailable.Release();
                    }
                }

                return;
            }
            else
            {
                // Worker became busy between check and assign — re-enqueue
                state.PendingAssignments.Enqueue(assignment);
                return;
            }
        }
    }
}
