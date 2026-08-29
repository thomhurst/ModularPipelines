using Kevlar;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Logging;
using ModularPipelines.Options;

namespace ModularPipelines.Http;

/// <summary>
/// A delegating handler that adds retry resilience to HTTP requests using Kevlar.
/// Handles transient failures including network errors and server errors (5xx).
/// </summary>
internal class ResilienceHttpHandler : DelegatingHandler
{
    private readonly IModuleLoggerAccessor _loggerAccessor;
    private readonly IOptions<PipelineOptions> _pipelineOptions;

    public ResilienceHttpHandler(
        IModuleLoggerAccessor loggerAccessor,
        IOptions<PipelineOptions> pipelineOptions)
    {
        _loggerAccessor = loggerAccessor;
        _pipelineOptions = pipelineOptions;
    }

    /// <inheritdoc/>
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var options = _pipelineOptions.Value.Http.Resilience ?? HttpResilienceOptions.Default;

        if (options.MaxRetryAttempts <= 0)
        {
            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }

        // Buffer content upfront if present, so it can be reused across retries
        byte[]? contentBytes = null;
        if (request.Content != null)
        {
            contentBytes = await request.Content.ReadAsByteArrayAsync(cancellationToken).ConfigureAwait(false);
        }

        var retryShield = BuildRetryShield(options);

        return await retryShield.ExecuteAsync(
            async ct => await base.SendAsync(CloneRequest(request, contentBytes), ct).ConfigureAwait(false),
            cancellationToken).ConfigureAwait(false);
    }

    private Shield<HttpResponseMessage> BuildRetryShield(HttpResilienceOptions options)
    {
        return Shield.For<HttpResponseMessage>()
            .When<HttpRequestException>(_ => options.RetryOnHttpRequestException)
            .Or<TaskCanceledException>(exception =>
                options.RetryOnTimeout &&
                exception.InnerException is TimeoutException)
            .OrResult(response => ShouldRetryStatusCode(response, options))
            .Retry(retryOptions =>
            {
                retryOptions.MaxRetries = options.MaxRetryAttempts;
                retryOptions.Backoff = Backoff.Custom(retryAttempt => CalculateDelay(retryAttempt, options));
                retryOptions.OnRetry = OnRetry;
            });
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

    private void OnRetry(RetryEvent<HttpResponseMessage> retryEvent)
    {
        var logger = _loggerAccessor.Logger;
        var outcome = retryEvent.Outcome;

        if (outcome.Exception != null)
        {
            logger.LogWarning("HTTP request failed with {ExceptionType}: {Message}. Retry attempt {RetryAttempt} after {Delay}ms",
                outcome.Exception.GetType().Name,
                outcome.Exception.Message,
                retryEvent.Attempt,
                (int) retryEvent.Delay.TotalMilliseconds);
        }
        else if (outcome.Result != null)
        {
            try
            {
                logger.LogWarning("HTTP request returned {StatusCode}. Retry attempt {RetryAttempt} after {Delay}ms",
                    (int) outcome.Result.StatusCode,
                    retryEvent.Attempt,
                    (int) retryEvent.Delay.TotalMilliseconds);
            }
            finally
            {
                outcome.Result.Dispose();
            }
        }
    }

    /// <summary>
    /// Clones an HTTP request message for retry purposes.
    /// This is necessary because HttpRequestMessage can only be sent once.
    /// </summary>
    /// <param name="original">The original request to clone.</param>
    /// <param name="contentBytes">Pre-buffered content bytes, or null if no content.</param>
    private static HttpRequestMessage CloneRequest(HttpRequestMessage original, byte[]? contentBytes)
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
            foreach (var header in original.Content!.Headers)
            {
                clone.Content.Headers.TryAddWithoutValidation(header.Key, header.Value);
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
