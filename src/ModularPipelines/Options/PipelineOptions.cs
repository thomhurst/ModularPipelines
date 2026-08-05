using System.Diagnostics.CodeAnalysis;

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
/// If <see cref="PipelineCommandOptions.Logging"/> is set globally, it applies to all command executions.
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
    /// Gets a value indicating whether running the pipeline should print a plan without executing modules.
    /// </summary>
    public bool DryRun { get; init; }

    /// <summary>
    /// Gets a value indicating whether module cache reads and writes are disabled for this run.
    /// </summary>
    public bool DisableModuleCache { get; init; }

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
    /// Gets the maximum cumulative time to wait for scheduler progress before retrying deferred
    /// <c>AlwaysRun</c> modules. Set to <see cref="TimeSpan.Zero"/> to disable this watchdog.
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
    /// Set to <c>false</c> for scenarios where you want to inspect the <see cref="Models.PipelineSummary"/>
    /// programmatically without catching exceptions (e.g., in tests or when implementing custom failure handling).
    /// </para>
    /// </remarks>
    public bool ThrowOnPipelineFailure { get; init; } = true;
}
