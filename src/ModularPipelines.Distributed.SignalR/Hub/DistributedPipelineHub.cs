using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using ModularPipelines.Distributed;

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

        PendingReconnect? supersededReconnect = null;
        ModuleAssignment? recoveredAssignment;
        bool restored;
        lock (state.GetWorkerStateLock(registration.WorkerIndex))
        {
            var supersededWorker = state.RegisterWorker(workerState);
            var supersededAssignment = supersededWorker?.ClearAssignment();
            if (supersededAssignment is not null
                && state.ResultWaiters.TryGetValue(supersededAssignment.ModuleTypeName, out var waiter)
                && !waiter.Task.IsCompleted)
            {
                supersededReconnect = state.TrackPendingReconnect(
                    supersededWorker!,
                    supersededAssignment);
            }

            // The index alone cannot establish ownership of a previous process's execution.
            restored = state.TryRestoreReconnect(workerState, resumingModuleTypeName, out recoveredAssignment);
        }

        if (restored && _logger.IsEnabled(LogLevel.Information))
        {
            _logger.LogInformation(
                "Worker {Index} reclaimed in-flight {Module}",
                registration.WorkerIndex,
                recoveredAssignment!.ModuleTypeName);
        }

        if (supersededReconnect is not null)
        {
            _ = ReEnqueueAfterGraceAsync(state, _logger, supersededReconnect);
        }

        // Cancellation is durable master state. A worker that was disconnected
        // during the original broadcast must receive it before registration returns.
        if (state.CancellationRequested.Task.IsCompletedSuccessfully)
        {
            await Clients.Caller.SendAsync(
                HubMethodNames.BroadcastCancellation,
                Context.ConnectionAborted).ConfigureAwait(false);
        }

        if (_logger.IsEnabled(LogLevel.Information))
        {
            _logger.LogInformation("Worker {Index} registered via connection {ConnectionId} with capabilities: {Capabilities}",
                registration.WorkerIndex, connectionId, string.Join(", ", registration.Capabilities));
        }
    }

    /// <summary>
    /// Records liveness for a connected worker.
    /// </summary>
    public Task Heartbeat(WorkerStatus status)
    {
        var connectionId = Context.ConnectionId;
        if (_masterState.Workers.TryGetValue(connectionId, out var worker))
        {
            _masterState.TryRecordHeartbeat(worker, status);
            return Task.CompletedTask;
        }

        // A reconnect heartbeat can race with RegisterWorker on a separate SignalR invocation.
        // Keep it connection-scoped until registration proves ownership of the worker index.
        _masterState.PendingWorkerStatuses[connectionId] = status;
        if (_masterState.Workers.TryGetValue(connectionId, out worker)
            && _masterState.PendingWorkerStatuses.TryRemove(connectionId, out var pendingStatus))
        {
            _masterState.TryRecordHeartbeat(worker, pendingStatus);
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// Called by workers to publish a completed module result.
    /// </summary>
    public async Task PublishResult(SerializedModuleResult result)
    {
        var state = _masterState;

        if (!state.Workers.TryGetValue(Context.ConnectionId, out var sendingWorker))
        {
            throw new HubException("Result rejected because this connection is not a registered worker.");
        }

        if (_logger.IsEnabled(LogLevel.Debug))
        {
            _logger.LogDebug("Received result for {Module} from worker {Worker}",
                result.ModuleTypeName, result.WorkerIndex);
        }

        // 1. Complete the result and atomically capture workers involved in reconnect
        // recovery before a concurrent registration can start tracking the assignment.
        var (accepted, reconnectedWorkers) = await state.TryCompleteWorkerResultAsync(sendingWorker, result)
            .ConfigureAwait(false);
        if (!accepted)
        {
            throw new HubException("Result rejected because this connection does not own the assignment.");
        }

        var workersToRelease = reconnectedWorkers.ToHashSet();
        workersToRelease.Add(sendingWorker);

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
        // Keep the positional hub argument; dispatch uses the registered capabilities.
        _ = capabilities;
        var state = _masterState;

        if (!state.Workers.TryGetValue(Context.ConnectionId, out var workerState))
        {
            return;
        }

        await TryAssignPendingWork(workerState, state);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var connectionId = Context.ConnectionId;
        _masterState.PendingWorkerStatuses.TryRemove(connectionId, out _);
        if (_masterState.Workers.TryGetValue(connectionId, out var workerState))
        {
            var workerIndex = workerState.Registration.WorkerIndex;
            PendingReconnect? pending = null;
            bool removed;
            lock (_masterState.GetWorkerStateLock(workerIndex))
            {
                removed = _masterState.Workers.TryRemove(new KeyValuePair<string, WorkerState>(connectionId, workerState));
                if (removed)
                {
                    // Publish reconnect ownership before another connection can register.
                    // Preserve registration and final metrics for run-report collection.
                    var inflight = workerState.ClearAssignment();
                    if (inflight is not null
                        && _masterState.ResultWaiters.TryGetValue(inflight.ModuleTypeName, out var waiter)
                        && !waiter.Task.IsCompleted)
                    {
                        pending = _masterState.TrackPendingReconnect(workerState, inflight);
                    }
                }
            }

            if (removed)
            {
                _logger.LogWarning("Worker {Index} disconnected (connection {ConnectionId})", workerIndex, connectionId);
            }

            if (pending is not null)
            {
                _ = ReEnqueueAfterGraceAsync(_masterState, _logger, pending);
            }
        }

        await base.OnDisconnectedAsync(exception).ConfigureAwait(false);
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
        catch (ObjectDisposedException)
        {
            return; // Result completion disposed the pending delay before this task started.
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
                if (_logger.IsEnabled(LogLevel.Debug))
                {
                    _logger.LogDebug("Assigning {Module} to worker {Index}",
                        assignment.ModuleTypeName, workerState.Registration.WorkerIndex);
                }

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
