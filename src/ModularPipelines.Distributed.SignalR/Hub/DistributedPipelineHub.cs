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

        // 3. Mark the sending worker as idle and try to assign pending work
        if (state.Workers.TryGetValue(Context.ConnectionId, out var workerState))
        {
            workerState.ClearAssignment();
            workerState.MarkIdle();
            await TryAssignPendingWork(workerState, state);
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
            _logger.LogWarning("Worker {Index} disconnected (connection {ConnectionId})",
                workerState.Registration.WorkerIndex, Context.ConnectionId);

            // Re-enqueue any in-flight assignment so a surviving worker (or the master's own
            // worker loop) can pick it up, instead of the master waiting forever for a result
            // that will never arrive. If the result already came back the assignment is null
            // (cleared in PublishResult) or its waiter is already completed, so we skip it.
            var inflight = workerState.ClearAssignment();
            if (inflight is not null
                && _masterState.ResultWaiters.TryGetValue(inflight.ModuleTypeName, out var waiter)
                && !waiter.Task.IsCompleted)
            {
                _logger.LogWarning(
                    "Re-enqueuing in-flight module {Module} from disconnected worker {Index}",
                    inflight.ModuleTypeName, workerState.Registration.WorkerIndex);

                _masterState.PendingAssignments.Enqueue(inflight);

                // Wake the master's own dequeue loop...
                _masterState.WorkAvailable.Release();

                // ...and nudge a currently-idle worker to pick it up immediately.
                foreach (var kvp in _masterState.Workers)
                {
                    if (kvp.Value.IsIdle)
                    {
                        await TryAssignPendingWork(kvp.Value, _masterState);
                        break;
                    }
                }
            }
        }

        await base.OnDisconnectedAsync(exception);
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
            if (workerState.TryMarkBusy())
            {
                _logger.LogDebug("Assigning {Module} to worker {Index}",
                    assignment.ModuleTypeName, workerState.Registration.WorkerIndex);
                workerState.SetAssignment(assignment);
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
                    workerState.ClearAssignment();
                    workerState.MarkIdle();
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
