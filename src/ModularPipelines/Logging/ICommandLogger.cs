using ModularPipelines.Options;

namespace ModularPipelines.Logging;

/// <summary>
/// Provides functionality for logging command execution details.
/// </summary>
public interface ICommandLogger
{
    /// <summary>
    /// Logs the details of a command execution.
    /// </summary>
    /// <param name="options">The command line tool options used for execution. Logging behavior is controlled via <see cref="CommandLineToolOptions.LogSettings"/>.</param>
    /// <param name="loggingOptions">Optional logging options that override <see cref="CommandLineToolOptions.LogSettings"/>. Prefer using LogSettings on the options instead.</param>
    /// <param name="inputToLog">The input command to log.</param>
    /// <param name="exitCode">The exit code returned by the command.</param>
    /// <param name="runTime">The time taken to execute the command.</param>
    /// <param name="standardOutput">The standard output from the command.</param>
    /// <param name="standardError">The standard error from the command.</param>
    /// <param name="commandWorkingDirPath">The working directory where the command was executed.</param>
    void Log(
        CommandLineToolOptions options,
        CommandLoggingOptions? loggingOptions,
        string? inputToLog,
        int? exitCode,
        TimeSpan? runTime,
        string standardOutput,
        string standardError,
        string commandWorkingDirPath);
}