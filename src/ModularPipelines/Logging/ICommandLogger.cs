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
    /// <param name="options">The command line tool options used for execution.</param>
    /// <param name="inputToLog">The input command to log.</param>
    /// <param name="exitCode">The exit code returned by the command.</param>
    /// <param name="runTime">The time taken to execute the command.</param>
    /// <param name="standardOutput">The standard output from the command.</param>
    /// <param name="standardError">The standard error from the command.</param>
    /// <param name="commandWorkingDirPath">The working directory where the command was executed.</param>
    void Log(CommandLineToolOptions options,
        string? inputToLog,
        int? exitCode,
        TimeSpan? runTime,
        string standardOutput,
        string standardError, string commandWorkingDirPath);
}