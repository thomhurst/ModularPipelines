using ModularPipelines.Enums;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace ModularPipelines.TestHelpers;

public record TestHostSettings
{
    /// <summary>
    /// Default timeout for test pipeline execution to prevent tests from hanging indefinitely.
    /// </summary>
    public static readonly TimeSpan DefaultTestTimeout = TimeSpan.FromSeconds(30);

    public CommandLogging CommandLogging { get; init; } = CommandLogging.Input | CommandLogging.Error;
    public LogLevel LogLevel { get; init; } = LogLevel.Debug;
    public bool ClearLogProviders { get; init; } = true;
    public bool ShowProgressInConsole { get; init; }

    /// <summary>
    /// Maximum time allowed for pipeline execution before the test is cancelled.
    /// Defaults to 30 seconds. Set to <see cref="Timeout.InfiniteTimeSpan"/> to disable.
    /// </summary>
    public TimeSpan TestTimeout { get; init; } = DefaultTestTimeout;
}