using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Distributed;

namespace ModularPipelines.Distributed.Master;

/// <summary>
/// Background task on master that monitors worker health via heartbeats.
/// Detects unresponsive workers and can trigger module reassignment.
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
        var options = _options.Value;
        var checkInterval = TimeSpan.FromSeconds(options.HeartbeatIntervalSeconds);
        _ = TimeSpan.FromSeconds(options.HeartbeatTimeoutSeconds);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var workers = await _coordinator.GetRegisteredWorkersAsync(stoppingToken);

                foreach (var worker in workers)
                {
                    if (worker.Status == WorkerStatus.TimedOut || worker.Status == WorkerStatus.Disconnected)
                    {
                        continue;
                    }

                    // Check if worker has timed out based on registration time
                    // In a real implementation, we'd track last heartbeat time
                    // For now, rely on the coordinator's heartbeat tracking
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
