using System.Net.Http.Headers;
using System.Text;

namespace ModularPipelines.Http;

/// <summary>
/// Formats HTTP requests as strings.
/// </summary>
internal class HttpRequestFormatter : IHttpRequestFormatter
{
    public async Task<string> FormatAsync(HttpRequestMessage request)
    {
        var sb = new StringBuilder();

        sb.AppendLine($"{request.Method} {request.RequestUri} HTTP/{request.Version}");
        sb.AppendLine();

        AppendHeaders(sb, request.Headers, request.Content?.Headers);
        sb.AppendLine();

        await AppendBodyAsync(sb, request.Content);

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