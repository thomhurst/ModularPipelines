using ModularPipelines.Http;

namespace ModularPipelines.Options;

/// <summary>
/// Options for downloading content from a URI.
/// </summary>
/// <param name="DownloadUri">The URI to download from.</param>
public record DownloadOptions(Uri DownloadUri)
{
    /// <summary>
    /// Gets or sets the HTTP client to use for the download.
    /// </summary>
    public HttpClient? HttpClient { get; init; }

    /// <summary>
    /// Gets or sets an action to configure the HTTP request before sending.
    /// </summary>
    public Action<HttpRequestMessage>? RequestConfigurator { get; init; }

    /// <summary>
    /// Gets or sets the type of HTTP logging to perform during the download.
    /// </summary>
    public HttpLoggingType LoggingType { get; init; } = HttpLoggingType.Request | HttpLoggingType.Response | HttpLoggingType.StatusCode | HttpLoggingType.Duration;
}
