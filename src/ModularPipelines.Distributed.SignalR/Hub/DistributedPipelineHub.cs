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
    public async Task RegisterWorker(WorkerRegistration registration)
    {
        var state = _masterState;
        var connectionId = Context.ConnectionId;

        var workerState = new WorkerState
        {
            ConnectionId = connectionId,
            Registration = registration,
        };

        // If this worker (identified by its stable index, not the connection id) had an
        // in-flight module pending re-enqueue after a disconnect, it has reconnected
        // within the grace window. Cancel the pending re-enqueue and restore its busy
        // state so the master keeps awaiting the original result instead of running the
        // module a second time.
        var pending = state.GetPendingReconnect(registration.WorkerIndex);
        if (pending is not null)
        {
            var resumed = pending.TryResume();
            pending.CancelDelay();

            if (state.ResultWaiters.TryGetValue(pending.Assignment.ModuleTypeName, out var waiter)
                && !waiter.Task.IsCompleted)
            {
                workerState.TryAssign(pending.Assignment);
                _logger.LogInformation(
                    resumed
                        ? "Worker {Index} reclaimed in-flight {Module}"
                        : "Worker {Index} reconnected after {Module} was claimed for redispatch; tracking its original execution",
                    registration.WorkerIndex,
                    pending.Assignment.ModuleTypeName);
            }
            else
            {
                state.CompletePendingReconnect(pending.Assignment.ModuleTypeName);
            }
        }

        state.Workers[connectionId] = workerState;
        state.Registrations[registration.WorkerIndex] = registration;

        _logger.LogInformation("Worker {Index} registered via connection {ConnectionId} with capabilities: {Capabilities}",
            registration.WorkerIndex, connectionId, string.Join(", ", registration.Capabilities));
    }

    /// <summary>
    /// Called by workers to publish a completed module result.
    /// </summary>
    public async Task PublishResult(SerializedModuleResult result)
    {
        var state = _masterState;

        _logger.LogDebug("Received result for {Module} from worker {Worker}",
            result.ModuleTypeName, result.WorkerIndex);

        // 1. Complete the result TCS (for master's WaitForResultAsync)
        if (state.ResultWaiters.TryGetValue(result.ModuleTypeName, out var tcs))
        {
            tcs.TrySetResult(result);
        }

        // 2. Broadcast ReceiveDependencyResult to all workers for CompletionSource pre-population
        await Clients.Others.SendAsync(HubMethodNames.ReceiveDependencyResult, result);

        state.CompletePendingReconnect(result.ModuleTypeName);

        // 3. Mark every worker tracked against this assignment as idle. Normally this
        // is only the sender; recovery can temporarily track both the original worker
        // and a retry worker until the first result wins.
        foreach (var workerState in state.Workers.Values)
        {
            if (workerState.TryCompleteAssignment(result.ModuleTypeName))
            {
                await TryAssignPendingWork(workerState, state);
            }
        }
    }

    /// <summary>
    /// Called by workers to request work when idle.
    /// </summary>
    public async Task RequestWork(HashSet<string> capabilities)
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

            // Don't re-enqueue in-flight work immediately: the worker may just be blipping
            // and auto-reconnecting, and re-running a module (with side effects) is unsafe.
            // Instead schedule a re-enqueue after a grace period; if the worker reconnects
            // within that window, RegisterWorker cancels it. If the result already came back
            // the assignment is null (cleared in PublishResult) or its waiter is complete.
            var inflight = workerState.ClearAssignment();
            if (inflight is not null
                && _masterState.ResultWaiters.TryGetValue(inflight.ModuleTypeName, out var waiter)
                && !waiter.Task.IsCompleted)
            {
                var pending = _masterState.TrackPendingReconnect(workerIndex, inflight);
                _ = ReEnqueueAfterGraceAsync(_masterState, _logger, pending);
            }
        }

        await base.OnDisconnectedAsync(exception);
    }

    /// <summary>
    /// After the reconnect grace period, re-enqueues a disconnected worker's in-flight
    /// module unless it reconnected (RegisterWorker removed the pending entry) or its
    /// result already arrived. Uses only shared state + logger so it is safe to run
    /// detached from the (transient) hub instance that scheduled it.
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

                if (!state.TryClaimRedispatch(assignment))
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
                    state.ReturnRedispatchToQueue(assignment);
                    state.PendingAssignments.Enqueue(assignment);

                    // Wake the master's dequeue loop so the re-queued work is picked up
                    // promptly instead of stalling until an unrelated event.
                    state.WorkAvailable.Release();
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
