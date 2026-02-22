using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Distributed;

namespace ModularPipelines.Distributed.Master;

/// <summary>
/// Background task on master that monitors worker health via heartbeats.
/// Detects unresponsive workers and marks them as timed out.
/// </summary>
internal class WorkerHealthMonitor(
    IDistributedCoordinator coordinator,
    IOptions<DistributedOptions> options,
    ILogger<WorkerHealthMonitor> logger) : BackgroundService
{
    private readonly IDistributedCoordinator _coordinator = coordinator;
    private readonly IOptions<DistributedOptions> _options = options;
    private readonly ILogger<WorkerHealthMonitor> _logger = logger;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var opts = _options.Value;
        var checkInterval = TimeSpan.FromSeconds(opts.HeartbeatIntervalSeconds);
        var heartbeatTimeout = TimeSpan.FromSeconds(opts.HeartbeatTimeoutSeconds);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var workers = await _coordinator.GetRegisteredWorkersAsync(stoppingToken);

                foreach (var worker in workers)
                {
                    if (worker.Status is WorkerStatus.TimedOut or WorkerStatus.Disconnected)
                    {
                        continue;
                    }

                    var heartbeat = await _coordinator.GetLastHeartbeatAsync(worker.WorkerIndex, stoppingToken);

                    // Use heartbeat timestamp if available, otherwise fall back to registration time
                    var lastSeen = heartbeat?.Timestamp ?? worker.RegisteredAt;

                    if (DateTimeOffset.UtcNow - lastSeen > heartbeatTimeout)
                    {
                        _logger.LogWarning(
                            "Worker {Index} timed out (last seen {LastSeen}, timeout {Timeout}s)",
                            worker.WorkerIndex, lastSeen, opts.HeartbeatTimeoutSeconds);
                        await _coordinator.UpdateWorkerStatusAsync(worker.WorkerIndex, WorkerStatus.TimedOut, stoppingToken);
                    }
                }
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                _logger.LogWarning(ex, "Failed to check worker health");
            }

            try
            {
                await Task.Delay(checkInterval, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                break;
            }
        }
    }
}
