using System.Net;

namespace ModularPipelines.Options;

/// <summary>
/// Options for configuring HTTP resilience behavior (retry policies for transient failures).
/// </summary>
/// <remarks>
/// <para>Configure via <see cref="PipelineOptions.DefaultHttpResilienceOptions"/> for global defaults,
/// or create custom options for specific use cases.</para>
/// <para>Default behavior:</para>
/// <list type="bullet">
/// <item><description>3 retry attempts with exponential backoff</description></item>
/// <item><description>Retries on network failures (HttpRequestException)</description></item>
/// <item><description>Retries on 5xx server errors and 408 Request Timeout</description></item>
/// </list>
/// <para><strong>Note:</strong> Request bodies are buffered into memory before the first attempt to enable retries.
/// For very large request payloads, consider disabling retries or increasing available memory.</para>
/// </remarks>
public record HttpResilienceOptions
{
    /// <summary>
    /// Gets or sets the maximum number of retry attempts. Default is 3.
    /// Set to 0 to disable retries.
    /// </summary>
    public int MaxRetryAttempts { get; init; } = 3;

    /// <summary>
    /// Gets or sets the initial delay before the first retry. Default is 1 second.
    /// Subsequent retries use exponential backoff based on this value.
    /// </summary>
    public TimeSpan InitialDelay { get; init; } = TimeSpan.FromSeconds(1);

    /// <summary>
    /// Gets or sets the maximum delay between retries. Default is 30 seconds.
    /// This caps the exponential backoff to prevent excessively long waits.
    /// </summary>
    public TimeSpan MaxDelay { get; init; } = TimeSpan.FromSeconds(30);

    /// <summary>
    /// Gets or sets whether to use exponential backoff for retry delays. Default is true.
    /// When false, uses the <see cref="InitialDelay"/> for all retries.
    /// </summary>
    public bool UseExponentialBackoff { get; init; } = true;

    /// <summary>
    /// Gets or sets the jitter factor to add randomness to retry delays. Default is 0.2 (20%).
    /// This helps prevent thundering herd problems when multiple clients retry simultaneously.
    /// Set to 0 to disable jitter.
    /// </summary>
    public double JitterFactor { get; init; } = 0.2;

    /// <summary>
    /// Gets or sets the HTTP status codes that should trigger a retry.
    /// Default includes server errors (5xx) and 408 Request Timeout.
    /// </summary>
    public IReadOnlyCollection<HttpStatusCode> RetryableStatusCodes { get; init; } = DefaultRetryableStatusCodes;

    /// <summary>
    /// Gets or sets whether to retry on <see cref="HttpRequestException"/> (network failures). Default is true.
    /// </summary>
    public bool RetryOnHttpRequestException { get; init; } = true;

    /// <summary>
    /// Gets or sets whether to retry on <see cref="TaskCanceledException"/> caused by timeouts. Default is true.
    /// Note: This only applies to timeout-caused cancellations, not user-initiated cancellations.
    /// </summary>
    public bool RetryOnTimeout { get; init; } = true;

    /// <summary>
    /// Default retryable HTTP status codes: 5xx server errors and 408 Request Timeout.
    /// </summary>
    public static IReadOnlyCollection<HttpStatusCode> DefaultRetryableStatusCodes { get; } =
    [
        HttpStatusCode.RequestTimeout,           // 408
        HttpStatusCode.InternalServerError,      // 500
        HttpStatusCode.BadGateway,               // 502
        HttpStatusCode.ServiceUnavailable,       // 503
        HttpStatusCode.GatewayTimeout,           // 504
    ];

    /// <summary>
    /// Default resilience options with 3 retries and exponential backoff.
    /// </summary>
    public static HttpResilienceOptions Default { get; } = new();

    /// <summary>
    /// Disabled resilience options (no retries).
    /// </summary>
    public static HttpResilienceOptions None { get; } = new() { MaxRetryAttempts = 0 };

    /// <summary>
    /// Aggressive retry options with 5 retries and shorter delays.
    /// Suitable for critical operations that must succeed.
    /// </summary>
    public static HttpResilienceOptions Aggressive { get; } = new()
    {
        MaxRetryAttempts = 5,
        InitialDelay = TimeSpan.FromMilliseconds(500),
        MaxDelay = TimeSpan.FromSeconds(15),
    };

    /// <summary>
    /// Conservative retry options with 2 retries and longer delays.
    /// Suitable for non-critical operations where you want to be gentle on the server.
    /// </summary>
    public static HttpResilienceOptions Conservative { get; } = new()
    {
        MaxRetryAttempts = 2,
        InitialDelay = TimeSpan.FromSeconds(2),
        MaxDelay = TimeSpan.FromSeconds(60),
    };
}
