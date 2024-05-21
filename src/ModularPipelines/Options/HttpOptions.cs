using ModularPipelines.Http;

namespace ModularPipelines.Options;

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

    public static implicit operator HttpOptions(HttpRequestMessage requestMessage) => new(requestMessage);

    public static implicit operator HttpOptions(Uri uri) => new HttpRequestMessage(HttpMethod.Get, uri);

    public static implicit operator HttpOptions(string uri) => new Uri(uri, UriKind.RelativeOrAbsolute);
}