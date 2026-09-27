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

    private const string OutputLinePrefix = "  ↳ ";
    private const string ErrorLinePrefix = "  ✗ ";
    private const string ErrorContinuationIndent = "    ";

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

        LogCapturedOutput(effectiveOptions, outputToLog);
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

        // Blank lines (for example MSBuild's section spacing) would only render as a bare marker.
        if (string.IsNullOrWhiteSpace(line))
        {
            return;
        }

        var obfuscatedOutput = ObfuscateLogValue(line.TrimEnd());
        Logger.LogInformation(
            isError ? OutputLinePrefix + "{CommandError}" : OutputLinePrefix + "{CommandOutput}",
            obfuscatedOutput);
    }

    /// <summary>
    /// Splits captured output into non-blank lines so each one carries the same marker as
    /// streamed output.
    /// </summary>
    internal static IEnumerable<string> GetOutputLines(string output) =>
        output
            .Split('\n')
            .Select(static line => line.TrimEnd())
            .Where(static line => !string.IsNullOrWhiteSpace(line));

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
        string output)
    {
        if (string.IsNullOrWhiteSpace(output)
            || options.Verbosity < CommandLogVerbosity.Normal
            || !options.ShowStandardOutput)
        {
            return;
        }

        // Obfuscate before splitting so secrets that span lines are still masked.
        var obfuscatedOutput = _secretObfuscator.Obfuscate(output, null);
        foreach (var line in GetOutputLines(obfuscatedOutput))
        {
            Logger.LogInformation(
                OutputLinePrefix + "{CommandOutput}",
                new PreObfuscatedLogValue(line));
        }
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

        // Standard error stays one warning so build systems raise a single annotation for it;
        // continuation lines are indented to align under the first.
        var errorText = string.Join(
            Environment.NewLine + ErrorContinuationIndent,
            GetOutputLines(_secretObfuscator.Obfuscate(error, null)));
        Logger.LogWarning(ErrorLinePrefix + "{CommandError}", new PreObfuscatedLogValue(errorText));
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
