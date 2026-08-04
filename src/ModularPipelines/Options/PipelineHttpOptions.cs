using System.Diagnostics.CodeAnalysis;

namespace ModularPipelines.Options;

/// <summary>
/// Configures global defaults for pipeline HTTP requests.
/// </summary>
[ExcludeFromCodeCoverage]
public sealed record PipelineHttpOptions
{
    /// <summary>
    /// Gets the default logging options for all HTTP requests.
    /// Per-request <see cref="HttpOptions.LogSettings"/> takes precedence.
    /// </summary>
    public HttpLoggingOptions? Logging { get; init; }

    /// <summary>
    /// Gets the default timeout for all HTTP requests.
    /// Per-request <see cref="HttpOptions.Timeout"/> takes precedence.
    /// A null value uses the default <see cref="HttpClient"/> timeout.
    /// </summary>
    public TimeSpan? Timeout { get; init; }

    /// <summary>
    /// Gets the default resilience options for all HTTP requests.
    /// </summary>
    public HttpResilienceOptions? Resilience { get; init; }
}
