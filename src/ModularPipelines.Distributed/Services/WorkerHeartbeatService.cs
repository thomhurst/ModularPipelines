using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Distributed.Abstractions;
using ModularPipelines.Distributed.Communication.Messages;
using ModularPipelines.Distributed.Options;

namespace ModularPipelines.Distributed.Services;

/// <summary>
/// Background service that sends periodic heartbeats from worker to orchestrator.
/// </summary>
internal sealed class WorkerHeartbeatService : BackgroundService
{
    private readonly INodeRegistry _nodeRegistry;
    private readonly ILogger<WorkerHeartbeatService> _logger;
    private readonly DistributedPipelineOptions _options;
    private readonly string _workerId;
    private readonly WorkerNode _workerNode;

    public WorkerHeartbeatService(
        INodeRegistry nodeRegistry,
        ILogger<WorkerHeartbeatService> logger,
        IOptions<DistributedPipelineOptions> options)
    {
        _nodeRegistry = nodeRegistry ?? throw new ArgumentNullException(nameof(nodeRegistry));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));

        if (_options.Mode != DistributedExecutionMode.Worker)
        {
            throw new InvalidOperationException(
                "WorkerHeartbeatService can only run in Worker mode");
        }

        if (string.IsNullOrWhiteSpace(_options.OrchestratorUrl))
        {
            throw new InvalidOperationException(
                "OrchestratorUrl must be configured in Worker mode");
        }

        if (_options.WorkerCapabilities == null)
        {
            throw new InvalidOperationException(
                "WorkerCapabilities must be configured in Worker mode");
        }

        _workerId = _options.WorkerId ?? $"worker-{Environment.MachineName}-{Guid.NewGuid():N}";

        // Create worker node for registration
        _workerNode = new WorkerNode
        {
            Id = _workerId,
            Endpoint = GetWorkerEndpoint(),
            Capabilities = _options.WorkerCapabilities,
            LastHeartbeat = DateTimeOffset.UtcNow,
            Status = WorkerStatus.Available,
        };
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation(
            "Worker heartbeat service starting. Worker ID: {WorkerId}, Orchestrator: {OrchestratorUrl}",
            _workerId,
            _options.OrchestratorUrl);

        try
        {
            // Initial registration
            await RegisterWorkerAsync(stoppingToken);

            // Send periodic heartbeats
            await SendPeriodicHeartbeatsAsync(stoppingToken);
        }
        catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker heartbeat service stopping gracefully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Worker heartbeat service encountered an error");
            throw;
        }
        finally
        {
            // Unregister on shutdown
            await UnregisterWorkerAsync();
        }
    }

    private async Task RegisterWorkerAsync(CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation(
                "Registering worker with orchestrator. Capabilities: OS={Os}, Tools={Tools}, MaxParallel={MaxParallel}",
                _workerNode.Capabilities.Os,
                string.Join(", ", _workerNode.Capabilities.InstalledTools),
                _workerNode.Capabilities.MaxParallelModules);

            await _nodeRegistry.RegisterWorkerAsync(_workerNode, cancellationToken);

            _logger.LogInformation("Worker successfully registered with orchestrator");
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to register worker with orchestrator at {OrchestratorUrl}",
                _options.OrchestratorUrl);
            throw;
        }
    }

    private async Task SendPeriodicHeartbeatsAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(_options.WorkerHeartbeatInterval);

        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            try
            {
                await SendHeartbeatAsync(stoppingToken);
            }
            catch (Exception ex) when (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogWarning(
                    ex,
                    "Failed to send heartbeat to orchestrator. Will retry in {Interval}",
                    _options.WorkerHeartbeatInterval);
            }
        }
    }

    private async Task SendHeartbeatAsync(CancellationToken cancellationToken)
    {
        _logger.LogDebug("Sending heartbeat to orchestrator");

        await _nodeRegistry.UpdateHeartbeatAsync(_workerId, cancellationToken);

        _logger.LogDebug("Heartbeat sent successfully");
    }

    private async Task UnregisterWorkerAsync()
    {
        try
        {
            _logger.LogInformation("Unregistering worker from orchestrator");

            await _nodeRegistry.UnregisterWorkerAsync(_workerId, CancellationToken.None);

            _logger.LogInformation("Worker successfully unregistered");
        }
        catch (Exception ex)
        {
            _logger.LogWarning(
                ex,
                "Failed to unregister worker from orchestrator (this is non-fatal)");
        }
    }

    private string GetWorkerEndpoint()
    {
        // Construct the worker endpoint from the configured port
        var port = _options.WorkerPort;
        var hostname = Environment.MachineName.ToLowerInvariant();
        return $"http://{hostname}:{port}";
    }
}
