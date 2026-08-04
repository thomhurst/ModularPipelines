using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
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
/// <description>Settings in this <see cref="PipelineOptions"/> class, configured via <see cref="PipelineBuilder.Options"/></description>
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
/// <see cref="Configuration.ModuleConfigurationBuilder.WithRetry"/> configured will use its custom retry policy instead of
/// <see cref="DefaultRetryCount"/>. Modules without configuration fall back to global settings.
/// </para>
/// </remarks>
[ExcludeFromCodeCoverage]
public record PipelineOptions
{
    private IReadOnlyList<string>? _runOnlyCategories;
    private IReadOnlyList<string>? _ignoreCategories;
    private IReadOnlyList<string>? _targetModules;
    private IReadOnlyList<string>? _skippedModules;
    private CommandExecutionOptions? _defaultExecutionOptions;

    /// <summary>
    /// Gets machine-readable run report and local history settings.
    /// </summary>
    public RunReportOptions RunReport { get; init; } = new();

    /// <summary>
    /// Gets a value indicating whether running the pipeline should print a plan without executing modules.
    /// </summary>
    public bool DryRun { get; init; }

    /// <summary>
    /// Gets the execution mode that determines how the pipeline handles failures.
    /// </summary>
    public ExecutionMode ExecutionMode { get; init; } = ExecutionMode.StopOnFirstException;

    /// <summary>
    /// Gets the default timeout for modules that do not configure their own timeout.
    /// Set to <see cref="TimeSpan.Zero"/> to disable the default module timeout.
    /// </summary>
    public TimeSpan DefaultModuleTimeout { get; init; } = TimeSpan.FromMinutes(30);

    /// <summary>
    /// Gets the collection of module categories to run exclusively, matched case-insensitively.
    /// If specified, only modules in these categories will run.
    /// </summary>
    public IReadOnlyList<string>? RunOnlyCategories
    {
        get => _runOnlyCategories;
        init => _runOnlyCategories = value is null
            ? null
            : Array.AsReadOnly(value.ToArray());
    }

    /// <summary>
    /// Gets the collection of module categories to ignore during execution, matched case-insensitively.
    /// </summary>
    public IReadOnlyList<string>? IgnoreCategories
    {
        get => _ignoreCategories;
        init => _ignoreCategories = value is null
            ? null
            : Array.AsReadOnly(value.ToArray());
    }

    /// <summary>
    /// Gets module names to execute with their transitive dependency closures.
    /// Names may be simple, full, or assembly-qualified module type names.
    /// </summary>
    public IReadOnlyList<string>? TargetModules
    {
        get => _targetModules;
        init => _targetModules = value is null
            ? null
            : Array.AsReadOnly(value.ToArray());
    }

    /// <summary>
    /// Gets module names to exclude from execution.
    /// Names may be simple, full, or assembly-qualified module type names.
    /// </summary>
    public IReadOnlyList<string>? SkippedModules
    {
        get => _skippedModules;
        init => _skippedModules = value is null
            ? null
            : Array.AsReadOnly(value.ToArray());
    }

    /// <summary>
    /// Gets a value indicating whether to show progress information in the console.
    /// </summary>
    public bool ShowProgressInConsole { get; init; } = AnsiConsole.Profile.Capabilities.Interactive;

    /// <summary>
    /// Gets a value indicating whether to print execution results to the console.
    /// </summary>
    public bool PrintResults { get; init; } = true;

    /// <summary>
    /// Gets a value indicating whether to print the ModularPipelines logo.
    /// </summary>
    public bool PrintLogo { get; init; } = true;

    /// <summary>
    /// Gets a value indicating whether to print module dependency chains.
    /// </summary>
    public bool PrintDependencyChains { get; init; } = true;

    /// <summary>
    /// Gets a value indicating whether assemblies whose filenames contain
    /// <c>ModularPipeline</c> are eagerly loaded from the application directory.
    /// </summary>
    /// <remarks>
    /// Disabled by default. Enable this only when a plugin relies on module initializers
    /// instead of explicit assembly or service registration.
    /// </remarks>
    public bool LoadModularPipelineAssemblies { get; init; }

    /// <summary>
    /// Gets the default number of retry attempts for failed operations.
    /// </summary>
    /// <remarks>
    /// <para>
    /// <strong>Configuration Precedence:</strong>
    /// This is a global default that applies when a module does not have custom retries configured via <see cref="Configuration.ModuleConfigurationBuilder.WithRetry"/>.
    /// </para>
    /// <list type="bullet">
    /// <item>If a module has retries configured via <see cref="Configuration.ModuleConfigurationBuilder.WithRetry"/>, that takes precedence</item>
    /// <item>Otherwise, this global <see cref="DefaultRetryCount"/> is used</item>
    /// <item>If this value is 0 (default), no retries are attempted</item>
    /// </list>
    /// </remarks>
    public int DefaultRetryCount { get; init; }

