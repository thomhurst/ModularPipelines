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
/// <see cref="ICommand"/> (for CommandLineToolOptions-based execution).
/// </para>
/// </remarks>
internal sealed class LoggingCommandLineExecutor : ICommandLineExecutor
{
    private readonly ICommandLineExecutor _inner;
    private readonly ICommandLogger _commandLogger;

    /// <summary>
    /// Initializes a new instance of the <see cref="LoggingCommandLineExecutor"/> class.
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

        var result = await _inner.ExecuteAsync(commandLine, options, cancellationToken).ConfigureAwait(false);

        // Delegate to CommandLogger for consistent logging across all command execution paths
        _commandLogger.Log(
            options: null, // Raw command line execution doesn't use CommandLineToolOptions
            execOpts: options,
            inputToLog: inputToLog,
            exitCode: result.ExitCode,
            runTime: result.Duration,
            standardOutput: result.StandardOutput,
            standardError: result.StandardError,
            commandWorkingDirPath: workingDirectory);

        return result;
    }

    private static string GetInputToLog(CommandLine commandLine, CommandExecutionOptions? options)
    {
        var input = commandLine.ToString();
        return options?.InputLoggingManipulator is not null
            ? options.InputLoggingManipulator(input)
            : input;
    }
}
