using ModularPipelines.Enums;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace ModularPipelines.TestHelpers;

/// <summary>
/// Configuration settings for test pipeline execution.
/// Used by <see cref="TestBase"/> and <see cref="TestPipelineHostBuilder"/> to configure
/// the test environment for module testing.
/// </summary>
/// <example>
/// <code>
/// // Using default settings
/// var result = await RunModule&lt;MyModule&gt;();
///
/// // With custom settings
/// var result = await RunModule&lt;MyModule&gt;(new TestHostSettings
/// {
///     LogLevel = LogLevel.Information,
///     TestTimeout = TimeSpan.FromMinutes(2)
/// });
/// </code>
/// </example>
public record TestHostSettings
{
    /// <summary>
    /// Default timeout for test pipeline execution to prevent tests from hanging indefinitely.
    /// </summary>
    public static readonly TimeSpan DefaultTestTimeout = TimeSpan.FromSeconds(30);

    /// <summary>
    /// Specifies which command execution events should be logged.
    /// Defaults to logging command input and errors.
    /// </summary>
    public CommandLogging CommandLogging { get; init; } = CommandLogging.Input | CommandLogging.Error;

    /// <summary>
    /// The minimum log level for test output.
    /// Defaults to <see cref="LogLevel.Debug"/> for comprehensive test logging.
    /// </summary>
    public LogLevel LogLevel { get; init; } = LogLevel.Debug;

    /// <summary>
    /// Whether to clear all default log providers before adding test-specific ones.
    /// Defaults to <c>true</c> to ensure clean test output.
    /// </summary>
    public bool ClearLogProviders { get; init; } = true;

    /// <summary>
    /// Whether to display pipeline progress in the console during test execution.
    /// Defaults to <c>false</c> for quieter test output.
    /// </summary>
    public bool ShowProgressInConsole { get; init; }

    /// <summary>
    /// Maximum time allowed for pipeline execution before the test is cancelled.
    /// Defaults to 30 seconds. Set to <see cref="Timeout.InfiniteTimeSpan"/> to disable.
    /// </summary>
    public TimeSpan TestTimeout { get; init; } = DefaultTestTimeout;
}