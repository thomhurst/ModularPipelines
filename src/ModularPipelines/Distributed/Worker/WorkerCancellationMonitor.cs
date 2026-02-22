using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Engine;

namespace ModularPipelines.Distributed.Worker;

internal class WorkerCancellationMonitor(
    IDistributedCoordinator coordinator,
    EngineCancellationToken engineCancellationToken,
    ILogger<WorkerCancellationMonitor> logger) : BackgroundService
{
    private readonly IDistributedCoordinator _coordinator = coordinator;
    private readonly EngineCancellationToken _engineCancellationToken = engineCancellationToken;
    private readonly ILogger<WorkerCancellationMonitor> _logger = logger;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var signal = await _coordinator.IsCancellationRequestedAsync(stoppingToken);
                if (signal is not null)
                {
                    _logger.LogWarning("Cancellation signal received: {Reason}", signal.Reason);
                    _engineCancellationToken.CancelWithReason(signal.Reason);
                    break;
                }
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                _logger.LogWarning(ex, "Failed to check cancellation signal");
            }

            try
            {
                await Task.Delay(TimeSpan.FromSeconds(2), stoppingToken);
            }
            catch (OperationCanceledException)
            {
                break;
            }
        }
    }
}
