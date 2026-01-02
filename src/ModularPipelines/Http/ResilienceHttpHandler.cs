using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Logging;
using ModularPipelines.Options;
using Polly;
using Polly.Retry;

namespace ModularPipelines.Http;

/// <summary>
/// A delegating handler that adds retry resilience to HTTP requests using Polly.
/// Handles transient failures including network errors and server errors (5xx).
/// </summary>
internal class ResilienceHttpHandler : DelegatingHandler
{
    private readonly IModuleLoggerProvider _loggerProvider;
    private readonly IOptions<PipelineOptions> _pipelineOptions;

    public ResilienceHttpHandler(
        IModuleLoggerProvider loggerProvider,
        IOptions<PipelineOptions> pipelineOptions)
    {
        _loggerProvider = loggerProvider;
        _pipelineOptions = pipelineOptions;
    }

    /// <inheritdoc/>
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var options = _pipelineOptions.Value.DefaultHttpResilienceOptions ?? HttpResilienceOptions.Default;

        if (options.MaxRetryAttempts <= 0)
        {
            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }

        // Buffer content upfront if present, so it can be reused across retries
        byte[]? contentBytes = null;
        string? contentType = null;
        if (request.Content != null)
        {
            contentBytes = await request.Content.ReadAsByteArrayAsync(cancellationToken).ConfigureAwait(false);
            contentType = request.Content.Headers.ContentType?.ToString();
        }

        var retryPolicy = BuildRetryPolicy(options);

        return await retryPolicy.ExecuteAsync(
            async ct => await base.SendAsync(CloneRequest(request, contentBytes, contentType), ct).ConfigureAwait(false),
            cancellationToken).ConfigureAwait(false);
    }

    private AsyncRetryPolicy<HttpResponseMessage> BuildRetryPolicy(HttpResilienceOptions options)
    {
        return Policy<HttpResponseMessage>
            .Handle<HttpRequestException>(ex => options.RetryOnHttpRequestException)
            .Or<TaskCanceledException>(ex =>
                options.RetryOnTimeout &&
                ex.InnerException is TimeoutException)
            .OrResult(response => ShouldRetryStatusCode(response, options))
            .WaitAndRetryAsync(
                options.MaxRetryAttempts,
                retryAttempt => CalculateDelay(retryAttempt, options),
                OnRetry);
    }

    private static bool ShouldRetryStatusCode(HttpResponseMessage response, HttpResilienceOptions options)
    {
        return options.RetryableStatusCodes.Contains(response.StatusCode);
    }

    private TimeSpan CalculateDelay(int retryAttempt, HttpResilienceOptions options)
    {
        TimeSpan baseDelay;

        if (options.UseExponentialBackoff)
        {
            // Exponential backoff: delay = initialDelay * 2^(attempt-1)
            var exponentialDelay = options.InitialDelay.TotalMilliseconds * Math.Pow(2, retryAttempt - 1);
            baseDelay = TimeSpan.FromMilliseconds(Math.Min(exponentialDelay, options.MaxDelay.TotalMilliseconds));
        }
        else
        {
            baseDelay = options.InitialDelay;
        }

        // Apply jitter if configured (use Random.Shared for thread-safety)
        if (options.JitterFactor > 0)
        {
            var jitter = baseDelay.TotalMilliseconds * options.JitterFactor * ((Random.Shared.NextDouble() * 2) - 1);
            var delayWithJitter = baseDelay.TotalMilliseconds + jitter;
            return TimeSpan.FromMilliseconds(Math.Max(0, delayWithJitter));
        }

        return baseDelay;
    }

    private Task OnRetry(DelegateResult<HttpResponseMessage> outcome, TimeSpan delay, int retryAttempt, Polly.Context context)
    {
        var logger = _loggerProvider.GetLogger();

        if (outcome.Exception != null)
        {
            logger.LogWarning("HTTP request failed with {ExceptionType}: {Message}. Retry attempt {RetryAttempt} after {Delay}ms",
                outcome.Exception.GetType().Name,
                outcome.Exception.Message,
                retryAttempt,
                (int)delay.TotalMilliseconds);
        }
        else if (outcome.Result != null)
        {
            logger.LogWarning("HTTP request returned {StatusCode}. Retry attempt {RetryAttempt} after {Delay}ms",
                (int)outcome.Result.StatusCode,
                retryAttempt,
                (int)delay.TotalMilliseconds);
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// Clones an HTTP request message for retry purposes.
    /// This is necessary because HttpRequestMessage can only be sent once.
    /// </summary>
    /// <param name="original">The original request to clone.</param>
    /// <param name="contentBytes">Pre-buffered content bytes, or null if no content.</param>
    /// <param name="contentType">The content type header value, or null if no content.</param>
    private static HttpRequestMessage CloneRequest(HttpRequestMessage original, byte[]? contentBytes, string? contentType)
    {
        var clone = new HttpRequestMessage(original.Method, original.RequestUri)
        {
            Version = original.Version,
#if NET5_0_OR_GREATER
            VersionPolicy = original.VersionPolicy,
#endif
        };

        // Create fresh content from buffered bytes for each retry attempt
        if (contentBytes != null)
        {
            clone.Content = new ByteArrayContent(contentBytes);
            if (!string.IsNullOrEmpty(contentType))
            {
                clone.Content.Headers.TryAddWithoutValidation("Content-Type", contentType);
            }
        }

        // Clone headers
        foreach (var header in original.Headers)
        {
            clone.Headers.TryAddWithoutValidation(header.Key, header.Value);
        }

#if NET5_0_OR_GREATER
        // Clone options
        foreach (var option in original.Options)
        {
            clone.Options.TryAdd(option.Key, option.Value);
        }
#else
        // Clone properties for older frameworks
        foreach (var property in original.Properties)
        {
            clone.Properties[property.Key] = property.Value;
        }
#endif

        return clone;
    }
}
