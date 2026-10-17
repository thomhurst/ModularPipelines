using System.Net;
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
    private const int MaximumRetryContentLength = 1024 * 1024;

    private readonly IModuleLoggerAccessor _loggerAccessor;
    private readonly IOptions<PipelineOptions> _pipelineOptions;
    private readonly TimeProvider _timeProvider;

    public ResilienceHttpHandler(
        IModuleLoggerAccessor loggerAccessor,
        IOptions<PipelineOptions> pipelineOptions)
        : this(loggerAccessor, pipelineOptions, TimeProvider.System)
    {
    }

    internal ResilienceHttpHandler(
        IModuleLoggerAccessor loggerAccessor,
        IOptions<PipelineOptions> pipelineOptions,
        TimeProvider timeProvider)
    {
        _loggerAccessor = loggerAccessor;
        _pipelineOptions = pipelineOptions;
        _timeProvider = timeProvider;
    }

    /// <inheritdoc/>
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var options = _pipelineOptions.Value.Http.Resilience ?? HttpResilienceOptions.Default;

        if (options.MaxRetryAttempts <= 0 || !CanRetry(request))
        {
            return await SendAttemptAsync(request, options.AttemptTimeout, cancellationToken).ConfigureAwait(false);
        }

        // Only byte-backed bodies with small declared lengths reach this point. Never consume upload streams for retries.
        byte[]? contentBytes = null;
        if (request.Content != null)
        {
            // A separate buffer enforces the cap even when the original content is already buffered.
            using var retryContent = new RetryBufferContent(request.Content);
            try
            {
                await retryContent.LoadIntoBufferAsync(MaximumRetryContentLength, cancellationToken).ConfigureAwait(false);
            }
            catch (HttpRequestException exception) when (exception.HttpRequestError == HttpRequestError.ConfigurationLimitExceeded)
            {
                return await SendAttemptAsync(request, options.AttemptTimeout, cancellationToken).ConfigureAwait(false);
            }

            contentBytes = await retryContent.ReadAsByteArrayAsync(cancellationToken).ConfigureAwait(false);
        }

        var retryShield = BuildRetryShield(options, cancellationToken);

        return await retryShield.ExecuteAsync(
            async ct => await SendAttemptAsync(CloneRequest(request, contentBytes), options.AttemptTimeout, ct).ConfigureAwait(false),
            cancellationToken).ConfigureAwait(false);
    }

    private static bool CanRetry(HttpRequestMessage request)
    {
        var retryEnabled = request.Options.TryGetValue(HttpResilienceOptions.RetryEnabled, out var enabled)
            ? enabled
            : request.Method == HttpMethod.Get || request.Method == HttpMethod.Head ||
              request.Method == HttpMethod.Put || request.Method == HttpMethod.Delete ||
              request.Method == HttpMethod.Options || request.Method == HttpMethod.Trace;

        if (!retryEnabled)
        {
            return false;
        }

        return request.Content == null ||
               (request.Content is ByteArrayContent &&
                request.Content.Headers.ContentLength is >= 0 and <= MaximumRetryContentLength);
    }

    private sealed class RetryBufferContent(HttpContent content) : HttpContent
    {
        protected override Task SerializeToStreamAsync(Stream stream, TransportContext? context)
            => content.CopyToAsync(stream, context);

        protected override Task SerializeToStreamAsync(Stream stream, TransportContext? context, CancellationToken cancellationToken)
            => content.CopyToAsync(stream, context, cancellationToken);

        protected override bool TryComputeLength(out long length)
        {
            length = content.Headers.ContentLength.GetValueOrDefault();
            return true;
        }
    }

    private async Task<HttpResponseMessage> SendAttemptAsync(
        HttpRequestMessage request,
        TimeSpan? attemptTimeout,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (attemptTimeout is null)
        {
            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }

        using var timeoutCts = new CancellationTokenSource(attemptTimeout.Value, _timeProvider);
        using var attemptCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeoutCts.Token);
        try
        {
            return await base.SendAsync(request, attemptCts.Token).ConfigureAwait(false);
        }
        catch (OperationCanceledException exception) when (timeoutCts.IsCancellationRequested && !cancellationToken.IsCancellationRequested)
        {
            // Use the same timeout shape as HttpClient, without cancelling the overall operation.
            throw new TaskCanceledException("The HTTP attempt timed out.", new TimeoutException("The HTTP attempt timed out.", exception), attemptCts.Token);
        }
    }

    private Shield<HttpResponseMessage> BuildRetryShield(HttpResilienceOptions options, CancellationToken cancellationToken)
    {
        return Shield.For<HttpResponseMessage>()
            .When<HttpRequestException>(_ => !cancellationToken.IsCancellationRequested && options.RetryOnHttpRequestException)
            .Or<TaskCanceledException>(exception =>
                !cancellationToken.IsCancellationRequested &&
                options.RetryOnTimeout &&
                exception.InnerException is TimeoutException)
            .OrResult(response => !cancellationToken.IsCancellationRequested && ShouldRetryStatusCode(response, options))
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

    private static TimeSpan CalculateDelay(int retryAttempt, HttpResilienceOptions options)
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

    private ValueTask OnRetry(RetryEvent<HttpResponseMessage> retryEvent)
    {
        var logger = _loggerAccessor.Logger;
        var outcome = retryEvent.Outcome;

        if (outcome.Exception != null)
        {
            logger.LogWarning("HTTP request failed with {ExceptionType}: {Message}. Retry attempt {RetryAttempt} after {Delay}ms",
                outcome.Exception.GetType().Name,
                outcome.Exception.Message,
                retryEvent.AttemptNumber,
                (int) retryEvent.Delay.TotalMilliseconds);
        }
        else if (outcome.Result != null)
        {
            try
            {
                logger.LogWarning("HTTP request returned {StatusCode}. Retry attempt {RetryAttempt} after {Delay}ms",
                    (int) outcome.Result.StatusCode,
                    retryEvent.AttemptNumber,
                    (int) retryEvent.Delay.TotalMilliseconds);
            }
            finally
            {
                outcome.Result.Dispose();
            }
        }

        return ValueTask.CompletedTask;
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
            VersionPolicy = original.VersionPolicy,
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

        // Clone options
        foreach (var option in original.Options)
        {
            clone.Options.TryAdd(option.Key, option.Value);
        }

        return clone;
    }
}