    /// <summary>
    /// Gets the default logging options for all commands.
    /// </summary>
    /// <remarks>
    /// <para>
    /// <strong>Configuration Precedence (highest to lowest):</strong>
    /// </para>
    /// <list type="number">
    /// <item><see cref="CommandExecutionOptions.LogSettings"/> - Per-call override (highest priority)</item>
    /// <item>This <see cref="DefaultLoggingOptions"/> - Global default</item>
    /// <item>System defaults (lowest priority)</item>
    /// </list>
    /// </remarks>
    public CommandLoggingOptions? DefaultLoggingOptions { get; init; }

    /// <summary>
    /// Gets the default HTTP logging options for all HTTP requests.
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
    public HttpLoggingOptions? DefaultHttpLoggingOptions { get; init; }

    /// <summary>
    /// Gets the default timeout for all HTTP requests.
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
    public TimeSpan? DefaultHttpTimeout { get; init; }

    /// <summary>
    /// Gets the default HTTP resilience options for all HTTP requests.
    /// Controls retry behavior for transient failures (network errors, 5xx server errors).
    /// Use <see cref="HttpResilienceOptions.None"/> to disable retries,
    /// <see cref="HttpResilienceOptions.Default"/> for standard retry behavior (3 retries with exponential backoff),
    /// <see cref="HttpResilienceOptions.Aggressive"/> for more retries with shorter delays,
    /// or <see cref="HttpResilienceOptions.Conservative"/> for fewer retries with longer delays.
    /// </summary>
    public HttpResilienceOptions? DefaultHttpResilienceOptions { get; init; }

    /// <summary>
    /// Gets the concurrency options for module execution.
    /// Controls parallelism limits and resource-based throttling.
    /// </summary>
    public ConcurrencyOptions Concurrency { get; init; } = new();

    /// <summary>
    /// Gets the console width for output rendering.
    /// When set to a value, that width is used for all console output.
    /// When null (default), the width is auto-detected: 160 characters for known CI environments,
    /// or the terminal's detected width for local execution.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Spectre.Console defaults to 80 characters when it cannot detect the terminal width,
    /// which is common in CI environments where output is redirected. This can cause
    /// tables and other formatted output to wrap unnecessarily.
    /// </para>
    /// <para>
    /// Known CI environments (GitHub Actions, Azure Pipelines, TeamCity, GitLab, Jenkins,
    /// Bitbucket, Travis CI, AppVeyor) automatically use 160 characters unless overridden.
    /// </para>
    /// </remarks>
    public int? ConsoleWidth { get; init; }

    /// <summary>
    /// Gets how often buffered output from still-running modules is written to the console.
    /// Set to <see cref="TimeSpan.Zero"/> to disable time-based flushing.
    /// </summary>
    /// <remarks>
    /// Periodic flushing preserves diagnostic output when a process is killed before pipeline
    /// teardown can run. Size-triggered flushing remains controlled separately by
    /// <see cref="ModuleOutputFlushThreshold"/>. The default is one minute.
    /// </remarks>
    public TimeSpan ModuleOutputFlushInterval { get; init; } = TimeSpan.FromMinutes(1);

    /// <summary>
    /// Gets the number of buffered output entries that triggers an immediate incremental flush.
    /// Set to 0 to disable size-triggered flushing.
    /// </summary>
    /// <remarks>
    /// The default is 1,000 entries per module. This limits retained output between periodic flushes
    /// without changing the order or final completion status of module output. The threshold counts
    /// entries and does not impose a byte-size cap.
    /// </remarks>
    public int ModuleOutputFlushThreshold { get; init; } = 1_000;

    /// <summary>
    /// Gets the default execution options for all commands.
    /// When set, these options apply to all command executions unless overridden per-call.
    /// </summary>
    /// <remarks>
    /// <para>
    /// <strong>Configuration Precedence:</strong>
    /// Per-call options always take precedence over these global defaults.
    /// Properties set on per-call <see cref="CommandExecutionOptions"/> override matching properties here.
    /// </para>
    /// </remarks>
    public CommandExecutionOptions? DefaultExecutionOptions
    {
        get => _defaultExecutionOptions;
        init => _defaultExecutionOptions = value is null
            ? null
            : value with
            {
                EnvironmentVariables = value.EnvironmentVariables is null
                    ? null
                    : new ReadOnlyDictionary<string, string?>(
                        new Dictionary<string, string?>(value.EnvironmentVariables)),
            };
    }

    /// <summary>
    /// Gets a value indicating whether to throw a <see cref="Exceptions.PipelineFailedException"/>
    /// when the pipeline completes with one or more failed modules.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Default is <c>true</c> to ensure non-zero exit codes in CI/CD environments.
    /// When a pipeline fails, the exception is thrown after the summary has been printed,
    /// ensuring users see the full output before the process exits.
    /// </para>
    /// <para>
    /// Set to <c>false</c> for scenarios where you want to inspect the <see cref="Models.PipelineSummary"/>
    /// programmatically without catching exceptions (e.g., in tests or when implementing custom failure handling).
    /// </para>
    /// </remarks>
    public bool ThrowOnPipelineFailure { get; init; } = true;

    internal static TimeSpan MaximumModuleOutputFlushInterval { get; } =
        TimeSpan.FromMilliseconds(uint.MaxValue - 1);
}
