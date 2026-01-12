using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Engine;
using ModularPipelines.Helpers;
using ModularPipelines.Logging;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Context;

/// <summary>
/// Decorator that adds logging to command execution.
/// </summary>
/// <remarks>
/// Wraps an inner <see cref="ICommandLineExecutor"/> and logs command input,
/// output, and execution details based on <see cref="CommandLoggingOptions"/>.
/// </remarks>
internal sealed class LoggingCommandLineExecutor : ICommandLineExecutor
{
    private readonly ICommandLineExecutor _inner;
    private readonly IModuleLoggerProvider _moduleLoggerProvider;
    private readonly IOptions<PipelineOptions> _pipelineOptions;
    private readonly ISecretObfuscator _secretObfuscator;

    /// <summary>
    /// Initializes a new instance of the <see cref="LoggingCommandLineExecutor"/> class.
    /// </summary>
    /// <param name="inner">The inner executor to wrap.</param>
    /// <param name="moduleLoggerProvider">Provider for module loggers.</param>
    /// <param name="pipelineOptions">Pipeline options containing default logging settings.</param>
    /// <param name="secretObfuscator">Obfuscator for sensitive information.</param>
    public LoggingCommandLineExecutor(
        ICommandLineExecutor inner,
        IModuleLoggerProvider moduleLoggerProvider,
        IOptions<PipelineOptions> pipelineOptions,
        ISecretObfuscator secretObfuscator)
    {
        _inner = inner;
        _moduleLoggerProvider = moduleLoggerProvider;
        _pipelineOptions = pipelineOptions;
        _secretObfuscator = secretObfuscator;
    }

    private ILogger Logger => _moduleLoggerProvider.GetLogger();

    /// <inheritdoc />
    public async Task<CommandResult> ExecuteAsync(
        CommandLine commandLine,
        CommandExecutionOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        var loggingOptions = GetEffectiveLoggingOptions(options);

        // Silent = no logging at all
        if (loggingOptions.Verbosity == CommandLogVerbosity.Silent)
        {
            return await _inner.ExecuteAsync(commandLine, options, cancellationToken).ConfigureAwait(false);
        }

        var workingDirectory = options?.WorkingDirectory ?? Environment.CurrentDirectory;
        var inputToLog = GetInputToLog(commandLine, options);

        LogCommandInput(loggingOptions, options, workingDirectory, inputToLog);

        var result = await _inner.ExecuteAsync(commandLine, options, cancellationToken).ConfigureAwait(false);

        LogExitCode(loggingOptions, result.ExitCode);
        LogDuration(loggingOptions, result.Duration);
        LogOutput(loggingOptions, options, result.StandardOutput);
        LogError(loggingOptions, options, result.ExitCode, result.StandardError);
        LogWorkingDirectory(loggingOptions, workingDirectory);

        return result;
    }

    private CommandLoggingOptions GetEffectiveLoggingOptions(CommandExecutionOptions? options)
    {
        // Priority: options.LogSettings > pipeline default > system default
        if (options?.LogSettings is not null)
        {
            return options.LogSettings;
        }

        return _pipelineOptions.Value.DefaultLoggingOptions ?? CommandLoggingOptions.Default;
    }

    private string GetInputToLog(CommandLine commandLine, CommandExecutionOptions? options)
    {
        var input = commandLine.ToString();
        return options?.InputLoggingManipulator is not null
            ? options.InputLoggingManipulator(input)
            : input;
    }

    private void LogCommandInput(
        CommandLoggingOptions loggingOptions,
        CommandExecutionOptions? options,
        string workingDirectory,
        string inputToLog)
    {
        // Minimal and above shows command input
        if (loggingOptions.Verbosity < CommandLogVerbosity.Minimal)
        {
            return;
        }

        var timestamp = GetTimestampPrefix(loggingOptions);

        if (loggingOptions.ShowCommandArguments)
        {
            Logger.LogInformation(
                "{Timestamp}Command: {WorkingDirectory}> {Input}",
                timestamp,
                workingDirectory,
                _secretObfuscator.Obfuscate(inputToLog, options));
        }
        else
        {
            Logger.LogInformation(
                "{Timestamp}Command: {WorkingDirectory}> [arguments hidden]",
                timestamp,
                workingDirectory);
        }
    }

