using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Constants;
using ModularPipelines.Engine;
using ModularPipelines.Helpers;
using ModularPipelines.Options;

namespace ModularPipelines.Logging;

internal class CommandLogger : ICommandLogger, ICommandOutputLogger
{
    internal const int MaximumInlineOutputLength = 100;

    private readonly IModuleLoggerProvider _moduleLoggerProvider;
    private readonly IOptions<PipelineOptions> _pipelineOptions;
    private readonly ISecretObfuscator _secretObfuscator;

    public CommandLogger(IModuleLoggerProvider moduleLoggerProvider,
        IOptions<PipelineOptions> pipelineOptions,
        ISecretObfuscator secretObfuscator)
    {
        _moduleLoggerProvider = moduleLoggerProvider;
        _pipelineOptions = pipelineOptions;
        _secretObfuscator = secretObfuscator;
    }

    private ILogger Logger => _moduleLoggerProvider.GetLogger();

    public void Log(
        CommandLineToolOptions? options,
        CommandExecutionOptions? execOpts,
        string? inputToLog,
        int? exitCode,
        TimeSpan? runTime,
        string standardOutput,
        string standardError,
        string commandWorkingDirPath)
    {
        // Determine effective logging options
        var effectiveOptions = GetEffectiveLoggingOptions(options, execOpts);

        // Silent = no logging at all
        if (effectiveOptions.Verbosity == CommandLogVerbosity.Silent)
        {
            return;
        }

        if (execOpts?.InternalDryRun == true)
        {
            LogDryRunCommand(effectiveOptions, commandWorkingDirPath, inputToLog);
            return;
        }

        // Use compact logging format for cleaner output
        LogCompact(effectiveOptions, execOpts, commandWorkingDirPath, inputToLog, exitCode, runTime, standardOutput, standardError);
    }

    void ICommandOutputLogger.LogStandardOutputLine(
        CommandLineToolOptions options,
        CommandExecutionOptions executionOptions,
        string line)
    {
        LogOutputLine(options, executionOptions, line, isError: false);
    }

    void ICommandOutputLogger.LogStandardErrorLine(
        CommandLineToolOptions options,
        CommandExecutionOptions executionOptions,
        string line)
    {
        LogOutputLine(options, executionOptions, line, isError: true);
    }

    private CommandLoggingOptions GetEffectiveLoggingOptions(CommandLineToolOptions? options, CommandExecutionOptions? execOpts)
    {
        // Priority: execOpts property > pipeline default > system default
        if (execOpts?.LogSettings is not null)
        {
            return execOpts.LogSettings;
        }

        return _pipelineOptions.Value.DefaultLoggingOptions ?? CommandLoggingOptions.Default;
    }

    private void LogDryRunCommand(CommandLoggingOptions options, string workingDirectory, string? input)
    {
        if (!ShouldShowInput(options))
        {
            return;
        }

        Logger.LogInformation("{WorkingDirectory}> {Input} [DRY-RUN]",
            workingDirectory,
            input);
    }

    private void LogOutputLine(
        CommandLineToolOptions options,
        CommandExecutionOptions executionOptions,
        string line,
        bool isError)
    {
        var effectiveOptions = GetEffectiveLoggingOptions(options, executionOptions);
        var shouldLog = effectiveOptions.Verbosity >= CommandLogVerbosity.Normal
                        && (isError
                            ? effectiveOptions.ShowStandardError
                            : effectiveOptions.ShowStandardOutput);
        if (!shouldLog)
        {
            return;
        }

        var obfuscatedOutput = _secretObfuscator.Obfuscate(line, null);
        Logger.LogInformation(
            isError ? "  ↳ {CommandError}" : "  ↳ {CommandOutput}",
            obfuscatedOutput);
    }

