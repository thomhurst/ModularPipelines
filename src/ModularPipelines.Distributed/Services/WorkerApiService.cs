using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Distributed.Communication.Messages;
using ModularPipelines.Distributed.Engine;
using ModularPipelines.Distributed.Options;

namespace ModularPipelines.Distributed.Services;

/// <summary>
/// Hosted service that provides HTTP API endpoints for workers.
/// </summary>
internal sealed class WorkerApiService : IHostedService
{
    private readonly WorkerModuleExecutionHandler _executionHandler;
    private readonly IOptions<DistributedPipelineOptions> _options;
    private readonly ILogger<WorkerApiService> _logger;
    private WebApplication? _app;
    private string? _workerId;

    public WorkerApiService(
        WorkerModuleExecutionHandler executionHandler,
        IOptions<DistributedPipelineOptions> options,
        ILogger<WorkerApiService> logger)
    {
        _executionHandler = executionHandler ?? throw new ArgumentNullException(nameof(executionHandler));
        _options = options ?? throw new ArgumentNullException(nameof(options));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Starts the HTTP API server.
    /// </summary>
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var port = _options.Value.WorkerPort;
        _workerId = _options.Value.WorkerId ?? $"worker-{Environment.MachineName}-{Guid.NewGuid():N}";

        _logger.LogInformation(
            "Starting worker HTTP API on port {Port} (Worker ID: {WorkerId})",
            port,
            _workerId);

        var builder = WebApplication.CreateSlimBuilder();

        _app = builder.Build();

        // Map endpoints
        MapExecutionEndpoints(_app);
        MapHealthEndpoint(_app);

        // Start the web application
        _app.Urls.Add($"http://*:{port}");

        await _app.StartAsync(cancellationToken);

        _logger.LogInformation(
            "✓ Worker HTTP API started on http://*:{Port}",
            port);
    }

    /// <summary>
    /// Stops the HTTP API server.
    /// </summary>
    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stopping worker HTTP API");

        if (_app != null)
        {
            await _app.StopAsync(cancellationToken);
            await _app.DisposeAsync();
        }

        _logger.LogInformation("✓ Worker HTTP API stopped");
    }

    private void MapExecutionEndpoints(WebApplication app)
    {
        // POST /api/execution/execute
        app.MapPost("/api/execution/execute", async (
            ModuleExecutionRequest request,
            WorkerModuleExecutionHandler handler,
            IOptions<DistributedPipelineOptions> options,
            ILogger<WorkerApiService> logger) =>
        {
            var workerId = options.Value.WorkerId ?? "unknown";

            logger.LogInformation(
                "Executing module {ModuleType} (execution {ExecutionId})",
                request.ModuleTypeName,
                request.ExecutionId);

            try
            {
                var response = await handler.ExecuteModuleAsync(request, workerId);

                if (response.Success)
                {
                    logger.LogInformation(
                        "✓ Module execution {ExecutionId} completed successfully in {Duration}",
                        request.ExecutionId,
                        response.Duration);

                    return Results.Ok(response);
                }
                else
                {
                    logger.LogWarning(
                        "✗ Module execution {ExecutionId} failed: {ErrorMessage}",
                        request.ExecutionId,
                        response.ErrorMessage);

                    return Results.Json(response, statusCode: 500);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "✗ Unhandled exception during module execution {ExecutionId}",
                    request.ExecutionId);

                return Results.Json(
                    new ModuleResultResponse
                    {
                        ExecutionId = request.ExecutionId,
                        Success = false,
                        ErrorMessage = ex.Message,
                        ExceptionType = ex.GetType().FullName ?? ex.GetType().Name,
                        StackTrace = ex.StackTrace,
                        Duration = TimeSpan.Zero,
                        StartTime = DateTimeOffset.UtcNow,
                        EndTime = DateTimeOffset.UtcNow,
                        WorkerId = workerId,
                    },
                    statusCode: 500);
            }
        });

        // POST /api/execution/cancel
        app.MapPost("/api/execution/cancel", (
            CancellationMessage message,
            WorkerModuleExecutionHandler handler,
            ILogger<WorkerApiService> logger) =>
        {
            logger.LogInformation(
                "Cancellation requested for execution {ExecutionId}: {Reason}",
                message.ExecutionId,
                message.Reason);

            var cancelled = handler.CancelExecution(message.ExecutionId);

            if (cancelled)
            {
                logger.LogInformation(
                    "✓ Execution {ExecutionId} cancelled successfully",
                    message.ExecutionId);

                return Results.Ok(new CancellationResponse
                {
                    Success = true,
                });
            }
            else
            {
                logger.LogWarning(
                    "✗ Execution {ExecutionId} not found for cancellation",
                    message.ExecutionId);

                return Results.NotFound(new CancellationResponse
                {
                    Success = false,
                    ErrorMessage = "Execution not found",
                });
            }
        });
    }

    private void MapHealthEndpoint(WebApplication app)
    {
        // GET /api/health
        app.MapGet("/api/health", (
            WorkerModuleExecutionHandler handler,
            IOptions<DistributedPipelineOptions> options,
            ILogger<WorkerApiService> logger) =>
        {
            try
            {
                var currentLoad = handler.GetCurrentExecutionCount();
                var maxLoad = options.Value.WorkerCapabilities?.MaxParallelModules ?? 1;

                logger.LogDebug(
                    "Health check: {CurrentLoad}/{MaxLoad} modules executing",
                    currentLoad,
                    maxLoad);

                return Results.Ok(new
                {
                    status = "healthy",
                    currentLoad,
                    maxLoad,
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
/// Response message for cancellation requests.
/// </summary>
public sealed class CancellationResponse
{
    /// <summary>
    /// Gets or sets a value indicating whether the cancellation was successful.
    /// </summary>
    public required bool Success { get; init; }

    /// <summary>
    /// Gets or sets the error message if cancellation failed.
    /// </summary>
    public string? ErrorMessage { get; init; }
}
