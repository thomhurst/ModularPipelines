using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ModularPipelines.Distributed.Worker;

internal class WorkerHeartbeatService(
    IDistributedCoordinator coordinator,
    IOptions<DistributedOptions> options,
    ILogger<WorkerHeartbeatService> logger) : BackgroundService
{
    private readonly IDistributedCoordinator _coordinator = coordinator;
    private readonly IOptions<DistributedOptions> _options = options;
    private readonly ILogger<WorkerHeartbeatService> _logger = logger;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var options = _options.Value;
        var interval = TimeSpan.FromSeconds(options.HeartbeatIntervalSeconds);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await _coordinator.SendHeartbeatAsync(options.InstanceIndex, stoppingToken);
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                _logger.LogWarning(ex, "Failed to send heartbeat for worker {Index}", options.InstanceIndex);
            }

            try
            {
                await Task.Delay(interval, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                break;
            }
        }
    }
}
