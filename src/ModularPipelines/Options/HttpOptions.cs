using ModularPipelines.Http;

namespace ModularPipelines.Options;

/// <summary>
/// Options for making HTTP requests.
/// </summary>
/// <param name="HttpRequestMessage">The HTTP request message to be sent.</param>
public record HttpOptions(HttpRequestMessage HttpRequestMessage)
{
    /// <summary>
    /// Gets or sets an optional HttpClient for handling the request.
    /// </summary>
    public HttpClient? HttpClient { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to throw an exception if the request returns a bad HTTP status code.
    /// </summary>
    public bool ThrowOnNonSuccessStatusCode { get; set; }

    /// <summary>
    /// Gets or sets the logging for the HTTP request.
    /// Controls which handlers are used (Request, Response, StatusCode, Duration).
    /// For fine-grained control over what gets logged within those handlers, use <see cref="LogSettings"/>.
    /// </summary>
    public HttpLoggingType LoggingType { get; init; } = HttpLoggingType.Request | HttpLoggingType.Response | HttpLoggingType.StatusCode | HttpLoggingType.Duration;

    /// <summary>
    /// Gets or sets detailed logging options controlling what parts of requests/responses are logged.
    /// This controls the verbosity of the logging, including headers, body content, and truncation limits.
    /// Use <see cref="HttpLoggingOptions.None"/> to disable all logging, <see cref="HttpLoggingOptions.Minimal"/> for URL/status only,
    /// <see cref="HttpLoggingOptions.Headers"/> for headers without body, or <see cref="HttpLoggingOptions.Full"/> for complete logging.
    /// </summary>
    public HttpLoggingOptions? LogSettings { get; init; }

    /// <summary>
    /// Gets or sets the maximum time allowed for the HTTP request to complete.
    /// </summary>
    /// <remarks>
    /// <para>When set, the request will be cancelled if it exceeds this duration.</para>
    /// <para>If the request does not complete within the timeout, a <see cref="System.OperationCanceledException"/> or
    /// <see cref="System.Threading.Tasks.TaskCanceledException"/> will be thrown.</para>
    /// <para>If not set (null), the request will use the default HttpClient timeout or run until completion
    /// or until the passed cancellation token is cancelled.</para>
    /// </remarks>
    public TimeSpan? Timeout { get; init; }

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
