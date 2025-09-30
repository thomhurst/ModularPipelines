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
        logger.LogInformation("{Header}\n{Request}", MarkupFormatter.FormatColoredHeader("HTTP Request", "cyan"), formattedRequest);
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
        logger.LogInformation("{Header}\n{Response}", MarkupFormatter.FormatColoredHeader("HTTP Response", "green"), formattedResponse);
    }

    public void PrintStatusCode(HttpStatusCode? httpStatusCode, IModuleLogger logger)
    {
        var statusCode = httpStatusCode == null ? null as int? : (int) httpStatusCode;
        var statusColor = MarkupFormatter.GetHttpStatusColor(statusCode);

        logger.LogInformation("{Header} [{0}]{1} {2}[/]", MarkupFormatter.FormatHeader("HTTP Status"), statusColor, statusCode, httpStatusCode);
    }

    public void PrintDuration(TimeSpan duration, IModuleLogger logger)
    {
        logger.LogInformation(MarkupFormatter.FormatDuration(duration));
    }
}