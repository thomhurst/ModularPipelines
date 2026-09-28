using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace ModularPipelines.Distributed.SignalR.Hub;

/// <summary>
/// SignalR hub through which workers register, claim leases, publish results and observe
/// cancellation. The master process hosts this hub; workers connect as clients.
/// </summary>
/// <remarks>
/// Connections are authenticated by the access token middleware before they reach the hub. Every
/// method except <see cref="RegisterWorker"/> requires the connection to hold the current
/// registration for its worker, and results and heartbeats must come from that worker.
/// </remarks>
internal sealed class DistributedPipelineHub(
    SignalRMasterState masterState,
    ILogger<DistributedPipelineHub> logger) : Microsoft.AspNetCore.SignalR.Hub
{
    public async Task RegisterWorker(WorkerRegistration registration)
    {
        ArgumentNullException.ThrowIfNull(registration);
        if (!string.Equals(registration.RunId, masterState.RunId, StringComparison.Ordinal))
        {
            throw new HubException(
                $"Worker {registration.WorkerId} belongs to run '{registration.RunId}', not to this master's run.");
        }

        try
        {
            await masterState.Coordinator.RegisterWorkerAsync(registration, Context.ConnectionAborted)
                .ConfigureAwait(false);
        }
        catch (InvalidOperationException exception)
        {
            throw new HubException(exception.Message);
        }

        masterState.Sessions[Context.ConnectionId] = registration;
        if (logger.IsEnabled(LogLevel.Information))
        {
            logger.LogInformation(
                "Worker {WorkerId} registered with capabilities: {Capabilities}",
                registration.WorkerId,
                string.Join(", ", registration.Capabilities));
        }
    }

    public Task Heartbeat(WorkerStatus status)
    {
        ArgumentNullException.ThrowIfNull(status);
        var registration = GetRegistration();
        if (status.WorkerId != registration.WorkerId
            || !string.Equals(status.RunId, masterState.RunId, StringComparison.Ordinal))
        {
            throw new HubException("Heartbeat rejected because it does not belong to this connection's worker and run.");
        }

        return masterState.Coordinator.SendHeartbeatAsync(status, Context.ConnectionAborted);
    }

    public Task<ModuleLease?> DequeueModule()
    {
        var registration = GetRegistration();
        return masterState.Coordinator.DequeueModuleAsync(
            registration.WorkerId,
            registration.Capabilities.ToHashSet(),
            Context.ConnectionAborted);
    }

    public Task PublishResult(SerializedModuleResult result, ModuleLease? lease)
    {
        ArgumentNullException.ThrowIfNull(result);
        var registration = GetRegistration();
        if (result.WorkerId != registration.WorkerId
            || (lease is not null && (lease.WorkerId != registration.WorkerId || lease.Assignment.ModuleId != result.ModuleId)))
        {
            throw new HubException("Result rejected because it was not produced under this connection's worker lease.");
        }

        if (logger.IsEnabled(LogLevel.Debug))
        {
            logger.LogDebug("Received result for {Module} from worker {WorkerId}", result.ModuleId, result.WorkerId);
        }

        return masterState.Coordinator.PublishResultAsync(result, lease, Context.ConnectionAborted);
    }

    public Task<SerializedModuleResult> WaitForResult(ModuleId moduleId)
    {
        GetRegistration();
        return masterState.Coordinator.WaitForResultAsync(moduleId, Context.ConnectionAborted);
    }

    public Task<DistributedCancellationReason> WaitForCancellation()
    {
        GetRegistration();
        return masterState.Coordinator.WaitForCancellationAsync(Context.ConnectionAborted);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        // Leases outlive the connection: a worker that reconnects renews them with its heartbeats,
        // and the master requeues them once they expire.
        if (masterState.Sessions.TryRemove(Context.ConnectionId, out var registration))
        {
            logger.LogWarning("Worker {WorkerId} disconnected", registration.WorkerId);
        }

        await base.OnDisconnectedAsync(exception).ConfigureAwait(false);
    }

    private WorkerRegistration GetRegistration()
    {
        if (!masterState.Sessions.TryGetValue(Context.ConnectionId, out var registration)
            || !masterState.Coordinator.IsRegistered(registration.WorkerId, registration.RegisteredAt))
        {
            throw new HubException("This connection is not a registered worker of this run.");
        }

        return registration;
    }
}