    private void LogCompact(
        CommandLoggingOptions options,
        CommandExecutionOptions? execOpts,
        string workingDirectory,
        string? input,
        int? exitCode,
        TimeSpan? runTime,
        string standardOutput,
        string standardError)
    {
        var isSuccess = exitCode == 0;
        var obfuscatedInput = ShouldShowInput(options)
            ? _secretObfuscator.Obfuscate(input, null)
            : LoggingConstants.CommandMask;

        var commandMessage = new StringBuilder();
        commandMessage.Append(workingDirectory);
        commandMessage.Append("> ");
        commandMessage.Append(obfuscatedInput);

        var standardOutputToLog = execOpts?.OutputLoggingManipulator is not null
            ? execOpts.OutputLoggingManipulator(standardOutput)
            : standardOutput;
        var standardErrorToLog = execOpts?.OutputLoggingManipulator is not null
            ? execOpts.OutputLoggingManipulator(standardError)
            : standardError;

        var trimmedOutput = standardOutputToLog.Trim();
        var hasShortOutput = ShouldInlineOutput(options, trimmedOutput);
        var hasInlineOutput = hasShortOutput && isSuccess;
        var inlineOutput = hasInlineOutput
            ? $" → {_secretObfuscator.Obfuscate(trimmedOutput, null)}"
            : string.Empty;
        var commandStatus = BuildCommandStatus(options, isSuccess, exitCode, runTime);

        Logger.LogInformation(
            "{CommandMessage}{CommandOutput}{CommandStatus}",
            commandMessage.ToString(),
            inlineOutput,
            commandStatus);

        LogCapturedOutput(options, trimmedOutput, hasInlineOutput);
        LogCapturedError(options, standardErrorToLog, exitCode);
    }

    private static bool ShouldInlineOutput(CommandLoggingOptions options, string output)
    {
        return !string.IsNullOrEmpty(output)
               && !output.Contains('\n')
               && output.Length <= MaximumInlineOutputLength
               && options.Verbosity >= CommandLogVerbosity.Normal
               && options.ShowStandardOutput;
    }

    private static string BuildCommandStatus(
        CommandLoggingOptions options,
        bool isSuccess,
        int? exitCode,
        TimeSpan? runTime)
    {
        var showExecutionTime = options.Verbosity >= CommandLogVerbosity.Detailed || options.ShowExecutionTime;
        var showExitCode = options.Verbosity >= CommandLogVerbosity.Detailed || options.ShowExitCode;
        if (!showExecutionTime && !showExitCode)
        {
            return !isSuccess
                   && options.Verbosity >= CommandLogVerbosity.Normal
                   && options.ShowStandardError
                ? " ✗"
                : string.Empty;
        }

        var status = new StringBuilder()
            .Append(' ')
            .Append(isSuccess ? '✓' : '✗');

        if (showExecutionTime)
        {
            status.Append(" [").Append(runTime?.ToDisplayString() ?? "?");
        }

        if (showExitCode)
        {
            status.Append(showExecutionTime ? ", " : " [").Append("exit ").Append(exitCode);
        }

        return status.Append(']').ToString();
    }

    private void LogCapturedOutput(
        CommandLoggingOptions options,
        string output,
        bool wasInlined)
    {
        if (wasInlined
            || string.IsNullOrWhiteSpace(output)
            || options.Verbosity < CommandLogVerbosity.Normal
            || !options.ShowStandardOutput)
        {
            return;
        }

        Logger.LogInformation("  ↳ {CommandOutput}", _secretObfuscator.Obfuscate(output, null));
    }

    private void LogCapturedError(
        CommandLoggingOptions options,
        string error,
        int? exitCode)
    {
        if (string.IsNullOrWhiteSpace(error)
            || options.Verbosity < CommandLogVerbosity.Normal
            || !options.ShowStandardError
            || exitCode == 0)
        {
            return;
        }

        Logger.LogWarning("  ✗ {CommandError}", _secretObfuscator.Obfuscate(error, null));
    }

    private static bool ShouldShowInput(CommandLoggingOptions options)
    {
        // ShowCommandArguments controls whether to show full command or obfuscated
        return options.ShowCommandArguments;
    }
}
