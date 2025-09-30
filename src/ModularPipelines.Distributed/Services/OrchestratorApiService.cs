using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Distributed.Abstractions;
using ModularPipelines.Distributed.Communication.Messages;
using ModularPipelines.Distributed.Options;

namespace ModularPipelines.Distributed.Services;

/// <summary>
/// Hosted service that provides HTTP API endpoints for the orchestrator.
/// </summary>
internal sealed class OrchestratorApiService : IHostedService
{
    private readonly INodeRegistry _nodeRegistry;
    private readonly IOptions<DistributedPipelineOptions> _options;
    private readonly ILogger<OrchestratorApiService> _logger;
    private WebApplication? _app;

    public OrchestratorApiService(
        INodeRegistry nodeRegistry,
        IOptions<DistributedPipelineOptions> options,
        ILogger<OrchestratorApiService> logger)
    {
        _nodeRegistry = nodeRegistry ?? throw new ArgumentNullException(nameof(nodeRegistry));
        _options = options ?? throw new ArgumentNullException(nameof(options));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Starts the HTTP API server.
    /// </summary>
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var port = _options.Value.OrchestratorPort;

        _logger.LogInformation(
            "Starting orchestrator HTTP API on port {Port}",
            port);

        var builder = WebApplication.CreateSlimBuilder();

        _app = builder.Build();

        // Map endpoints
        MapWorkerEndpoints(_app);
        MapHealthEndpoint(_app);

        // Start the web application
        _app.Urls.Add($"http://*:{port}");

        await _app.StartAsync(cancellationToken);

        _logger.LogInformation(
            "✓ Orchestrator HTTP API started on http://*:{Port}",
            port);
    }

    /// <summary>
    /// Stops the HTTP API server.
    /// </summary>
    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stopping orchestrator HTTP API");

        if (_app != null)
        {
            await _app.StopAsync(cancellationToken);
            await _app.DisposeAsync();
        }

        _logger.LogInformation("✓ Orchestrator HTTP API stopped");
    }

    private void MapWorkerEndpoints(WebApplication app)
    {
        // POST /api/workers/register
        app.MapPost("/api/workers/register", async (
            WorkerRegistrationMessage message,
            INodeRegistry nodeRegistry,
            ILogger<OrchestratorApiService> logger) =>
        {
            try
            {
                logger.LogInformation(
                    "Registering worker {WorkerId} from {Endpoint}",
                    message.WorkerNode.Id,
                    message.WorkerNode.Endpoint);

                await nodeRegistry.RegisterWorkerAsync(message.WorkerNode);

                return Results.Ok(new WorkerRegistrationResponse
                {
                    Success = true,
                    WorkerId = message.WorkerNode.Id,
                    HeartbeatIntervalSeconds = 30,
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to register worker");

                return Results.BadRequest(new WorkerRegistrationResponse
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                });
            }
        });

        // POST /api/workers/heartbeat
        app.MapPost("/api/workers/heartbeat", async (
            HeartbeatMessage message,
            INodeRegistry nodeRegistry,
            ILogger<OrchestratorApiService> logger) =>
        {
            try
            {
                logger.LogDebug(
                    "Heartbeat received from worker {WorkerId} (load: {CurrentLoad})",
                    message.WorkerId,
                    message.CurrentLoad);

                await nodeRegistry.UpdateHeartbeatAsync(message.WorkerId);

                return Results.Ok(new HeartbeatResponse
                {
                    Acknowledged = true,
                    ShouldDrain = false,
                });
            }
            catch (Exception ex)
            {
                logger.LogWarning(
                    ex,
                    "Failed to process heartbeat from worker {WorkerId}",
                    message.WorkerId);

                return Results.BadRequest(new HeartbeatResponse
                {
                    Acknowledged = false,
                });
            }
        });

        // GET /api/workers
        app.MapGet("/api/workers", async (
            INodeRegistry nodeRegistry,
            ILogger<OrchestratorApiService> logger) =>
        {
            try
            {
                var workers = await nodeRegistry.GetAvailableWorkersAsync();

                logger.LogDebug("Retrieved {WorkerCount} available workers", workers.Count);

                return Results.Ok(workers);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to retrieve workers");
                return Results.StatusCode(500);
            }
        });

        // DELETE /api/workers/{workerId}
        app.MapDelete("/api/workers/{workerId}", async (
            string workerId,
            INodeRegistry nodeRegistry,
            ILogger<OrchestratorApiService> logger) =>
        {
            try
            {
                logger.LogInformation("Unregistering worker {WorkerId}", workerId);

                await nodeRegistry.UnregisterWorkerAsync(workerId);

                return Results.Ok(new { success = true });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to unregister worker {WorkerId}", workerId);
                return Results.StatusCode(500);
            }
        });
    }

    private void MapHealthEndpoint(WebApplication app)
    {
        // GET /api/health
        app.MapGet("/api/health", async (
            INodeRegistry nodeRegistry,
            ILogger<OrchestratorApiService> logger) =>
        {
            try
            {
                var workers = await nodeRegistry.GetAvailableWorkersAsync();

                return Results.Ok(new
                {
                    status = "healthy",
                    availableWorkers = workers.Count,
                    timestamp = DateTimeOffset.UtcNow,
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Health check failed");

                return Results.Json(
                    new
                    {
                        status = "unhealthy",
                        error = ex.Message,
                        timestamp = DateTimeOffset.UtcNow,
                    },
                    statusCode: 503);
            }
        });
    }
}

/// <summary>
/// Response message for worker registration.
/// </summary>
public sealed class WorkerRegistrationResponse
{
    /// <summary>
    /// Gets or sets a value indicating whether the registration was successful.
    /// </summary>
    public required bool Success { get; init; }

    /// <summary>
    /// Gets or sets the worker ID assigned by the orchestrator.
    /// </summary>
    public string? WorkerId { get; init; }

    /// <summary>
    /// Gets or sets the heartbeat interval in seconds.
    /// </summary>
    public int HeartbeatIntervalSeconds { get; init; }

    /// <summary>
    /// Gets or sets the error message if registration failed.
    /// </summary>
    public string? ErrorMessage { get; init; }
}

/// <summary>
/// Response message for heartbeat.
/// </summary>
public sealed class HeartbeatResponse
{
    /// <summary>
    /// Gets or sets a value indicating whether the heartbeat was acknowledged.
    /// </summary>
    public required bool Acknowledged { get; init; }

    /// <summary>
    /// Gets or sets a value indicating whether the worker should drain and stop accepting new work.
    /// </summary>
    public bool ShouldDrain { get; init; }
}
