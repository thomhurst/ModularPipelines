namespace ModularPipelines.Options;

/// <summary>
/// Options for downloading content from a URI.
/// </summary>
/// <param name="DownloadUri">The URI to download from.</param>
public record DownloadOptions(Uri DownloadUri)
{
    /// <summary>
    /// Gets the HTTP client to use for the download.
    /// </summary>
    public HttpClient? HttpClient { get; init; }

    /// <summary>
    /// Gets an optional total timeout for the download, including response body reads.
    /// </summary>
    /// <remarks>
    /// When null, the pipeline or HTTP client timeout applies only until response headers arrive;
    /// body reads continue until completion or caller cancellation. Set a finite duration to limit
    /// the entire download, or <see cref="System.Threading.Timeout.InfiniteTimeSpan"/> for no total deadline.
    /// The HTTP client's own timeout still applies while waiting for response headers.
    /// </remarks>
    public TimeSpan? Timeout { get; init; }

    /// <summary>
    /// Gets an action to configure the HTTP request before sending.
    /// </summary>
    public Action<HttpRequestMessage>? RequestConfigurator { get; init; }

    /// <summary>
    /// Gets logging options for the download.
    /// </summary>
    public HttpLoggingOptions? Logging { get; init; }
}
