using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Reporting;
using ModularPipelines.Secrets;

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
/// <description>Settings defined on individual modules via <see cref="ModuleConfiguration"/>
/// (e.g., Timeout, RetryCount, AlwaysRun)</description>
/// </item>
/// <item>
/// <term>Per-Call Configuration (highest priority)</term>
/// <description>Options passed to individual method calls (e.g., <see cref="CommandExecutionOptions.Logging"/>,
/// <see cref="HttpOptions.Logging"/>)</description>
/// </item>
/// </list>
/// <para>
/// <strong>Example:</strong>
/// If <see cref="PipelineCommandOptions.Logging"/> is set globally, it applies to all command executions.
/// However, if a specific command call passes <see cref="CommandExecutionOptions.Logging"/>,
/// that per-call setting takes precedence for that execution only.
/// </para>
/// <para>
/// <strong>Module Behaviors:</strong>
/// Module-level configuration uses <see cref="ModuleConfiguration"/>. A module with
/// <see cref="ModuleConfigurationBuilder.WithRetry(int, TimeSpan?, Func{Exception, bool}, TimeSpan?)"/> configured will use its custom retry configuration instead of
/// <see cref="DefaultRetryCount"/>. Modules without configuration fall back to global settings.
/// </para>
/// </remarks>
[ExcludeFromCodeCoverage]
public sealed record PipelineOptions
{
    private IReadOnlyList<string>? _runOnlyCategories;
    private IReadOnlyList<string>? _ignoreCategories;
    private IReadOnlyList<string>? _runOnlyTags;
    private IReadOnlyList<string>? _ignoreTags;
    private IReadOnlyList<string>? _runOnlyModules;
    private IReadOnlyList<string>? _ignoreModules;

    /// <summary>
    /// Gets machine-readable run report and local history settings.
    /// </summary>
    public RunReportOptions RunReport { get; init; } = new();

    /// <summary>
    /// Gets console rendering and output flushing settings.
    /// </summary>
    public PipelineConsoleOptions Console { get; init; } = new();

    /// <summary>
    /// Gets global HTTP request defaults.
    /// </summary>
    public PipelineHttpOptions Http { get; init; } = new();

    /// <summary>
    /// Gets global command execution and logging defaults.
    /// </summary>
    public PipelineCommandOptions Commands { get; init; } = new();

    /// <summary>
    /// Gets secret masking settings.
    /// </summary>
    public SecretMaskingOptions Secrets { get; init; } = new();

    /// <summary>
    /// Gets a value indicating whether running the pipeline should print a plan without executing modules.
    /// </summary>
    public bool DryRun { get; init; }

    /// <summary>
    /// Gets a value indicating whether module cache reads and writes are disabled for this run.
    /// </summary>
    public bool DisableModuleCache { get; init; }

    /// <summary>
    /// Gets a value indicating whether to rerun unsuccessful modules and their dependents,
    /// reusing compatible successful results from the previous run.
    /// </summary>
    public bool RerunFailed { get; init; }

    /// <summary>
    /// Gets how the pipeline responds to module failures.
    /// </summary>
    public FailureMode FailureMode { get; init; } = FailureMode.FailFast;

    /// <summary>
    /// Gets the default per-attempt timeout for modules that do not configure their own timeout.
    /// Set to <see cref="Timeout.InfiniteTimeSpan"/> to disable the default module timeout.
    /// Zero and other negative durations are invalid.
    /// Retry delays do not count toward this timeout.
    /// </summary>
    public TimeSpan DefaultModuleTimeout { get; init; } = TimeSpan.FromMinutes(30);

    /// <summary>
    /// Gets the maximum time to wait for a running module to finish after pipeline cancellation,
    /// before disposing its resources and allowing dependent AlwaysRun modules to execute.
    /// Defaults to 30 seconds. Set to zero to stop waiting immediately.
    /// Must be non-negative and no greater than 4,294,967,294 milliseconds.
    /// This does not change the one-second grace period after a module timeout.
    /// </summary>
    public TimeSpan ModuleCancellationGracePeriod { get; init; } = TimeSpan.FromSeconds(30);

