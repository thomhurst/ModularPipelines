using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ModularPipelines.Helpers;
using ModularPipelines.Logging;

namespace ModularPipelines.Http;

/// <summary>
/// Logs HTTP Requests and Responses.
/// </summary>
public static class HttpLogger
{
    /// <summary>
    /// Prints the HTTP request.
    /// </summary>
    /// <param name="request">The HTTP request to print.</param>
    /// <param name="logger">The current module logger.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public static async Task PrintRequest(HttpRequestMessage request, IModuleLogger logger)
    {
        var sb = new StringBuilder();

        sb.AppendLine($"{request.Method} {request.RequestUri} HTTP/{request.Version}");

        sb.AppendLine();

        PrintHeaders(sb, request.Headers, request.Content?.Headers);

        sb.AppendLine();

        await PrintBody(sb, request.Content);

        logger.LogInformation("---Request---\r\n{Request}", sb.ToString());
    }

    /// <summary>
    /// Prints the HTTP response.
    /// </summary>
    /// <param name="response">The HTTP response to print.</param>
    /// <param name="logger">The current module logger.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public static async Task PrintResponse(HttpResponseMessage response, IModuleLogger logger)
    {
        var sb = new StringBuilder();

        sb.AppendLine($"HTTP/{response.Version} {response.ReasonPhrase}");

        sb.AppendLine();

        PrintHeaders(sb, response.Headers, response.Content.Headers);

        sb.AppendLine();

        await PrintBody(sb, response.Content);

        logger.LogInformation("---Response---\r\n{Response}", sb.ToString());
    }
    
    public static void PrintStatusCode(HttpStatusCode? httpStatusCode, IModuleLogger logger)
    {
        var statusCode = httpStatusCode == null ? null as int? : (int)httpStatusCode;

        logger.LogInformation("---HTTP Status Code---\r\n{IntegerStatusCode} {StatusCode}", statusCode, httpStatusCode);
    }

    public static void PrintDuration(TimeSpan duration, IModuleLogger logger)
    {
        logger.LogInformation("---Duration---\r\n{Duration}", duration.ToDisplayString());
    }

    private static void PrintHeaders(StringBuilder sb, HttpHeaders baseHeaders, HttpHeaders? contentHeaders)
    {
        sb.AppendLine("Headers");
        foreach (var (key, values) in baseHeaders)
        {
            foreach (var value in values)
            {
                sb.AppendLine($"\t{key}: {value}");
            }
        }

        var contentHeadersArray = contentHeaders as IEnumerable<KeyValuePair<string, IEnumerable<string>>> ?? Array.Empty<KeyValuePair<string, IEnumerable<string>>>();

        foreach (var (key, values) in contentHeadersArray)
        {
            foreach (var value in values)
            {
                sb.AppendLine($"\t{key}: {value}");
            }
        }

        if (!baseHeaders.Any() && (!contentHeaders?.Any() ?? true))
        {
            sb.AppendLine("\t(null)");
        }
    }

    private static async Task PrintBody(StringBuilder sb, HttpContent? content)
    {
        sb.AppendLine("Body");
        var body = await (content?.ReadAsStringAsync() ?? Task.FromResult(string.Empty));
        if (!string.IsNullOrWhiteSpace(body))
        {
            sb.AppendLine($"\t{body}");
        }
        else
        {
            sb.AppendLine("\t(null)");
        }
    }
}