    private void LogExitCode(CommandLoggingOptions loggingOptions, int exitCode)
    {
        // Detailed and above shows exit code, or if explicitly requested
        if (loggingOptions.Verbosity < CommandLogVerbosity.Detailed && !loggingOptions.ShowExitCode)
        {
            return;
        }

        var timestamp = GetTimestampPrefix(loggingOptions);
        var icon = exitCode == 0 ? "+" : "x";
        Logger.LogInformation("{Timestamp}{Icon} Exit Code: {ExitCode}", timestamp, icon, exitCode);
    }

    private void LogDuration(CommandLoggingOptions loggingOptions, TimeSpan duration)
    {
        // Detailed and above shows duration, or if explicitly requested
        if (loggingOptions.Verbosity < CommandLogVerbosity.Detailed && !loggingOptions.ShowExecutionTime)
        {
            return;
        }

        var timestamp = GetTimestampPrefix(loggingOptions);
        Logger.LogInformation("{Timestamp}Duration: {Duration}", timestamp, duration.ToDisplayString());
    }

    private void LogOutput(
        CommandLoggingOptions loggingOptions,
        CommandExecutionOptions? options,
        string standardOutput)
    {
        if (string.IsNullOrWhiteSpace(standardOutput))
        {
            return;
        }

        // Verbosity >= Normal shows output by default; ShowStandardOutput can disable it
        if (loggingOptions.Verbosity < CommandLogVerbosity.Normal || !loggingOptions.ShowStandardOutput)
        {
            return;
        }

        var timestamp = GetTimestampPrefix(loggingOptions);
        var outputToLog = options?.OutputLoggingManipulator is not null
            ? options.OutputLoggingManipulator(standardOutput)
            : standardOutput;
        Logger.LogInformation("{Timestamp}Output:\n{Output}", timestamp, _secretObfuscator.Obfuscate(outputToLog, options));
    }

    private void LogError(
        CommandLoggingOptions loggingOptions,
        CommandExecutionOptions? options,
        int exitCode,
        string standardError)
    {
        if (string.IsNullOrWhiteSpace(standardError))
        {
            return;
        }

        // Verbosity >= Normal shows error on failure by default; ShowStandardError can disable it
        var showDueToVerbosity = loggingOptions.Verbosity >= CommandLogVerbosity.Normal && exitCode != 0;
        if (!showDueToVerbosity || !loggingOptions.ShowStandardError)
        {
            return;
        }

        var timestamp = GetTimestampPrefix(loggingOptions);
        var errorToLog = options?.OutputLoggingManipulator is not null
            ? options.OutputLoggingManipulator(standardError)
            : standardError;
        Logger.LogInformation("{Timestamp}x Error:\n{Error}", timestamp, _secretObfuscator.Obfuscate(errorToLog, options));
    }

    private void LogWorkingDirectory(CommandLoggingOptions loggingOptions, string workingDirectory)
    {
        // Diagnostic shows working directory, or if explicitly requested
        if (loggingOptions.Verbosity < CommandLogVerbosity.Diagnostic && !loggingOptions.ShowWorkingDirectory)
        {
            return;
        }

        var timestamp = GetTimestampPrefix(loggingOptions);
        Logger.LogInformation("{Timestamp}Working Directory: {WorkingDirectory}", timestamp, workingDirectory);
    }

    private static string GetTimestampPrefix(CommandLoggingOptions loggingOptions)
    {
        // Diagnostic automatically includes timestamps, or if explicitly requested
        if (loggingOptions.Verbosity < CommandLogVerbosity.Diagnostic && !loggingOptions.IncludeTimestamps)
        {
            return string.Empty;
        }

        return $"[{DateTime.UtcNow:HH:mm:ss.fff}] ";
    }
}
