using System.Net.Http.Headers;
using System.Text;
using ModularPipelines.Engine;
using ModularPipelines.Options;

namespace ModularPipelines.Http;

/// <summary>
/// Formats HTTP responses as strings with headers and body content.
/// Produces human-readable output suitable for logging and debugging.
/// Sensitive values are automatically obfuscated using the secret obfuscator.
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
    private readonly ISecretObfuscator _secretObfuscator;

    public HttpResponseFormatter(ISecretObfuscator secretObfuscator)
    {
        _secretObfuscator = secretObfuscator;
    }

    private static readonly HashSet<string> TextMediaTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        "application/json",
        "application/xml",
        "application/x-www-form-urlencoded",
        "text/plain",
        "text/html",
        "text/xml",
        "text/css",
        "text/javascript",
        "application/javascript",
    };

    public Task<string> FormatAsync(HttpResponseMessage response, CancellationToken cancellationToken = default)
    {
        return FormatAsync(response, HttpLoggingOptions.Default, cancellationToken);
    }

    public async Task<string> FormatAsync(HttpResponseMessage response, HttpLoggingOptions options, CancellationToken cancellationToken = default)
    {
        var sb = new StringBuilder();

        sb.AppendLine($"HTTP/{response.Version} {response.ReasonPhrase}");
        sb.AppendLine();

        if (options.LogResponseHeaders)
        {
            AppendHeaders(sb, response.Headers, response.Content.Headers);
            sb.AppendLine();
        }

        if (options.LogResponseBody)
        {
            await AppendBodyAsync(sb, response.Content, options.MaxBodySizeToLog, cancellationToken).ConfigureAwait(false);
        }

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

    private async Task AppendBodyAsync(StringBuilder sb, HttpContent? content, int maxBodySize, CancellationToken cancellationToken)
    {
        sb.AppendLine("Body");

        if (content == null)
        {
            sb.AppendLine("\t(null)");
            return;
        }

        // Check if content is binary (stream content without text media type)
        if (IsBinaryContent(content))
        {
            var contentLength = content.Headers.ContentLength;
            var contentType = content.Headers.ContentType?.MediaType ?? "unknown";
            sb.AppendLine($"\t[Binary content: {contentType}, {FormatSize(contentLength)}]");
            return;
        }

        var body = await content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

        if (string.IsNullOrWhiteSpace(body))
        {
            sb.AppendLine("\t(empty)");
            return;
        }

        // Obfuscate sensitive values in the body
        var obfuscatedBody = _secretObfuscator.Obfuscate(body, null);

        // Truncate if body is too large
        if (maxBodySize > 0 && obfuscatedBody.Length > maxBodySize)
        {
            sb.AppendLine($"\t{obfuscatedBody[..maxBodySize]}");
            sb.AppendLine($"\t... [truncated, total size: {obfuscatedBody.Length:N0} characters]");
        }
        else
        {
            sb.AppendLine($"\t{obfuscatedBody}");
        }
    }

    private static bool IsBinaryContent(HttpContent content)
    {
        var contentType = content.Headers.ContentType?.MediaType;

        // If no content type, check if it's a stream content
        if (string.IsNullOrEmpty(contentType))
        {
            return content is StreamContent or ByteArrayContent;
        }

        // Check for known text types
        if (TextMediaTypes.Contains(contentType))
        {
            return false;
        }

        // Check if it starts with "text/"
        if (contentType.StartsWith("text/", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        // Check for common text-based application types
        if (contentType.StartsWith("application/", StringComparison.OrdinalIgnoreCase))
        {
            // Types ending in +json or +xml are text
            if (contentType.EndsWith("+json", StringComparison.OrdinalIgnoreCase) ||
                contentType.EndsWith("+xml", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
        }

        // Everything else is considered binary
        return true;
    }

    private static string FormatSize(long? bytes)
    {
        if (bytes == null)
        {
            return "unknown size";
        }

        return bytes switch
        {
            < 1024 => $"{bytes} bytes",
            < 1024 * 1024 => $"{bytes / 1024.0:F1} KB",
            _ => $"{bytes / (1024.0 * 1024.0):F1} MB",
        };
    }
}
