using System.Diagnostics;
using ModularPipelines.Context.Domains.Shell;
using ModularPipelines.Logging;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Context;

/// <summary>
/// Decorator that adds logging to command execution.
/// </summary>
/// <remarks>
/// <para>
/// Wraps an inner <see cref="ICommandLineExecutor"/> and delegates logging to
/// <see cref="ICommandLogger"/> for consistent command output across the framework.
/// </para>
/// <para>
/// <b>Consolidation Note:</b> This class delegates to <see cref="ICommandLogger"/> to avoid
/// duplicating logging logic. The same logging infrastructure is used by both
/// <see cref="ICommandLineExecutor"/> (for CommandLine-based execution) and
/// <see cref="ICommandContext"/> (for CommandLineToolOptions-based execution).
/// </para>
/// </remarks>
internal sealed class LoggingCommandLineExecutor : ICommandLineExecutor
{
    private readonly ICommandLineExecutor _inner;
    private readonly ICommandLogger _commandLogger;

    /// <summary>
    /// Initialises a new instance of the <see cref="LoggingCommandLineExecutor"/> class.
    /// </summary>
    /// <param name="inner">The inner executor to wrap.</param>
    /// <param name="commandLogger">The command logger for consistent logging.</param>
    public LoggingCommandLineExecutor(
        ICommandLineExecutor inner,
        ICommandLogger commandLogger)
    {
        _inner = inner;
        _commandLogger = commandLogger;
    }

    /// <inheritdoc />
    public async Task<CommandResult> ExecuteAsync(
        CommandLine commandLine,
        CommandExecutionOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        var workingDirectory = options?.WorkingDirectory ?? Environment.CurrentDirectory;
        var inputToLog = GetInputToLog(commandLine, options);
        var stopwatch = Stopwatch.StartNew();
        var loggingFailures = new DeferredCommandLoggingFailures();
        loggingFailures.Capture(
            () => _commandLogger.LogCommandStart(
                options: null,
                execOpts: options,
                inputToLog: inputToLog,
                commandWorkingDirPath: workingDirectory));

        CommandResult result;
        try
        {
            result = await _inner.ExecuteAsync(commandLine, options, cancellationToken).ConfigureAwait(false);
        }
        catch (Exception executionFailure)
        {
            loggingFailures.Capture(
                () => LogCompletion(
                    options,
                    inputToLog,
                    workingDirectory,
                    exitCode: -1,
                    runTime: stopwatch.Elapsed,
                    standardOutput: string.Empty,
                    standardError: executionFailure.Message));

            if (!loggingFailures.HasFailures)
            {
                throw;
            }

            throw loggingFailures.CombineWith(executionFailure);
        }

        loggingFailures.Capture(
            () => LogCompletion(
                options,
                inputToLog,
                workingDirectory,
                result.ExitCode,
                result.Duration,
                result.StandardOutput,
                result.StandardError));
        loggingFailures.Throw();
        return result;
    }

    private static string GetInputToLog(CommandLine commandLine, CommandExecutionOptions? options)
    {
        var input = commandLine.ToString();
        return options?.InputLoggingManipulator is not null
            ? options.InputLoggingManipulator(input)
            : input;
    }

    private void LogCompletion(
        CommandExecutionOptions? options,
        string inputToLog,
        string workingDirectory,
        int exitCode,
        TimeSpan runTime,
        string standardOutput,
        string standardError)
    {
        _commandLogger.LogCommandCompletion(
            options: null,
            execOpts: options,
            inputToLog: inputToLog,
            exitCode: exitCode,
            runTime: runTime,
            standardOutput: standardOutput,
            standardError: standardError,
            commandWorkingDirPath: workingDirectory);
    }
}
