using System.Diagnostics.CodeAnalysis;
using Spectre.Console;

namespace ModularPipelines.Options;

/// <summary>
/// Configures pipeline console rendering and output flushing.
/// </summary>
[ExcludeFromCodeCoverage]
public sealed record PipelineConsoleOptions
{
    /// <summary>
    /// Gets a value indicating whether to show progress information in the console.
    /// </summary>
    public bool ShowProgress { get; init; } = AnsiConsole.Profile.Capabilities.Interactive;

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
    /// Gets the console width for output rendering.
    /// When set to a value, that width is used for all console output.
    /// When null (default), the width is auto-detected: 160 characters for known CI environments,
    /// or the terminal's detected width for local execution.
    /// </summary>
    /// <remarks>
    /// Spectre.Console defaults to 80 characters when it cannot detect the terminal width,
    /// which is common in CI environments where output is redirected. Known CI environments
    /// automatically use 160 characters unless overridden.
    /// </remarks>
    public int? Width { get; init; }

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
    /// without changing the order or final completion status of module output.
    /// </remarks>
    public int ModuleOutputFlushThreshold { get; init; } = 1_000;

    internal static TimeSpan MaximumModuleOutputFlushInterval { get; } =
        TimeSpan.FromMilliseconds(uint.MaxValue - 1);
}