    /// <summary>
    /// Gets the maximum cumulative time to wait for scheduler progress before retrying deferred
    /// <c>AlwaysRun</c> modules. Set to <see cref="Timeout.InfiniteTimeSpan"/> to disable this watchdog.
    /// Zero and other negative durations are invalid.
    /// </summary>
    public TimeSpan AlwaysRunProgressTimeout { get; init; } = TimeSpan.FromSeconds(30);

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
    /// Gets tags to run exclusively, matched case-insensitively against attribute and fluent tags.
    /// A module must match at least one tag. Null or empty lists do not restrict execution.
    /// Category and module filters still apply; required dependents of filtered modules are skipped.
    /// </summary>
    public IReadOnlyList<string>? RunOnlyTags
    {
        get => _runOnlyTags;
        init => _runOnlyTags = value is null
            ? null
            : Array.AsReadOnly(value.ToArray());
    }

    /// <summary>
    /// Gets tags to exclude, matched case-insensitively. A module matching any excluded tag is skipped,
    /// even when another tag matches <see cref="RunOnlyTags"/>. Null or empty lists exclude no modules.
    /// </summary>
    public IReadOnlyList<string>? IgnoreTags
    {
        get => _ignoreTags;
        init => _ignoreTags = value is null
            ? null
            : Array.AsReadOnly(value.ToArray());
    }

    /// <summary>
    /// Gets module names to execute with their transitive dependency closures.
    /// Names may be module identifiers (including <see cref="ModuleIdAttribute"/> overrides),
    /// simple, full, or assembly-qualified module type names, matched case-insensitively.
    /// A complete name is matched before interpreting it as a comma-separated list.
    /// Assembly-qualified type names take precedence over module identifiers with the same value.
    /// </summary>
    public IReadOnlyList<string>? RunOnlyModules
    {
        get => _runOnlyModules;
        init => _runOnlyModules = value is null
            ? null
            : Array.AsReadOnly(value.ToArray());
    }

    /// <summary>
    /// Gets module names to exclude from execution.
    /// Names may be module identifiers (including <see cref="ModuleIdAttribute"/> overrides),
    /// simple, full, or assembly-qualified module type names, matched case-insensitively.
    /// A complete name is matched before interpreting it as a comma-separated list.
    /// Assembly-qualified type names take precedence over module identifiers with the same value.
    /// </summary>
    public IReadOnlyList<string>? IgnoreModules
    {
        get => _ignoreModules;
        init => _ignoreModules = value is null
            ? null
            : Array.AsReadOnly(value.ToArray());
    }

    /// <summary>
    /// Gets the default number of retry attempts for failed operations.
    /// </summary>
    /// <remarks>
    /// <para>
    /// <strong>Configuration Precedence:</strong>
    /// This is a global default that applies when a module does not have custom retries configured via <see cref="ModuleConfigurationBuilder.WithRetry(int, TimeSpan?, Func{Exception, bool}, TimeSpan?)"/>.
    /// </para>
    /// <list type="bullet">
    /// <item>If a module has retries configured via <see cref="ModuleConfigurationBuilder.WithRetry(int, TimeSpan?, Func{Exception, bool}, TimeSpan?)"/>, that takes precedence</item>
    /// <item>Otherwise, this global <see cref="DefaultRetryCount"/> is used</item>
    /// <item>If this value is 0 (default), no retries are attempted</item>
    /// </list>
    /// </remarks>
    public int DefaultRetryCount { get; init; }

    /// <summary>
    /// Gets the concurrency options for module execution.
    /// Controls parallelism limits and resource-based throttling.
    /// </summary>
    public ConcurrencyOptions Concurrency { get; init; } = new();

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
    /// Set to <c>false</c> for scenarios where you want to inspect the <see cref="PipelineSummary"/>
    /// programmatically without catching exceptions (e.g., in tests or when implementing custom failure handling).
    /// To suppress recorded module exceptions, also set <see cref="FailureMode"/> to
    /// <see cref="FailureMode.ContinueOnFailure"/>, which waits for all modules to complete.
    /// With the default <see cref="FailureMode.FailFast"/>, any recorded exception is rethrown
    /// regardless of this setting. Failures reported without an exception respect this setting
    /// in either failure mode.
    /// </para>
    /// </remarks>
    public bool ThrowOnPipelineFailure { get; init; } = true;
}
