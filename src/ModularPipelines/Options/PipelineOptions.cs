using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Enums;
using Spectre.Console;

namespace ModularPipelines.Options;

/// <summary>
/// Configuration options for pipeline execution behavior.
/// </summary>
/// <remarks>
/// <para>
/// <strong>Configuration Precedence:</strong>
/// ModularPipelines uses a layered configuration system where more specific settings override more general ones.
/// The precedence order from lowest to highest priority is:
/// </para>
/// <list type="number">
/// <item>
/// <term>System Defaults (lowest priority)</term>
/// <description>Built-in defaults in the framework code (e.g., 30-minute module timeout, no retries)</description>
/// </item>
/// <item>
/// <term>Global Configuration</term>
/// <description>Settings in this <see cref="PipelineOptions"/> class, configured via <see cref="Host.PipelineHostBuilder.ConfigurePipelineOptions"/></description>
/// </item>
/// <item>
/// <term>Module Configuration</term>
/// <description>Settings defined on individual modules via <see cref="Configuration.ModuleConfiguration"/>
/// (e.g., Timeout, RetryCount, AlwaysRun)</description>
/// </item>
/// <item>
/// <term>Per-Call Configuration (highest priority)</term>
/// <description>Options passed to individual method calls (e.g., <see cref="CommandExecutionOptions.LogSettings"/>,
/// <see cref="HttpOptions.LogSettings"/>)</description>
/// </item>
/// </list>
/// <para>
/// <strong>Example:</strong>
/// If <see cref="DefaultLoggingOptions"/> is set globally, it applies to all command executions.
/// However, if a specific command call passes <see cref="CommandExecutionOptions.LogSettings"/>,
/// that per-call setting takes precedence for that execution only.
/// </para>
/// <para>
/// <strong>Module Behaviors:</strong>
/// Module-level configuration uses <see cref="Configuration.ModuleConfiguration"/>. A module with
/// <see cref="Configuration.ModuleConfigurationBuilder.WithRetryCount"/> configured will use its custom retry policy instead of
/// <see cref="DefaultRetryCount"/>. Modules without configuration fall back to global settings.
/// </para>
/// </remarks>
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
    /// <remarks>
    /// <para>
    /// <strong>Configuration Precedence:</strong>
    /// This is a global default that applies when a module does not have a custom retry policy configured via <see cref="Configuration.ModuleConfigurationBuilder.WithRetryCount"/>.
    /// </para>
    /// <list type="bullet">
    /// <item>If a module has a retry policy configured via <see cref="Configuration.ModuleConfigurationBuilder.WithRetryCount"/>, that takes precedence</item>
    /// <item>Otherwise, this global <see cref="DefaultRetryCount"/> is used</item>
    /// <item>If this value is 0 (default), no retries are attempted</item>
    /// </list>
    /// </remarks>
    public int DefaultRetryCount { get; set; }

    /// <summary>
    /// Gets or sets the default logging options for all commands.
    /// </summary>
    /// <remarks>
    /// <para>
    /// <strong>Configuration Precedence (highest to lowest):</strong>
    /// </para>
    /// <list type="number">
    /// <item><see cref="CommandExecutionOptions.LogSettings"/> - Per-call override (highest priority)</item>
    /// <item>This <see cref="DefaultLoggingOptions"/> - Global default</item>
    /// <item><see cref="DefaultCommandLogging"/> - Legacy enum-based fallback (lowest priority)</item>
    /// </list>
    /// </remarks>
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
    /// <remarks>
    /// <para>
    /// <strong>Configuration Precedence (highest to lowest):</strong>
    /// </para>
    /// <list type="number">
    /// <item><see cref="HttpOptions.LogSettings"/> - Per-request override (highest priority)</item>
    /// <item>This <see cref="DefaultHttpLoggingOptions"/> - Global default</item>
    /// <item><see cref="HttpLoggingOptions.Default"/> - System default (lowest priority)</item>
    /// </list>
    /// </remarks>
    public HttpLoggingOptions? DefaultHttpLoggingOptions { get; set; }

    /// <summary>
    /// Gets or sets the default timeout for all HTTP requests.
    /// When set, HTTP requests will be cancelled if they exceed this duration,
    /// unless overridden by <see cref="HttpOptions.Timeout"/> on individual requests.
    /// If not set (null), HTTP requests will use the default HttpClient timeout.
    /// </summary>
    /// <remarks>
    /// <para>
    /// <strong>Configuration Precedence (highest to lowest):</strong>
    /// </para>
    /// <list type="number">
    /// <item><see cref="HttpOptions.Timeout"/> - Per-request override (highest priority)</item>
    /// <item>This <see cref="DefaultHttpTimeout"/> - Global default</item>
    /// <item>HttpClient default timeout - System default (lowest priority)</item>
    /// </list>
    /// </remarks>
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
    /// <remarks>
    /// <para>
    /// <strong>Configuration Precedence:</strong>
    /// Per-call options always take precedence over these global defaults.
    /// Properties set on per-call <see cref="CommandExecutionOptions"/> override matching properties here.
    /// </para>
    /// </remarks>
    public CommandExecutionOptions? DefaultExecutionOptions { get; set; }
}
