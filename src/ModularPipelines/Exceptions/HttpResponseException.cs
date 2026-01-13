using System.Net;

namespace ModularPipelines.Exceptions;

/// <summary>
/// Thrown when an HTTP request returns a non-success status code.
/// </summary>
/// <remarks>
/// <para>
/// This exception is thrown by HTTP client operations when the server returns a non-success
/// status code (4xx or 5xx). It provides detailed information about the failed response
/// for debugging and error handling purposes.
/// </para>
/// <para><b>When this is thrown:</b></para>
/// <list type="bullet">
/// <item>When an HTTP request returns a 4xx client error status code</item>
/// <item>When an HTTP request returns a 5xx server error status code</item>
/// <item>When using HTTP-based APIs that validate response status</item>
/// </list>
/// <para><b>Properties available:</b></para>
/// <list type="bullet">
/// <item><see cref="StatusCode"/> - The HTTP status code (e.g., 404, 500)</item>
/// <item><see cref="ReasonPhrase"/> - The reason phrase from the response</item>
/// <item><see cref="ResponseContent"/> - The body content of the failed response (may be truncated)</item>
/// <item><see cref="RequestUri"/> - The URI that was requested</item>
/// </list>
/// <para><b>Handling example:</b></para>
/// <code>
/// try
/// {
///     var response = await httpClient.GetAsync(url);
/// }
/// catch (HttpResponseException ex)
/// {
///     Console.WriteLine($"HTTP {(int)ex.StatusCode} {ex.StatusCode}");
///     Console.WriteLine($"URI: {ex.RequestUri}");
///     Console.WriteLine($"Response: {ex.ResponseContent}");
///
///     if (ex.StatusCode == HttpStatusCode.NotFound)
///     {
///         // Handle 404 specifically
///     }
/// }
/// </code>
/// </remarks>
/// <seealso cref="PipelineException"/>
public class HttpResponseException : PipelineException
{
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
        : base(FormatMessage(statusCode, reasonPhrase, responseContent, requestUri))
    {
        StatusCode = statusCode;
        ReasonPhrase = reasonPhrase;
        ResponseContent = responseContent;
        RequestUri = requestUri;
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
        : base(FormatMessage(statusCode, reasonPhrase, responseContent, requestUri), innerException)
    {
        StatusCode = statusCode;
        ReasonPhrase = reasonPhrase;
        ResponseContent = responseContent;
        RequestUri = requestUri;
    }

    private static string FormatMessage(HttpStatusCode statusCode, string? reasonPhrase, string? responseContent, Uri? requestUri)
    {
        var message = $"HTTP request failed with status code {(int)statusCode} ({statusCode})";

        if (!string.IsNullOrWhiteSpace(reasonPhrase))
        {
            message += $": {reasonPhrase}";
        }

        if (requestUri is not null)
        {
            message += $"\nRequest URI: {requestUri}";
        }

        if (!string.IsNullOrWhiteSpace(responseContent))
        {
            // Truncate very long responses to avoid excessive exception messages
            const int maxContentLength = 2000;
            var truncatedContent = responseContent.Length > maxContentLength
                ? responseContent[..maxContentLength] + "... (truncated)"
                : responseContent;
            message += $"\nResponse content: {truncatedContent}";
        }

        return message;
    }
}
