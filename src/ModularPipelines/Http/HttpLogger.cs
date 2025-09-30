using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.Logging;
using ModularPipelines.Helpers;
using ModularPipelines.Logging;

namespace ModularPipelines.Http;

/// <summary>
/// Logs HTTP Requests and Responses.
/// </summary>
internal class HttpLogger : IHttpLogger
{
    private readonly IHttpRequestFormatter _requestFormatter;
    private readonly IHttpResponseFormatter _responseFormatter;

    public HttpLogger(IHttpRequestFormatter requestFormatter, IHttpResponseFormatter responseFormatter)
    {
        _requestFormatter = requestFormatter;
        _responseFormatter = responseFormatter;
    }
    /// <summary>
    /// Prints the HTTP request.
    /// </summary>
    /// <param name="request">The HTTP request to print.</param>
    /// <param name="logger">The current module logger.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task PrintRequest(HttpRequestMessage request, IModuleLogger logger)
    {
        var formattedRequest = await _requestFormatter.FormatAsync(request);
        logger.LogInformation("HTTP Request:\n{Request}", formattedRequest);
    }

    /// <summary>
    /// Prints the HTTP response.
    /// </summary>
    /// <param name="response">The HTTP response to print.</param>
    /// <param name="logger">The current module logger.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task PrintResponse(HttpResponseMessage response, IModuleLogger logger)
    {
        var formattedResponse = await _responseFormatter.FormatAsync(response);
        logger.LogInformation("HTTP Response:\n{Response}", formattedResponse);
    }

    public void PrintStatusCode(HttpStatusCode? httpStatusCode, IModuleLogger logger)
    {
        var statusCode = httpStatusCode == null ? null as int? : (int) httpStatusCode;
        var icon = statusCode is >= 200 and < 300 ? "✓" : "✗";

        logger.LogInformation("{Icon} HTTP Status: {StatusCode} {HttpStatusCode}", icon, statusCode, httpStatusCode);
    }

    public void PrintDuration(TimeSpan duration, IModuleLogger logger)
    {
        logger.LogInformation("Duration: {Duration}", duration.ToDisplayString());
    }
}