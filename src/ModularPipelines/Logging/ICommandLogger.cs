using ModularPipelines.Options;
using ModularPipelines.Secrets;

namespace ModularPipelines.Logging;

/// <summary>
/// Provides functionality for logging command execution details.
/// </summary>
/// <remarks>
/// <para>
/// <b>Thread Safety:</b> Implementations must be thread-safe as logging may occur
/// from multiple modules executing in parallel.
/// </para>
/// <para>
/// <b>Secret Obfuscation:</b> All logged content is automatically obfuscated using
/// <see cref="ModularPipelines.Secrets.ISecretObfuscator"/> before being written to the logger.
/// </para>
/// </remarks>
internal interface ICommandLogger
{
    void LogOutputDrainTimeout(CommandLineToolOptions? options, CommandExecutionOptions execOpts);

    void LogOutputTruncation(CommandLineToolOptions? options, CommandExecutionOptions? execOpts, long standardOutputCharacters, long standardErrorCharacters, int maximumLength);

    /// <summary>
    /// Logs a command immediately before execution starts.
    /// </summary>
    /// <param name="options">The command line tool options used for execution. Can be null for raw command line execution.</param>
    /// <param name="execOpts">The command execution options containing logging settings.</param>
    /// <param name="inputToLog">The input command to log.</param>
    /// <param name="commandWorkingDirPath">The working directory where the command will execute.</param>
    void LogCommandStart(
        CommandLineToolOptions? options,
        CommandExecutionOptions? execOpts,
        string? inputToLog,
        string commandWorkingDirPath)
    {
    }

    /// <summary>
    /// Logs command output and status after execution finishes.
    /// </summary>
    /// <param name="options">The command line tool options used for execution. Can be null for raw command line execution.</param>
    /// <param name="execOpts">The command execution options containing logging settings.</param>
    /// <param name="inputToLog">The input command to log.</param>
    /// <param name="exitCode">The exit code returned by the command.</param>
    /// <param name="runTime">The time taken to execute the command.</param>
    /// <param name="standardOutput">The standard output from the command.</param>
    /// <param name="standardError">The standard error from the command.</param>
    /// <param name="commandWorkingDirPath">The working directory where the command executed.</param>
    void LogCommandCompletion(
        CommandLineToolOptions? options,
        CommandExecutionOptions? execOpts,
        string? inputToLog,
        int? exitCode,
        TimeSpan? runTime,
        string standardOutput,
        string standardError,
        string commandWorkingDirPath)
    {
        Log(
            options,
            execOpts,
            inputToLog,
            exitCode,
            runTime,
            standardOutput,
            standardError,
            commandWorkingDirPath);
    }

    /// <summary>
    /// Logs the details of a completed command execution.
    /// </summary>
    /// <param name="options">The command line tool options used for execution. Can be null for raw command line execution.</param>
    /// <param name="execOpts">The command execution options containing logging settings. Logging behavior is controlled via <see cref="CommandExecutionOptions.Logging"/>.</param>
    /// <param name="inputToLog">The input command to log.</param>
    /// <param name="exitCode">The exit code returned by the command.</param>
    /// <param name="runTime">The time taken to execute the command.</param>
    /// <param name="standardOutput">The standard output from the command.</param>
    /// <param name="standardError">The standard error from the command.</param>
    /// <param name="commandWorkingDirPath">The working directory where the command was executed.</param>
    void Log(
        CommandLineToolOptions? options,
        CommandExecutionOptions? execOpts,
        string? inputToLog,
        int? exitCode,
        TimeSpan? runTime,
        string standardOutput,
        string standardError,
        string commandWorkingDirPath);

    /// <summary>
    /// Logs that the framework is about to terminate a command process or one of its descendants.
    /// </summary>
    /// <remarks>
    /// Terminations are always logged, regardless of command logging verbosity, so that a killed
    /// process can be traced back to the command that owned it.
    /// </remarks>
    /// <param name="target">The process being terminated.</param>
    /// <param name="reason">Why the process is being terminated.</param>
    /// <param name="method">How the process is being terminated.</param>
    void LogProcessTermination(
        ProcessTerminationTarget target,
        ProcessTerminationReason reason,
        ProcessTerminationMethod method);

    /// <summary>
    /// Logs that the operating system rejected a process termination.
    /// </summary>
    /// <param name="target">The process that could not be terminated.</param>
    /// <param name="exception">The error the operating system reported.</param>
    void LogProcessTerminationFailure(ProcessTerminationTarget target, Exception exception);
}
