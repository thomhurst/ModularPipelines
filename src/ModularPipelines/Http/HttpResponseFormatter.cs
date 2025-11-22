using System.Net.Http.Headers;
using System.Text;

namespace ModularPipelines.Http;

/// <summary>
/// Formats HTTP responses as strings with headers and body content.
/// Produces human-readable output suitable for logging and debugging.
/// </summary>
/// <example>
/// <code>
/// // Example output:
/// // HTTP/2.0 OK
/// //
/// // Headers
/// //     Content-Type: application/json
/// //     Content-Length: 1234
/// //
/// // Body
/// //     {"users": [...]}
///
/// var formatter = new HttpResponseFormatter();
/// var formatted = await formatter.FormatAsync(httpResponse);
/// logger.LogInformation(formatted);
/// </code>
/// </example>
internal class HttpResponseFormatter : IHttpResponseFormatter
{
    public async Task<string> FormatAsync(HttpResponseMessage response)
    {
        var sb = new StringBuilder();

        sb.AppendLine($"HTTP/{response.Version} {response.ReasonPhrase}");
        sb.AppendLine();

        AppendHeaders(sb, response.Headers, response.Content.Headers);
        sb.AppendLine();

        await AppendBodyAsync(sb, response.Content);

        return sb.ToString();
    }

    private static void AppendHeaders(StringBuilder sb, HttpHeaders baseHeaders, HttpHeaders? contentHeaders)
    {
        sb.AppendLine("Headers");

        foreach (var (key, values) in baseHeaders)
        {
            foreach (var value in values)
            {
                sb.AppendLine($"\t{key}: {value}");
            }
        }

        var contentHeadersArray = contentHeaders as IEnumerable<KeyValuePair<string, IEnumerable<string>>>
                                  ?? Array.Empty<KeyValuePair<string, IEnumerable<string>>>();

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

    private static async Task AppendBodyAsync(StringBuilder sb, HttpContent? content)
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
