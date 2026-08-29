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
    /// Gets an action to configure the HTTP request before sending.
    /// </summary>
    public Action<HttpRequestMessage>? RequestConfigurator { get; init; }

    /// <summary>
    /// Gets logging options for the download.
    /// </summary>
    public HttpLoggingOptions? Logging { get; init; }
}
