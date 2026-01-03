using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Enums;
using Spectre.Console;

namespace ModularPipelines.Options;

/// <summary>
/// Configuration options for pipeline execution behavior.
/// </summary>
[ExcludeFromCodeCoverage]
public record PipelineOptions
{
    /// <summary>
    /// Gets or sets the execution mode that determines how the pipeline handles failures.
    /// </summary>
    public ExecutionMode ExecutionMode { get; set; } = ExecutionMode.StopOnFirstException;

    /// <summary>
    /// Gets or sets the collection of module categories to run exclusively. If specified, only modules in these categories will run.
    /// </summary>
    public ICollection<string>? RunOnlyCategories { get; set; }

    /// <summary>
    /// Gets or sets the collection of module categories to ignore during execution.
    /// </summary>
    public ICollection<string>? IgnoreCategories { get; set; }

    private bool _showProgressInConsole = AnsiConsole.Profile.Capabilities.Interactive;

    /// <summary>
    /// Gets or sets a value indicating whether to show progress information in the console.
    /// </summary>
    public bool ShowProgressInConsole
    {
        get => _showProgressInConsole;
        set => AnsiConsole.Profile.Capabilities.Interactive = _showProgressInConsole = value;
    }

    /// <summary>
    /// Gets or sets a value indicating whether to print execution results to the console.
    /// </summary>
    public bool PrintResults { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether to print the ModularPipelines logo.
    /// </summary>
    public bool PrintLogo { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether to print module dependency chains.
    /// </summary>
    public bool PrintDependencyChains { get; set; } = true;

    /// <summary>
    /// Gets or sets the default number of retry attempts for failed operations.
    /// </summary>
    public int DefaultRetryCount { get; set; }

    /// <summary>
    /// Gets or sets the default logging options for all commands.
    /// When set, this takes precedence over DefaultCommandLogging.
    /// </summary>
    public CommandLoggingOptions? DefaultLoggingOptions { get; set; }

    /// <summary>
    /// Gets or sets the default command logging level for all commands.
    /// </summary>
    [Obsolete("Use DefaultLoggingOptions instead. This property will be removed in a future version.")]
    public CommandLogging DefaultCommandLogging { get; set; } = CommandLogging.Default;

    /// <summary>
    /// Gets or sets the default HTTP logging options for all HTTP requests.
    /// Controls what parts of HTTP requests and responses are logged (headers, body, etc.).
    /// Use <see cref="HttpLoggingOptions.None"/> to disable all logging,
    /// <see cref="HttpLoggingOptions.Minimal"/> for URL/status only,
    /// <see cref="HttpLoggingOptions.Headers"/> for headers without body,
    /// or <see cref="HttpLoggingOptions.Full"/> for complete logging.
    /// </summary>
    public HttpLoggingOptions? DefaultHttpLoggingOptions { get; set; }

    /// <summary>
    /// Gets or sets the default timeout for all HTTP requests.
    /// When set, HTTP requests will be cancelled if they exceed this duration,
    /// unless overridden by <see cref="HttpOptions.Timeout"/> on individual requests.
    /// If not set (null), HTTP requests will use the default HttpClient timeout.
    /// </summary>
    public TimeSpan? DefaultHttpTimeout { get; set; }

    /// <summary>
    /// Gets or sets the default HTTP resilience options for all HTTP requests.
    /// Controls retry behavior for transient failures (network errors, 5xx server errors).
    /// Use <see cref="HttpResilienceOptions.None"/> to disable retries,
    /// <see cref="HttpResilienceOptions.Default"/> for standard retry behavior (3 retries with exponential backoff),
    /// <see cref="HttpResilienceOptions.Aggressive"/> for more retries with shorter delays,
    /// or <see cref="HttpResilienceOptions.Conservative"/> for fewer retries with longer delays.
    /// </summary>
    public HttpResilienceOptions? DefaultHttpResilienceOptions { get; set; }

    /// <summary>
    /// Gets or sets the concurrency options for module execution.
    /// Controls parallelism limits and resource-based throttling.
    /// </summary>
    public ConcurrencyOptions Concurrency { get; set; } = new();

    /// <summary>
    /// Gets or sets the default execution options for all commands.
    /// When set, these options apply to all command executions unless overridden per-call.
    /// </summary>
    public CommandExecutionOptions? DefaultExecutionOptions { get; set; }
}
