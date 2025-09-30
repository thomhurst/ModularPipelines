using System.IO.Compression;
using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Distributed.Abstractions;
using ModularPipelines.Distributed.Communication.Messages;
using ModularPipelines.Distributed.Options;
using Polly;
using Polly.Retry;

namespace ModularPipelines.Distributed.Communication;

/// <summary>
/// HTTP-based implementation of <see cref="IRemoteCommunicator"/> with retry logic and compression support.
/// </summary>
internal sealed class HttpRemoteCommunicator : IRemoteCommunicator, IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<HttpRemoteCommunicator> _logger;
    private readonly DistributedPipelineOptions _options;
    private readonly AsyncRetryPolicy<HttpResponseMessage> _retryPolicy;
    private readonly JsonSerializerOptions _jsonOptions;

    public HttpRemoteCommunicator(
        IHttpClientFactory httpClientFactory,
        ILogger<HttpRemoteCommunicator> logger,
        IOptions<DistributedPipelineOptions> options)
    {
        ArgumentNullException.ThrowIfNull(httpClientFactory);
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));

        _httpClient = httpClientFactory.CreateClient("ModularPipelines.Distributed");
        _httpClient.Timeout = _options.RemoteExecutionTimeout;

        _retryPolicy = Policy<HttpResponseMessage>
            .Handle<HttpRequestException>()
            .Or<TaskCanceledException>()
            .OrResult(r => !r.IsSuccessStatusCode && r.StatusCode != HttpStatusCode.BadRequest)
            .WaitAndRetryAsync(
                _options.MaxRetryAttempts,
                retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                onRetry: (outcome, timespan, retryCount, context) =>
                {
                    _logger.LogWarning(
                        "Request failed (attempt {RetryCount}/{MaxRetries}). Retrying in {Delay}s. Status: {Status}",
                        retryCount,
                        _options.MaxRetryAttempts,
                        timespan.TotalSeconds,
                        outcome.Result?.StatusCode);
                });

        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false,
        };
    }

    /// <inheritdoc />
    public async Task<ModuleResultResponse> ExecuteModuleAsync(
        WorkerNode worker,
        ModuleExecutionRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(worker);
        ArgumentNullException.ThrowIfNull(request);

        var url = $"{worker.Endpoint}/api/execution/execute";

        _logger.LogDebug(
            "Sending execution request {ExecutionId} to worker {WorkerId} at {Endpoint}",
            request.ExecutionId,
            worker.Id,
            url);

        try
        {
            var response = await _retryPolicy.ExecuteAsync(async () =>
            {
                var httpRequest = await CreateHttpRequestAsync(url, request, cancellationToken);
                return await _httpClient.SendAsync(httpRequest, cancellationToken);
            });

            response.EnsureSuccessStatusCode();

            var result = await DeserializeResponseAsync<ModuleResultResponse>(response, cancellationToken);

            if (result == null)
            {
                throw new InvalidOperationException("Worker returned null result");
            }

            _logger.LogInformation(
                "Execution {ExecutionId} completed on worker {WorkerId}. Success: {Success}, Duration: {Duration}",
                request.ExecutionId,
                worker.Id,
                result.Success,
                result.Duration);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to execute module {ModuleType} on worker {WorkerId}",
                request.ModuleTypeName,
                worker.Id);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<bool> HealthCheckAsync(
        WorkerNode worker,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(worker);

        var url = $"{worker.Endpoint}/api/health";

        try
        {
            using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            cts.CancelAfter(TimeSpan.FromSeconds(10));

            var response = await _httpClient.GetAsync(url, cts.Token);

            var isHealthy = response.IsSuccessStatusCode;

            _logger.LogDebug(
                "Health check for worker {WorkerId}: {Status}",
                worker.Id,
                isHealthy ? "Healthy" : "Unhealthy");

            return isHealthy;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(
                ex,
                "Health check failed for worker {WorkerId} at {Endpoint}",
                worker.Id,
                worker.Endpoint);
            return false;
        }
    }

    /// <inheritdoc />
    public async Task CancelExecutionAsync(
        WorkerNode worker,
        string executionId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(worker);
        ArgumentException.ThrowIfNullOrWhiteSpace(executionId);

        var url = $"{worker.Endpoint}/api/execution/cancel";

        var message = new CancellationMessage
        {
            ExecutionId = executionId,
            Reason = "Cancelled by orchestrator",
        };

        try
        {
            var httpRequest = await CreateHttpRequestAsync(url, message, cancellationToken);
            var response = await _httpClient.SendAsync(httpRequest, cancellationToken);

            response.EnsureSuccessStatusCode();

            _logger.LogInformation(
                "Cancellation signal sent for execution {ExecutionId} on worker {WorkerId}",
                executionId,
                worker.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to cancel execution {ExecutionId} on worker {WorkerId}",
                executionId,
                worker.Id);
            throw;
        }
    }

    private async Task<HttpRequestMessage> CreateHttpRequestAsync<T>(
        string url,
        T payload,
        CancellationToken cancellationToken)
    {
        var json = JsonSerializer.Serialize(payload, _jsonOptions);
        var content = Encoding.UTF8.GetBytes(json);

        // Validate payload size
        if (content.Length > _options.MaxPayloadSize)
        {
            throw new InvalidOperationException(
                $"Payload size ({content.Length} bytes) exceeds maximum allowed size ({_options.MaxPayloadSize} bytes)");
        }

        HttpContent httpContent;

        if (_options.EnableCompression && content.Length > 1024) // Only compress if > 1KB
        {
            var compressedContent = await CompressAsync(content, cancellationToken);
            httpContent = new ByteArrayContent(compressedContent);
            httpContent.Headers.Add("Content-Encoding", "gzip");

            _logger.LogDebug(
                "Compressed payload: {OriginalSize} bytes -> {CompressedSize} bytes ({Ratio:P1} reduction)",
                content.Length,
                compressedContent.Length,
                1.0 - ((double)compressedContent.Length / content.Length));
        }
        else
        {
            httpContent = new ByteArrayContent(content);
        }

        httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

        var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = httpContent,
        };

        return request;
    }

    private async Task<T?> DeserializeResponseAsync<T>(
        HttpResponseMessage response,
        CancellationToken cancellationToken)
    {
        Stream stream = await response.Content.ReadAsStreamAsync(cancellationToken);

        // Check if response is compressed
        if (response.Content.Headers.ContentEncoding.Contains("gzip"))
        {
            stream = new GZipStream(stream, CompressionMode.Decompress);
        }

        return await JsonSerializer.DeserializeAsync<T>(stream, _jsonOptions, cancellationToken);
    }

    private static async Task<byte[]> CompressAsync(byte[] data, CancellationToken cancellationToken)
    {
        using var outputStream = new MemoryStream();
        await using (var gzipStream = new GZipStream(outputStream, CompressionLevel.Fastest))
        {
            await gzipStream.WriteAsync(data, cancellationToken);
        }

        return outputStream.ToArray();
    }

    public void Dispose()
    {
        _httpClient?.Dispose();
    }
}
