using System.Net.Http.Headers;
using System.Text;
using ModularPipelines.Constants;
using ModularPipelines.Engine;
using ModularPipelines.Options;

namespace ModularPipelines.Http;

/// <summary>
/// Formats HTTP requests as strings with headers and body content.
/// Produces human-readable output suitable for logging and debugging.
/// Sensitive values are automatically obfuscated using the secret obfuscator.
/// </summary>
/// <example>
/// <code>
/// // Example output:
/// // GET https://api.example.com/users HTTP/2.0
/// //
/// // Headers
/// //     Accept: application/json
/// //     User-Agent: ModularPipelines/1.0
/// //
/// // Body
/// //     (null)
///
/// var formatter = new HttpRequestFormatter();
/// var formatted = await formatter.FormatAsync(httpRequest);
/// logger.LogInformation(formatted);
/// </code>
/// </example>
internal class HttpRequestFormatter : IHttpRequestFormatter
{
    private readonly ISecretObfuscator _secretObfuscator;

    public HttpRequestFormatter(ISecretObfuscator secretObfuscator)
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

    public Task<string> FormatAsync(HttpRequestMessage request, CancellationToken cancellationToken = default)
    {
        return FormatAsync(request, HttpLoggingOptions.Default, cancellationToken);
    }

    public async Task<string> FormatAsync(HttpRequestMessage request, HttpLoggingOptions options, CancellationToken cancellationToken = default)
    {
        var sb = new StringBuilder();

        sb.AppendLine($"{request.Method} {request.RequestUri} HTTP/{request.Version}");
        sb.AppendLine();

        if (options.LogRequestHeaders)
        {
            AppendHeaders(sb, request.Headers, request.Content?.Headers, options.SensitiveHeaderNames);
            sb.AppendLine();
        }

        if (options.LogRequestBody)
        {
            await AppendBodyAsync(sb, request.Content, options.MaxBodySizeToLog, cancellationToken).ConfigureAwait(false);
        }

        return sb.ToString();
    }

    private static void AppendHeaders(StringBuilder sb, HttpHeaders baseHeaders, HttpHeaders? contentHeaders, IReadOnlyList<string> sensitiveHeaderNames)
    {
        sb.AppendLine("Headers");

        foreach (var (key, values) in baseHeaders)
        {
            foreach (var value in values)
            {
                var displayValue = IsSensitiveHeader(key, sensitiveHeaderNames) ? LoggingConstants.SecretMask : value;
                sb.AppendLine($"\t{key}: {displayValue}");
            }
        }

        var contentHeadersArray = contentHeaders as IEnumerable<KeyValuePair<string, IEnumerable<string>>>
                                  ?? Array.Empty<KeyValuePair<string, IEnumerable<string>>>();

        foreach (var (key, values) in contentHeadersArray)
        {
            foreach (var value in values)
            {
                var displayValue = IsSensitiveHeader(key, sensitiveHeaderNames) ? LoggingConstants.SecretMask : value;
                sb.AppendLine($"\t{key}: {displayValue}");
            }
        }

        if (!baseHeaders.Any() && (!contentHeaders?.Any() ?? true))
        {
            sb.AppendLine("\t(null)");
        }
    }

    private static bool IsSensitiveHeader(string headerName, IReadOnlyList<string> sensitiveHeaderNames)
    {
        // Use HashSet for O(1) lookup when using default headers
        if (ReferenceEquals(sensitiveHeaderNames, HttpLoggingOptions.DefaultSensitiveHeaders))
        {
            return HttpLoggingOptions.DefaultSensitiveHeadersSet.Contains(headerName);
        }

        // For custom lists, fall back to case-insensitive comparison
        foreach (var sensitiveHeader in sensitiveHeaderNames)
        {
            if (string.Equals(headerName, sensitiveHeader, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        return false;
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
            return content is StreamContent or ByteArrayContent or MultipartContent;
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
