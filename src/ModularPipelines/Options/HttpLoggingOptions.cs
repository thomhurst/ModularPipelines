using ModularPipelines.Constants;

namespace ModularPipelines.Options;

/// <summary>
/// Options for customizing HTTP request/response logging.
/// </summary>
/// <remarks>
/// <para>Set via <see cref="HttpOptions.LogSettings"/> or <see cref="PipelineOptions.DefaultHttpLoggingOptions"/>.</para>
/// <para>Controls what parts of HTTP requests and responses are logged:</para>
/// <list type="bullet">
/// <item><description><see cref="LogRequest"/> - Log request method, URL, and version</description></item>
/// <item><description><see cref="LogRequestHeaders"/> - Log request headers</description></item>
/// <item><description><see cref="LogRequestBody"/> - Log request body content</description></item>
/// <item><description><see cref="LogResponse"/> - Log response status and version</description></item>
/// <item><description><see cref="LogResponseHeaders"/> - Log response headers</description></item>
/// <item><description><see cref="LogResponseBody"/> - Log response body content</description></item>
/// <item><description><see cref="LogStatusCode"/> - Log HTTP status code with icon</description></item>
/// <item><description><see cref="LogDuration"/> - Log request duration</description></item>
/// </list>
/// <para>Binary content detection prevents logging of non-text content like file uploads.</para>
/// </remarks>
public record HttpLoggingOptions
{
    /// <summary>
    /// Gets or sets whether to log request method, URL, and version. Default is true.
    /// </summary>
    public bool LogRequest { get; init; } = true;

    /// <summary>
    /// Gets or sets whether to log request headers. Default is true.
    /// </summary>
    public bool LogRequestHeaders { get; init; } = true;

    /// <summary>
    /// Gets or sets whether to log request body content. Default is true.
    /// Binary content is automatically skipped regardless of this setting.
    /// </summary>
    public bool LogRequestBody { get; init; } = true;

    /// <summary>
    /// Gets or sets whether to log response status and version. Default is true.
    /// </summary>
    public bool LogResponse { get; init; } = true;

    /// <summary>
    /// Gets or sets whether to log response headers. Default is true.
    /// </summary>
    public bool LogResponseHeaders { get; init; } = true;

    /// <summary>
    /// Gets or sets whether to log response body content. Default is true.
    /// Binary content is automatically skipped regardless of this setting.
    /// </summary>
    public bool LogResponseBody { get; init; } = true;

    /// <summary>
    /// Gets or sets whether to log HTTP status code with success/failure icon. Default is true.
    /// </summary>
    public bool LogStatusCode { get; init; } = true;

    /// <summary>
    /// Gets or sets whether to log request duration. Default is true.
    /// </summary>
    public bool LogDuration { get; init; } = true;

    /// <summary>
    /// Gets or sets the maximum body size in characters to log. Default is 4096.
    /// Bodies larger than this will be truncated with a message indicating the full size.
    /// Set to 0 or negative to disable truncation.
    /// </summary>
    public int MaxBodySizeToLog { get; init; } = LoggingConstants.DefaultMaxBodySizeToLog;

    /// <summary>
    /// Gets or sets the list of header names that should have their values obfuscated in logs.
    /// Values are compared case-insensitively. Default includes common sensitive headers like
    /// Authorization, X-API-Key, Cookie, Set-Cookie, and various token/key headers.
    /// </summary>
    public IReadOnlyList<string> SensitiveHeaderNames { get; init; } = DefaultSensitiveHeaders;

    /// <summary>
    /// Default list of sensitive header names that should be obfuscated.
    /// </summary>
    public static IReadOnlyList<string> DefaultSensitiveHeaders { get; } = new[]
    {
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
    };

    /// <summary>
    /// HashSet of default sensitive headers for O(1) lookup.
    /// Used internally by formatters for performance.
    /// </summary>
    internal static HashSet<string> DefaultSensitiveHeadersSet { get; } =
        new(DefaultSensitiveHeaders, StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Default logging options (all logging enabled, 4KB body limit).
    /// </summary>
    public static HttpLoggingOptions Default { get; } = new();

    /// <summary>
    /// Silent logging options (no HTTP logging).
    /// </summary>
    public static HttpLoggingOptions None { get; } = new()
    {
        LogRequest = false,
        LogRequestHeaders = false,
        LogRequestBody = false,
        LogResponse = false,
        LogResponseHeaders = false,
        LogResponseBody = false,
        LogStatusCode = false,
        LogDuration = false,
    };

    /// <summary>
    /// Minimal logging options (URL, status code, and duration only).
    /// </summary>
    public static HttpLoggingOptions Minimal { get; } = new()
    {
        LogRequest = true,
        LogRequestHeaders = false,
        LogRequestBody = false,
        LogResponse = false,
        LogResponseHeaders = false,
        LogResponseBody = false,
        LogStatusCode = true,
        LogDuration = true,
    };

    /// <summary>
    /// Headers logging options (URL, headers, status code, and duration - no body).
    /// </summary>
    public static HttpLoggingOptions Headers { get; } = new()
    {
        LogRequest = true,
        LogRequestHeaders = true,
        LogRequestBody = false,
        LogResponse = true,
        LogResponseHeaders = true,
        LogResponseBody = false,
        LogStatusCode = true,
        LogDuration = true,
    };

    /// <summary>
    /// Full logging options (everything logged, 64KB body limit).
    /// </summary>
    public static HttpLoggingOptions Full { get; } = new()
    {
        LogRequest = true,
        LogRequestHeaders = true,
        LogRequestBody = true,
        LogResponse = true,
        LogResponseHeaders = true,
        LogResponseBody = true,
        LogStatusCode = true,
        LogDuration = true,
        MaxBodySizeToLog = LoggingConstants.FullLoggingMaxBodySize,
    };
}
