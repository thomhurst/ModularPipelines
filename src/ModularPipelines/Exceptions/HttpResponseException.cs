using System.Net;

namespace ModularPipelines.Exceptions;

/// <summary>
/// Exception thrown when an HTTP request returns a non-success status code.
/// Provides detailed information about the failed response including status code, reason phrase, and response content.
/// </summary>
public class HttpResponseException : PipelineException
{
    private readonly Lazy<string> _formattedMessage;

    /// <summary>
    /// Gets the HTTP status code of the failed response.
    /// </summary>
    public HttpStatusCode StatusCode { get; }

    /// <summary>
    /// Gets the reason phrase from the failed response.
    /// </summary>
    public string? ReasonPhrase { get; }

    /// <summary>
    /// Gets the content of the failed response, if available.
    /// </summary>
    public string? ResponseContent { get; }

    /// <summary>
    /// Gets the request URI that produced the failed response.
    /// </summary>
    public Uri? RequestUri { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpResponseException"/> class.
    /// </summary>
    /// <param name="statusCode">The HTTP status code.</param>
    /// <param name="reasonPhrase">The reason phrase from the response.</param>
    /// <param name="responseContent">The content of the response.</param>
    /// <param name="requestUri">The request URI.</param>
    public HttpResponseException(HttpStatusCode statusCode, string? reasonPhrase, string? responseContent, Uri? requestUri)
        : base($"HTTP request failed with status code {(int)statusCode} ({statusCode})")
    {
        StatusCode = statusCode;
        ReasonPhrase = reasonPhrase;
        ResponseContent = responseContent;
        RequestUri = requestUri;
        _formattedMessage = new Lazy<string>(FormatMessage);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpResponseException"/> class with an inner exception.
    /// </summary>
    /// <param name="statusCode">The HTTP status code.</param>
    /// <param name="reasonPhrase">The reason phrase from the response.</param>
    /// <param name="responseContent">The content of the response.</param>
    /// <param name="requestUri">The request URI.</param>
    /// <param name="innerException">The inner exception.</param>
    public HttpResponseException(HttpStatusCode statusCode, string? reasonPhrase, string? responseContent, Uri? requestUri, Exception? innerException)
        : base($"HTTP request failed with status code {(int)statusCode} ({statusCode})", innerException)
    {
        StatusCode = statusCode;
        ReasonPhrase = reasonPhrase;
        ResponseContent = responseContent;
        RequestUri = requestUri;
        _formattedMessage = new Lazy<string>(FormatMessage);
    }

    /// <inheritdoc />
    public override string Message => _formattedMessage.Value;

    private string FormatMessage()
    {
        var message = $"HTTP request failed with status code {(int)StatusCode} ({StatusCode})";

        if (!string.IsNullOrWhiteSpace(ReasonPhrase))
        {
            message += $": {ReasonPhrase}";
        }

        if (RequestUri is not null)
        {
            message += $"\nRequest URI: {RequestUri}";
        }

        if (!string.IsNullOrWhiteSpace(ResponseContent))
        {
            // Truncate very long responses to avoid excessive exception messages
            const int maxContentLength = 2000;
            var truncatedContent = ResponseContent.Length > maxContentLength
                ? ResponseContent[..maxContentLength] + "... (truncated)"
                : ResponseContent;
            message += $"\nResponse content: {truncatedContent}";
        }

        return message;
    }
}
