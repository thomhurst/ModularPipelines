using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace ModularPipelines.Distributed.SignalR.Hub;

/// <summary>
/// SignalR hub that handles worker registration, work assignment, and result collection.
/// The master process hosts this hub; workers connect as clients.
/// </summary>
internal class DistributedPipelineHub(
    ILogger<DistributedPipelineHub> logger) : Microsoft.AspNetCore.SignalR.Hub
{
    private readonly ILogger<DistributedPipelineHub> _logger = logger;

    /// <summary>
    /// Injected by DI — the master coordinator that manages state.
    /// </summary>
    internal SignalRMasterState? MasterState { get; set; }

    /// <summary>
    /// Called by workers to register their capabilities.
    /// </summary>
    public async Task RegisterWorker(WorkerRegistration registration)
    {
        var state = GetMasterState();
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
        var state = GetMasterState();

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
            workerState.MarkIdle();
            await TryAssignPendingWork(workerState, state);
        }
    }

    /// <summary>
    /// Called by workers to request work when idle.
    /// </summary>
    public async Task RequestWork(IReadOnlySet<string> capabilities)
    {
        var state = GetMasterState();

        if (!state.Workers.TryGetValue(Context.ConnectionId, out var workerState))
        {
            return;
        }

        await TryAssignPendingWork(workerState, state);
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        var state = MasterState;
        if (state is null)
        {
            return Task.CompletedTask;
        }

        if (state.Workers.TryRemove(Context.ConnectionId, out var workerState))
        {
            _logger.LogWarning("Worker {Index} disconnected (connection {ConnectionId})",
                workerState.Registration.WorkerIndex, Context.ConnectionId);

            // TODO: Re-enqueue in-flight work for the disconnected worker
        }

        return Task.CompletedTask;
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

            // Check capability match
            if (assignment.RequiredCapabilities.Count > 0 &&
                !assignment.RequiredCapabilities.IsSubsetOf(workerState.Registration.Capabilities))
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
                await Clients.Client(workerState.ConnectionId)
                    .SendAsync(HubMethodNames.ReceiveAssignment, assignment);
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

    private SignalRMasterState GetMasterState()
    {
        return MasterState ?? throw new InvalidOperationException(
            "Hub was not initialized with master state. Ensure the hub is used through SignalRMasterCoordinator.");
    }
}
