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
        if (effectiveOptions.Verbosity == CommandLogVerbosity.Silent
            && !effectiveOptions.IncludesCommandArguments
            && !effectiveOptions.IncludesWorkingDirectory
            && !effectiveOptions.IncludesTimestamps)
        {
            return;
        }

        var obfuscatedInput = effectiveOptions.IncludesCommandArguments
            ? ObfuscateLogValue(inputToLog)
            : new PreObfuscatedLogValue(LoggingConstants.CommandMask);
        var dryRun = execOpts?.InternalDryRun == true ? " [DRY-RUN]" : string.Empty;
        if (effectiveOptions.IncludesWorkingDirectory)
        {
            Logger.LogInformation(
                "{CommandTimestamp}{WorkingDirectory}> {Input}{DryRun}",
                GetTimestamp(effectiveOptions),
                ObfuscateLogValue(commandWorkingDirPath),
                obfuscatedInput,
                dryRun);
        }
        else
        {
            Logger.LogInformation("{CommandTimestamp}{Input}{DryRun}", GetTimestamp(effectiveOptions), obfuscatedInput, dryRun);
        }
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
        if (execOpts?.InternalDryRun == true
            || (effectiveOptions.Verbosity < CommandLogVerbosity.Normal
                && !effectiveOptions.IncludesStandardOutput
                && !effectiveOptions.IncludesStandardError
                && !effectiveOptions.IncludesExitCode
                && !effectiveOptions.IncludesExecutionTime))
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

    private void LogOutputLine(
        CommandLineToolOptions options,
        CommandExecutionOptions executionOptions,
        string line,
        bool isError)
    {
        var effectiveOptions = GetEffectiveLoggingOptions(options, executionOptions);
        var shouldLog = isError
            ? effectiveOptions.IncludesStandardError
            : effectiveOptions.IncludesStandardOutput;
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
            isError ? "{CommandTimestamp}" + OutputLinePrefix + "{CommandError}" : "{CommandTimestamp}" + OutputLinePrefix + "{CommandOutput}",
            GetTimestamp(effectiveOptions),
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
        var showExecutionTime = options.IncludesExecutionTime;
        var showExitCode = options.IncludesExitCode;
        if (!showExecutionTime && !showExitCode)
        {
            return !isSuccess
                   && options.Verbosity >= CommandLogVerbosity.Normal
                   && options.IncludesStandardError
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
            || !options.IncludesStandardOutput)
        {
            return;
        }

        // Obfuscate before splitting so secrets that span lines are still masked.
        var obfuscatedOutput = _secretObfuscator.Obfuscate(output, null);
        foreach (var line in GetOutputLines(obfuscatedOutput))
        {
            Logger.LogInformation(
                "{CommandTimestamp}" + OutputLinePrefix + "{CommandOutput}",
                GetTimestamp(options),
                new PreObfuscatedLogValue(line));
        }
    }

    private void LogCapturedError(
        CommandLoggingOptions options,
        string error,
        int? exitCode)
    {
        if (string.IsNullOrWhiteSpace(error)
            || !options.IncludesStandardError
            || (exitCode == 0 && options.ShowStandardError is null))
        {
            return;
        }

        // Standard error stays one warning so build systems raise a single annotation for it;
        // continuation lines are indented to align under the first.
        var errorText = string.Join(
            Environment.NewLine + ErrorContinuationIndent,
            GetOutputLines(_secretObfuscator.Obfuscate(error, null)));
        Logger.LogWarning("{CommandTimestamp}" + ErrorLinePrefix + "{CommandError}", GetTimestamp(options), new PreObfuscatedLogValue(errorText));
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
            var obfuscatedInput = options.IncludesCommandArguments
                ? ObfuscateLogValue(inputToLog)
                : new PreObfuscatedLogValue(LoggingConstants.CommandMask);
            Logger.LogInformation(
                "{CommandTimestamp}{CommandStatus} {Input}",
                GetTimestamp(options),
                commandStatus.TrimStart(),
                obfuscatedInput);
        }
    }

    private PreObfuscatedLogValue ObfuscateLogValue(string? value) =>
        new(_secretObfuscator.Obfuscate(value, null));

    private static string GetTimestamp(CommandLoggingOptions options) =>
        options.IncludesTimestamps ? $"[{DateTimeOffset.UtcNow:O}] " : string.Empty;
}
