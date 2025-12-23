using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Engine;
using ModularPipelines.Enums;
using ModularPipelines.Helpers;
using ModularPipelines.Options;

namespace ModularPipelines.Logging;

internal class CommandLogger : ICommandLogger
{
    private readonly IModuleLoggerProvider _moduleLoggerProvider;
    private readonly IOptions<PipelineOptions> _pipelineOptions;
    private readonly ISecretObfuscator _secretObfuscator;
    private readonly object _lock = new();

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
        CommandLineToolOptions options,
        CommandLoggingOptions? loggingOptions,
        string? inputToLog,
        int? exitCode,
        TimeSpan? runTime,
        string standardOutput,
        string standardError,
        string commandWorkingDirPath)
    {
        // If explicit logging options specify Silent, return immediately
        if (loggingOptions?.Verbosity == CommandLogVerbosity.Silent)
        {
            return;
        }

        if (options.CommandLogging == CommandLogging.None)
        {
            return;
        }

        var loggingConfig = options.CommandLogging ?? _pipelineOptions.Value.DefaultCommandLogging;

        lock (_lock)
        {
            if (options.InternalDryRun)
            {
                LogDryRunCommand(loggingConfig, loggingOptions, commandWorkingDirPath, inputToLog);
                return;
            }

            LogCommandInput(loggingConfig, loggingOptions, options, commandWorkingDirPath, inputToLog);
            LogExitCode(loggingConfig, loggingOptions, exitCode);
            LogDuration(loggingConfig, loggingOptions, runTime);
            LogOutput(loggingConfig, loggingOptions, options, standardOutput);
            LogError(loggingConfig, loggingOptions, options, exitCode, standardError);
            LogWorkingDirectory(loggingOptions, commandWorkingDirPath);
        }
    }

    private void LogDryRunCommand(CommandLogging loggingConfig, CommandLoggingOptions? loggingOptions, string workingDirectory, string? input)
    {
        if (!ShouldLogInput(loggingConfig, loggingOptions))
        {
            return;
        }

        var timestamp = GetTimestampPrefix(loggingOptions);
        Logger.LogInformation("{Timestamp}Command (Dry-Run): {WorkingDirectory}> {Input}",
            timestamp,
            workingDirectory,
            input);
        Logger.LogInformation("{Timestamp}⚠ Dry-Run: No actual execution", timestamp);
    }

    private void LogCommandInput(CommandLogging loggingConfig, CommandLoggingOptions? loggingOptions, CommandLineToolOptions options, string workingDirectory, string? input)
    {
        // If no command logging at all, skip
        if (loggingConfig == CommandLogging.None)
        {
            return;
        }

        var timestamp = GetTimestampPrefix(loggingOptions);

        // If Input flag is set, show actual command; otherwise show obfuscated
        if (ShouldLogInput(loggingConfig, loggingOptions))
        {
            Logger.LogInformation("{Timestamp}Command: {WorkingDirectory}> {Input}",
                timestamp,
                workingDirectory,
                _secretObfuscator.Obfuscate(input, options));
        }
        else
        {
            // Still log command header with obfuscated input if any other logging is enabled
            Logger.LogInformation("{Timestamp}Command: {WorkingDirectory}> ********",
                timestamp,
                workingDirectory);
        }
    }

    private void LogExitCode(CommandLogging loggingConfig, CommandLoggingOptions? loggingOptions, int? exitCode)
    {
        // Check both legacy flag and new options
        var shouldLog = loggingConfig.HasFlag(CommandLogging.ExitCode) ||
                        (loggingOptions?.ShowExitCode ?? false);

        if (!shouldLog)
        {
            return;
        }

        var timestamp = GetTimestampPrefix(loggingOptions);
        var icon = exitCode == 0 ? "✓" : "✗";
        Logger.LogInformation("{Timestamp}{Icon} Exit Code: {ExitCode}", timestamp, icon, exitCode);
    }

    private void LogDuration(CommandLogging loggingConfig, CommandLoggingOptions? loggingOptions, TimeSpan? runTime)
    {
        // Check both legacy flag and new options
        var shouldLog = loggingConfig.HasFlag(CommandLogging.Duration) ||
                        (loggingOptions?.ShowExecutionTime ?? false);

        if (!shouldLog)
        {
            return;
        }

        var timestamp = GetTimestampPrefix(loggingOptions);
        Logger.LogInformation("{Timestamp}Duration: {Duration}", timestamp, runTime?.ToDisplayString());
    }

    private void LogOutput(CommandLogging loggingConfig, CommandLoggingOptions? loggingOptions, CommandLineToolOptions options, string standardOutput)
    {
        if (!ShouldLogOutput(loggingConfig, loggingOptions) || string.IsNullOrWhiteSpace(standardOutput))
        {
            return;
        }

        var timestamp = GetTimestampPrefix(loggingOptions);
        Logger.LogInformation("{Timestamp}Output:\n{Output}", timestamp, _secretObfuscator.Obfuscate(standardOutput, options));
    }

    private void LogError(CommandLogging loggingConfig, CommandLoggingOptions? loggingOptions, CommandLineToolOptions options, int? exitCode, string standardError)
    {
        if (!ShouldLogError(loggingConfig, loggingOptions, exitCode) || string.IsNullOrWhiteSpace(standardError))
        {
            return;
        }

        var timestamp = GetTimestampPrefix(loggingOptions);
        Logger.LogInformation("{Timestamp}✗ Error:\n{Error}", timestamp, _secretObfuscator.Obfuscate(standardError, options));
    }

    private void LogWorkingDirectory(CommandLoggingOptions? loggingOptions, string workingDirectory)
    {
        if (loggingOptions?.ShowWorkingDirectory != true)
        {
            return;
        }

        var timestamp = GetTimestampPrefix(loggingOptions);
        Logger.LogInformation("{Timestamp}Working Directory: {WorkingDirectory}", timestamp, workingDirectory);
    }

    private static string GetTimestampPrefix(CommandLoggingOptions? loggingOptions)
    {
        if (loggingOptions?.IncludeTimestamps != true)
        {
            return string.Empty;
        }

        return $"[{DateTime.UtcNow:HH:mm:ss.fff}] ";
    }

    private static bool ShouldLogInput(CommandLogging commandLogging, CommandLoggingOptions? loggingOptions)
    {
        // If explicit logging options say don't show arguments, respect that
        if (loggingOptions?.ShowCommandArguments == false)
        {
            return false;
        }

        return commandLogging.HasFlag(CommandLogging.Input);
    }

    private static bool ShouldLogOutput(CommandLogging commandLogging, CommandLoggingOptions? loggingOptions)
    {
        // If explicit logging options say don't show output, respect that
        if (loggingOptions?.ShowStandardOutput == false)
        {
            return false;
        }

        return commandLogging.HasFlag(CommandLogging.Output);
    }

    private static bool ShouldLogError(CommandLogging commandLogging, CommandLoggingOptions? loggingOptions, int? resultCode)
    {
        // If explicit logging options say don't show error, respect that
        if (loggingOptions?.ShowStandardError == false)
        {
            return false;
        }

        return resultCode != 0 && commandLogging.HasFlag(CommandLogging.Error);
    }
}