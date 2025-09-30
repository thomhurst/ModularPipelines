using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Distributed.Abstractions;
using ModularPipelines.Distributed.Options;

namespace ModularPipelines.Distributed.Services;

/// <summary>
/// Background service that periodically removes stale workers from the orchestrator's registry.
/// </summary>
internal sealed class StaleWorkerCleanupService : BackgroundService
{
    private readonly INodeRegistry _nodeRegistry;
    private readonly ILogger<StaleWorkerCleanupService> _logger;
    private readonly DistributedPipelineOptions _options;

    public StaleWorkerCleanupService(
        INodeRegistry nodeRegistry,
        ILogger<StaleWorkerCleanupService> logger,
        IOptions<DistributedPipelineOptions> options)
    {
        _nodeRegistry = nodeRegistry ?? throw new ArgumentNullException(nameof(nodeRegistry));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));

        if (_options.Mode != DistributedExecutionMode.Orchestrator)
        {
            throw new InvalidOperationException(
                "StaleWorkerCleanupService can only run in Orchestrator mode");
        }
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Run cleanup every 30 seconds (half the heartbeat timeout for responsiveness)
        var cleanupInterval = TimeSpan.FromSeconds(Math.Max(30, _options.WorkerHeartbeatTimeout.TotalSeconds / 2));

        _logger.LogInformation(
            "Stale worker cleanup service starting. Cleanup interval: {Interval}, Heartbeat timeout: {Timeout}",
            cleanupInterval,
            _options.WorkerHeartbeatTimeout);

        try
        {
            using var timer = new PeriodicTimer(cleanupInterval);

            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                try
                {
                    await CleanupStaleWorkersAsync(stoppingToken);
                }
                catch (Exception ex) when (!stoppingToken.IsCancellationRequested)
                {
                    _logger.LogWarning(
                        ex,
                        "Error during stale worker cleanup. Will retry in {Interval}",
                        cleanupInterval);
                }
            }
        }
        catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Stale worker cleanup service stopping gracefully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Stale worker cleanup service encountered an error");
            throw;
        }
    }

    private async Task CleanupStaleWorkersAsync(CancellationToken cancellationToken)
    {
        _logger.LogDebug("Running stale worker cleanup");

        var workersBefore = await _nodeRegistry.GetAvailableWorkersAsync(cancellationToken);
        var countBefore = workersBefore.Count;

        await _nodeRegistry.RemoveStaleWorkersAsync(cancellationToken);

        var workersAfter = await _nodeRegistry.GetAvailableWorkersAsync(cancellationToken);
        var countAfter = workersAfter.Count;

        var removedCount = countBefore - countAfter;

        if (removedCount > 0)
        {
            _logger.LogInformation(
                "Removed {RemovedCount} stale worker(s). Active workers: {ActiveCount}",
                removedCount,
                countAfter);
        }
        else
        {
            _logger.LogDebug(
                "No stale workers found. Active workers: {ActiveCount}",
                countAfter);
        }
    }
}
