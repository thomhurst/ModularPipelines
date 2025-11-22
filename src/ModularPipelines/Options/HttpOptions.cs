using ModularPipelines.Http;

namespace ModularPipelines.Options;

/// <summary>
/// Options for making HTTP requests.
/// </summary>
/// <param name="HttpRequestMessage">The HTTP request message to be sent.</param>
public record HttpOptions(HttpRequestMessage HttpRequestMessage)
{
    /// <summary>
    /// Gets or sets and sets an optional HttpClient for handling the request.
    /// </summary>
    public HttpClient? HttpClient { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to throw an exception if the request returns a bad HTTP status code.
    /// </summary>
    public bool ThrowOnNonSuccessStatusCode { get; set; }

    /// <summary>
    /// Gets and sets the logging for the HTTP request.
    /// </summary>
    public HttpLoggingType LoggingType { get; init; } = HttpLoggingType.Request | HttpLoggingType.Response | HttpLoggingType.StatusCode | HttpLoggingType.Duration;

    /// <summary>
    /// Implicitly converts an <see cref="HttpRequestMessage"/> to <see cref="HttpOptions"/>.
    /// </summary>
    /// <param name="requestMessage">The HTTP request message.</param>
    public static implicit operator HttpOptions(HttpRequestMessage requestMessage) => new(requestMessage);

    /// <summary>
    /// Implicitly converts a <see cref="Uri"/> to <see cref="HttpOptions"/> with a GET request.
    /// </summary>
    /// <param name="uri">The URI for the request.</param>
    public static implicit operator HttpOptions(Uri uri) => new HttpRequestMessage(HttpMethod.Get, uri);

    /// <summary>
    /// Implicitly converts a string URI to <see cref="HttpOptions"/> with a GET request.
    /// </summary>
    /// <param name="uri">The URI string for the request.</param>
    public static implicit operator HttpOptions(string uri) => new Uri(uri, UriKind.RelativeOrAbsolute);
}
