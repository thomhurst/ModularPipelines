using ModularPipelines.Constants;

namespace ModularPipelines.Options;

/// <summary>
/// Options for customizing HTTP request/response logging.
/// </summary>
/// <remarks>
/// <para>Set via <see cref="HttpOptions.Logging"/> or <see cref="PipelineHttpOptions.Logging"/>.</para>
/// <para>Controls what parts of HTTP requests and responses are logged:</para>
/// <list type="bullet">
/// <item><description><see cref="ShowRequest"/> - Log request method, URL, and version</description></item>
/// <item><description><see cref="ShowRequestHeaders"/> - Log request headers</description></item>
/// <item><description><see cref="ShowRequestBody"/> - Log request body content</description></item>
/// <item><description><see cref="ShowResponse"/> - Log response status and version</description></item>
/// <item><description><see cref="ShowResponseHeaders"/> - Log response headers</description></item>
/// <item><description><see cref="ShowResponseBody"/> - Log response body content</description></item>
/// <item><description><see cref="ShowStatusCode"/> - Log HTTP status code with icon</description></item>
/// <item><description><see cref="ShowDuration"/> - Log request duration</description></item>
/// </list>
/// <para>Binary content detection prevents logging of non-text content like file uploads.</para>
/// </remarks>
public record HttpLoggingOptions
{
    private IReadOnlyList<string> _sensitiveHeaderNames = DefaultSensitiveHeaders;

    /// <summary>
    /// Gets a value indicating whether to log request method, URL, and version. Default is true.
    /// </summary>
    public bool ShowRequest { get; init; } = true;

    /// <summary>
    /// Gets a value indicating whether to log request headers. Default is true.
    /// </summary>
    public bool ShowRequestHeaders { get; init; } = true;

    /// <summary>
    /// Gets a value indicating whether to log request body content. Default is true.
    /// Binary content is automatically skipped regardless of this setting.
    /// </summary>
    public bool ShowRequestBody { get; init; } = true;

    /// <summary>
    /// Gets a value indicating whether to log response status and version. Default is true.
    /// </summary>
    public bool ShowResponse { get; init; } = true;

    /// <summary>
    /// Gets a value indicating whether to log response headers. Default is true.
    /// </summary>
    public bool ShowResponseHeaders { get; init; } = true;

    /// <summary>
    /// Gets a value indicating whether to log response body content. Default is true.
    /// Binary content is automatically skipped regardless of this setting.
    /// </summary>
    public bool ShowResponseBody { get; init; } = true;

    /// <summary>
    /// Gets a value indicating whether to log HTTP status code with success/failure icon. Default is true.
    /// </summary>
    public bool ShowStatusCode { get; init; } = true;

    /// <summary>
    /// Gets a value indicating whether to log request duration. Default is true.
    /// </summary>
    public bool ShowDuration { get; init; } = true;

    /// <summary>
    /// Gets the maximum body size in bytes to read and log. Default is 4096.
    /// Bodies larger than this will be truncated with a message indicating the full size.
    /// Set to 0 or negative to disable truncation.
    /// </summary>
    public int MaxBodySizeToLog { get; init; } = LoggingConstants.DefaultMaxBodySizeToLog;

    /// <summary>
    /// Gets the list of header names that should have their values obfuscated in logs.
    /// Values are compared case-insensitively. Default includes common sensitive headers like
    /// Authorization, X-API-Key, Cookie, Set-Cookie, and various token/key headers.
    /// </summary>
    public IReadOnlyList<string> SensitiveHeaderNames
    {
        get => _sensitiveHeaderNames;
        init => _sensitiveHeaderNames = Array.AsReadOnly(value.ToArray());
    }

    /// <summary>
    /// Gets the default list of sensitive header names that should be obfuscated.
    /// </summary>
    public static IReadOnlyList<string> DefaultSensitiveHeaders { get; } = Array.AsReadOnly(
    [
        "Authorization",
        "X-API-Key",
        "X-Api-Key",
        "Api-Key",
        "ApiKey",
        "X-Auth-Token",
        "X-Access-Token",
        "X-Secret",
        "X-Secret-Key",
        "Cookie",
        "Set-Cookie",
        "WWW-Authenticate",
        "Proxy-Authorization",
        "Proxy-Authenticate",
        "X-CSRF-Token",
        "X-XSRF-Token",
        "X-Amz-Security-Token",
        "X-Amz-Credential",
    ]);

    /// <summary>
    /// Gets the default logging options (all logging enabled, 4KB body limit).
    /// </summary>
    public static HttpLoggingOptions Default { get; } = new();

    /// <summary>
    /// Gets the silent logging options (no HTTP logging).
    /// </summary>
    public static HttpLoggingOptions Silent { get; } = new()
    {
        ShowRequest = false,
        ShowRequestHeaders = false,
        ShowRequestBody = false,
        ShowResponse = false,
        ShowResponseHeaders = false,
        ShowResponseBody = false,
        ShowStatusCode = false,
        ShowDuration = false,
    };

    /// <summary>
    /// Gets the minimal logging options (URL, status code, and duration only).
    /// </summary>
    public static HttpLoggingOptions Minimal { get; } = new()
    {
        ShowRequest = true,
        ShowRequestHeaders = false,
        ShowRequestBody = false,
        ShowResponse = false,
        ShowResponseHeaders = false,
        ShowResponseBody = false,
        ShowStatusCode = true,
        ShowDuration = true,
    };

    /// <summary>
    /// Gets the headers-only logging options (URL, headers, status code, and duration - no body).
    /// </summary>
    public static HttpLoggingOptions Headers { get; } = new()
    {
        ShowRequest = true,
        ShowRequestHeaders = true,
        ShowRequestBody = false,
        ShowResponse = true,
        ShowResponseHeaders = true,
        ShowResponseBody = false,
        ShowStatusCode = true,
        ShowDuration = true,
    };

    /// <summary>
    /// Gets the full logging options (everything logged, 64KB body limit).
    /// </summary>
    public static HttpLoggingOptions Diagnostic { get; } = new()
    {
        ShowRequest = true,
        ShowRequestHeaders = true,
        ShowRequestBody = true,
        ShowResponse = true,
        ShowResponseHeaders = true,
        ShowResponseBody = true,
        ShowStatusCode = true,
        ShowDuration = true,
        MaxBodySizeToLog = LoggingConstants.FullLoggingMaxBodySize,
    };

    /// <summary>
    /// Gets the set of default sensitive headers used internally for O(1) lookup.
    /// </summary>
    internal static HashSet<string> DefaultSensitiveHeadersSet { get; } =
        new(DefaultSensitiveHeaders, StringComparer.OrdinalIgnoreCase);
}
