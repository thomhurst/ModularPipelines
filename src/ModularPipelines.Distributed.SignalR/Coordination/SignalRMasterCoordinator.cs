using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using ModularPipelines.Distributed.Capabilities;
using ModularPipelines.Distributed.SignalR.Hub;

namespace ModularPipelines.Distributed.SignalR.Coordination;

/// <summary>
/// Master-side <see cref="IDistributedCoordinator"/> backed by SignalR.
/// Push model: tries to assign work to idle workers immediately, queues otherwise.
/// </summary>
internal class SignalRMasterCoordinator : IDistributedCoordinator
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
            _logger.LogDebug("Queued {Module} — no idle worker with matching capabilities", assignment.ModuleTypeName);
        }
    }

    public async Task<ModuleAssignment?> DequeueModuleAsync(IReadOnlySet<string> workerCapabilities, CancellationToken cancellationToken)
    {
        // The master's worker loop dequeues from the pending queue (same as external workers).
        // Poll with a short delay to avoid busy-waiting.
        while (!cancellationToken.IsCancellationRequested)
        {
            if (_state.IsCompleted && _state.PendingAssignments.IsEmpty)
            {
                return null;
            }

            var pendingCount = _state.PendingAssignments.Count;
            for (var i = 0; i < pendingCount; i++)
            {
                if (!_state.PendingAssignments.TryDequeue(out var assignment))
                {
                    break;
                }

                if (!CapabilityMatcher.CanExecute(assignment, workerCapabilities))
                {
                    // Re-enqueue — master can't handle this module
                    _state.PendingAssignments.Enqueue(assignment);
                    continue;
                }

                return assignment;
            }

            try
            {
                await Task.Delay(50, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                return null;
            }
        }

        return null;
    }

    public Task PublishResultAsync(SerializedModuleResult result, CancellationToken cancellationToken)
    {
        // Master receives results through the hub's PublishResult method.
        // This is called when the master itself produces a result (e.g., modules executed locally by the master's worker loop).
        if (_state.ResultWaiters.TryGetValue(result.ModuleTypeName, out var tcs))
        {
            tcs.TrySetResult(result);
        }

        return Task.CompletedTask;
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
        return Task.CompletedTask;
    }

    public Task<IReadOnlyList<WorkerRegistration>> GetRegisteredWorkersAsync(CancellationToken cancellationToken)
    {
        IReadOnlyList<WorkerRegistration> workers = _state.Registrations.Values.ToList().AsReadOnly();
        return Task.FromResult(workers);
    }

    public async Task SignalCompletionAsync(CancellationToken cancellationToken)
    {
        _state.IsCompleted = true;

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

    private async Task<bool> TryPushToIdleWorker(ModuleAssignment assignment)
    {
        foreach (var kvp in _state.Workers)
        {
            var worker = kvp.Value;

            // Check capability match
            if (!CapabilityMatcher.CanExecute(assignment, worker.Registration))
            {
                continue;
            }

            // Try to claim this worker
            if (worker.TryMarkBusy())
            {
                _logger.LogDebug("Pushing {Module} to worker {Index}",
                    assignment.ModuleTypeName, worker.Registration.WorkerIndex);

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
                    worker.MarkIdle();
                }
            }
        }

        return false;
    }
}
