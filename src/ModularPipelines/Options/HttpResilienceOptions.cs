using System.Net;

namespace ModularPipelines.Options;

/// <summary>
/// Options for configuring HTTP resilience behavior (retry policies for transient failures).
/// </summary>
/// <remarks>
/// <para>Configure via <see cref="PipelineHttpOptions.Resilience"/> for global defaults,
/// or create custom options for specific use cases.</para>
/// <para>Default behavior:</para>
/// <list type="bullet">
/// <item><description>3 retry attempts with exponential backoff</description></item>
/// <item><description>Retries on network failures (HttpRequestException)</description></item>
/// <item><description>Retries on 5xx server errors and 408 Request Timeout</description></item>
/// <item><description>Retries only GET, HEAD, PUT, DELETE, OPTIONS, and TRACE unless
/// overridden by <see cref="RetryEnabled"/> on <see cref="HttpRequestMessage.Options"/></description></item>
/// </list>
/// <para>Requests with bodies are retried only for <see cref="ByteArrayContent"/> (including
/// <see cref="StringContent"/> and <see cref="FormUrlEncodedContent"/>) with a known length of at most
/// 1 MiB (1,048,576 bytes). Streamed content, other content types, and bodies with unknown or larger
/// lengths are sent once without buffering by the resilience handler, even when retries are explicitly enabled.</para>
/// <para>Replay buffering also enforces this limit against the serialized body. If the body exceeds its
/// declared length and the limit, the original request is sent once without retries.</para>
/// <example>
/// Opt in only when the server guarantees that repeating the operation is safe, for example with an idempotency key:
/// <code>
/// request.Options.Set(HttpResilienceOptions.RetryEnabled, true);
/// </code>
/// </example>
/// </remarks>
public record HttpResilienceOptions
{
    private IReadOnlyCollection<HttpStatusCode> _retryableStatusCodes = DefaultRetryableStatusCodes;
    private TimeSpan? _attemptTimeout;

    /// <summary>
    /// Gets the timeout for each send attempt, including the initial attempt when retries are disabled.
    /// The default is null, which adds no per-attempt timeout.
    /// </summary>
    /// <remarks>
    /// The timeout covers sending the request and receiving response headers, not consuming the response body.
    /// Set a positive duration no greater than 4,294,967,294 milliseconds; use null to disable it.
    /// An expired attempt can be retried when <see cref="RetryOnTimeout"/> is true.
    /// Caller cancellation and the overall request timeout still limit the entire operation, including retry delays.
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException">The value is not a supported positive timeout.</exception>
    public TimeSpan? AttemptTimeout
    {
        get => _attemptTimeout;
        init
        {
            if (value is { } timeout && (timeout <= TimeSpan.Zero || timeout.TotalMilliseconds > uint.MaxValue - 1))
            {
                throw new ArgumentOutOfRangeException(nameof(value), value, "Attempt timeout must be positive and no greater than 4,294,967,294 milliseconds.");
            }

            _attemptTimeout = value;
        }
    }

    /// <summary>
    /// Gets the per-request option that enables or disables retries, overriding the HTTP method default.
    /// Set this on <see cref="HttpRequestMessage.Options"/> only when repeating the operation is safe.
    /// This does not override disabled retries or the request body restrictions.
    /// </summary>
    public static HttpRequestOptionsKey<bool> RetryEnabled { get; } = new("ModularPipelines.Http.RetryEnabled");

    /// <summary>
    /// Gets the maximum number of retry attempts. Default is 3.
    /// Set to 0 to disable retries.
    /// </summary>
    public int MaxRetryAttempts { get; init; } = 3;

    /// <summary>
    /// Gets the initial delay before the first retry. Default is 1 second.
    /// Subsequent retries use exponential backoff based on this value.
    /// </summary>
    public TimeSpan InitialDelay { get; init; } = TimeSpan.FromSeconds(1);

    /// <summary>
    /// Gets the maximum delay between retries. Default is 30 seconds.
    /// This caps the exponential backoff to prevent excessively long waits.
    /// </summary>
    public TimeSpan MaxDelay { get; init; } = TimeSpan.FromSeconds(30);

    /// <summary>
    /// Gets a value indicating whether to use exponential backoff for retry delays. Default is true.
    /// When false, uses the <see cref="InitialDelay"/> for all retries.
    /// </summary>
    public bool UseExponentialBackoff { get; init; } = true;

    /// <summary>
    /// Gets the jitter factor to add randomness to retry delays. Default is 0.2 (20%).
    /// This helps prevent thundering herd problems when multiple clients retry simultaneously.
    /// Set to 0 to disable jitter.
    /// </summary>
    public double JitterFactor { get; init; } = 0.2;

    /// <summary>
    /// Gets the HTTP status codes that should trigger a retry.
    /// Default includes server errors (5xx) and 408 Request Timeout.
    /// </summary>
    public IReadOnlyCollection<HttpStatusCode> RetryableStatusCodes
    {
        get => _retryableStatusCodes;
        init => _retryableStatusCodes = Array.AsReadOnly(value.ToArray());
    }

    /// <summary>
    /// Gets a value indicating whether to retry on <see cref="HttpRequestException"/> (network failures). Default is true.
    /// </summary>
    public bool RetryOnHttpRequestException { get; init; } = true;

    /// <summary>
    /// Gets a value indicating whether to retry on <see cref="TaskCanceledException"/> caused by timeouts. Default is true.
    /// Note: This only applies to timeout-caused cancellations, not user-initiated cancellations.
    /// </summary>
    public bool RetryOnTimeout { get; init; } = true;

    /// <summary>
    /// Gets the default retryable HTTP status codes: 5xx server errors and 408 Request Timeout.
    /// </summary>
    public static IReadOnlyCollection<HttpStatusCode> DefaultRetryableStatusCodes { get; } =
        Array.AsReadOnly(
        [
            HttpStatusCode.RequestTimeout,           // 408
            HttpStatusCode.InternalServerError,      // 500
            HttpStatusCode.BadGateway,               // 502
            HttpStatusCode.ServiceUnavailable,       // 503
            HttpStatusCode.GatewayTimeout,           // 504
        ]);

    /// <summary>
    /// Gets the default resilience options with 3 retries and exponential backoff.
    /// </summary>
    public static HttpResilienceOptions Default { get; } = new();

    /// <summary>
    /// Gets disabled resilience options (no retries).
    /// </summary>
    public static HttpResilienceOptions None { get; } = new() { MaxRetryAttempts = 0 };

    /// <summary>
    /// Gets aggressive retry options with 5 retries and shorter delays.
    /// Suitable for critical operations that must succeed.
    /// </summary>
    public static HttpResilienceOptions Aggressive { get; } = new()
    {
        MaxRetryAttempts = 5,
        InitialDelay = TimeSpan.FromMilliseconds(500),
        MaxDelay = TimeSpan.FromSeconds(15),
    };

    /// <summary>
    /// Gets conservative retry options with 2 retries and longer delays.
    /// Suitable for non-critical operations where you want to be gentle on the server.
    /// </summary>
    public static HttpResilienceOptions Conservative { get; } = new()
    {
        MaxRetryAttempts = 2,
        InitialDelay = TimeSpan.FromSeconds(2),
        MaxDelay = TimeSpan.FromSeconds(60),
    };
}
