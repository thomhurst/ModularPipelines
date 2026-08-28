using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Constants;
using ModularPipelines.Engine;
using ModularPipelines.Helpers;
using ModularPipelines.Options;
using ModularPipelines.Secrets;

namespace ModularPipelines.Logging;

internal class CommandLogger : ICommandLogger, ICommandOutputLogger
{
    internal const int MaximumInlineOutputLength = 100;

    private readonly IModuleLoggerAccessor _moduleLoggerAccessor;
    private readonly IOptions<PipelineOptions> _pipelineOptions;
    private readonly ISecretObfuscator _secretObfuscator;

    public CommandLogger(IModuleLoggerAccessor moduleLoggerAccessor,
        IOptions<PipelineOptions> pipelineOptions,
        ISecretObfuscator secretObfuscator)
    {
        _moduleLoggerAccessor = moduleLoggerAccessor;
        _pipelineOptions = pipelineOptions;
        _secretObfuscator = secretObfuscator;
    }

    private ILogger Logger => _moduleLoggerAccessor.Logger;

    public void LogCommandStart(
        CommandLineToolOptions? options,
        CommandExecutionOptions? execOpts,
        string? inputToLog,
        string commandWorkingDirPath)
    {
        var effectiveOptions = GetEffectiveLoggingOptions(options, execOpts);
        if (effectiveOptions.Verbosity == CommandLogVerbosity.Silent)
        {
            return;
        }

        if (execOpts?.InternalDryRun == true)
        {
            LogDryRunCommand(effectiveOptions, commandWorkingDirPath, inputToLog);
            return;
        }

        var obfuscatedInput = ShouldShowInput(effectiveOptions)
            ? ObfuscateLogValue(inputToLog)
            : new PreObfuscatedLogValue(LoggingConstants.CommandMask);
        Logger.LogInformation(
            "{WorkingDirectory}> {Input}",
            commandWorkingDirPath,
            obfuscatedInput);
    }

    public void LogCommandCompletion(
        CommandLineToolOptions? options,
        CommandExecutionOptions? execOpts,
        string? inputToLog,
        int? exitCode,
        TimeSpan? runTime,
        string standardOutput,
        string standardError,
        string commandWorkingDirPath)
    {
        var effectiveOptions = GetEffectiveLoggingOptions(options, execOpts);
        if (effectiveOptions.Verbosity == CommandLogVerbosity.Silent
            || execOpts?.InternalDryRun == true)
        {
            return;
        }

        var (outputToLog, errorToLog) = ManipulateOutput(
            execOpts?.OutputLoggingManipulator,
            standardOutput,
            standardError);
        var isSuccess = exitCode == 0;

        LogCapturedOutput(effectiveOptions, outputToLog.Trim(), isSuccess);
        LogCapturedError(effectiveOptions, errorToLog, exitCode);
        LogCommandStatus(effectiveOptions, inputToLog, isSuccess, exitCode, runTime);
    }

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
        LogCommandStart(options, execOpts, inputToLog, commandWorkingDirPath);
        LogCommandCompletion(
            options,
            execOpts,
            inputToLog,
            exitCode,
            runTime,
            standardOutput,
            standardError,
            commandWorkingDirPath);
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
        if (execOpts?.Logging is not null)
        {
            return execOpts.Logging;
        }

        return _pipelineOptions.Value.Commands.Logging ?? CommandLoggingOptions.Default;
    }

    private void LogDryRunCommand(CommandLoggingOptions options, string workingDirectory, string? input)
    {
        var logger = Logger;
        if (!ShouldShowInput(options) || !logger.IsEnabled(LogLevel.Information))
        {
            return;
        }

        logger.LogInformation("{WorkingDirectory}> {Input} [DRY-RUN]",
            workingDirectory,
            ObfuscateLogValue(input));
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

        var obfuscatedOutput = ObfuscateLogValue(line);
        Logger.LogInformation(
            isError ? "  ↳ {CommandError}" : "  ↳ {CommandOutput}",
            obfuscatedOutput);
    }

    private static bool ShouldInlineOutput(CommandLoggingOptions options, string output)
    {
        return !string.IsNullOrEmpty(output)
               && !output.Contains('\n')
               && output.Length <= MaximumInlineOutputLength
               && options.Verbosity >= CommandLogVerbosity.Normal
               && options.ShowStandardOutput;
    }

    private static (string Output, string Error) ManipulateOutput(
        Func<string, string>? manipulator,
        string standardOutput,
        string standardError)
    {
        if (manipulator is null)
        {
            return (standardOutput, standardError);
        }

        return (manipulator(standardOutput), manipulator(standardError));
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
        bool isSuccess)
    {
        if (isSuccess && ShouldInlineOutput(options, output))
        {
            Logger.LogInformation(
                "  → {CommandOutput}",
                ObfuscateLogValue(output));
            return;
        }

        if (string.IsNullOrWhiteSpace(output)
            || options.Verbosity < CommandLogVerbosity.Normal
            || !options.ShowStandardOutput)
        {
            return;
        }

        Logger.LogInformation("  ↳ {CommandOutput}", ObfuscateLogValue(output));
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

        Logger.LogWarning("  ✗ {CommandError}", ObfuscateLogValue(error));
    }

    private void LogCommandStatus(
        CommandLoggingOptions options,
        string? inputToLog,
        bool isSuccess,
        int? exitCode,
        TimeSpan? runTime)
    {
        var commandStatus = BuildCommandStatus(options, isSuccess, exitCode, runTime);
        if (string.IsNullOrEmpty(commandStatus)
            && options.Verbosity >= CommandLogVerbosity.Normal)
        {
            commandStatus = isSuccess ? "✓" : "✗";
        }

        if (!string.IsNullOrEmpty(commandStatus))
        {
            var obfuscatedInput = ShouldShowInput(options)
                ? ObfuscateLogValue(inputToLog)
                : new PreObfuscatedLogValue(LoggingConstants.CommandMask);
            Logger.LogInformation(
                "{CommandStatus} {Input}",
                commandStatus.TrimStart(),
                obfuscatedInput);
        }
    }

    private PreObfuscatedLogValue ObfuscateLogValue(string? value) =>
        new(_secretObfuscator.Obfuscate(value, null));

    private static bool ShouldShowInput(CommandLoggingOptions options)
    {
        // ShowCommandArguments controls whether to show full command or obfuscated
        return options.ShowCommandArguments;
    }
}
