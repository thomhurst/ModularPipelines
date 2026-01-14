using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Constants;
using ModularPipelines.Engine;
using ModularPipelines.Helpers;
using ModularPipelines.Options;

namespace ModularPipelines.Logging;

internal class CommandLogger : ICommandLogger
{
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

        LogCommandInput(effectiveOptions, execOpts, commandWorkingDirPath, inputToLog);
        LogExitCode(effectiveOptions, exitCode);
        LogDuration(effectiveOptions, runTime);
        LogOutput(effectiveOptions, execOpts, standardOutput);
        LogError(effectiveOptions, execOpts, exitCode, standardError);
        LogWorkingDirectory(effectiveOptions, commandWorkingDirPath);
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

        var timestamp = GetTimestampPrefix(options);
        Logger.LogInformation("{Timestamp}Command (Dry-Run): {WorkingDirectory}> {Input}",
            timestamp,
            workingDirectory,
            input);
        Logger.LogInformation("{Timestamp}⚠ Dry-Run: No actual execution", timestamp);
    }

    private void LogCommandInput(CommandLoggingOptions options, CommandExecutionOptions? execOpts, string workingDirectory, string? input)
    {
        // Minimal and above shows command input
        if (options.Verbosity < CommandLogVerbosity.Minimal)
        {
            return;
        }

        var timestamp = GetTimestampPrefix(options);

        if (ShouldShowInput(options))
        {
            Logger.LogInformation("{Timestamp}Command: {WorkingDirectory}> {Input}",
                timestamp,
                workingDirectory,
                _secretObfuscator.Obfuscate(input, execOpts));
        }
        else
        {
            Logger.LogInformation("{Timestamp}Command: {WorkingDirectory}> " + LoggingConstants.CommandMask,
                timestamp,
                workingDirectory);
        }
    }

    private void LogExitCode(CommandLoggingOptions options, int? exitCode)
    {
        // Detailed and above shows exit code, or if explicitly requested
        if (options.Verbosity < CommandLogVerbosity.Detailed && !options.ShowExitCode)
        {
            return;
        }

        var timestamp = GetTimestampPrefix(options);
        var icon = exitCode == 0 ? "✓" : "✗";
        Logger.LogInformation("{Timestamp}{Icon} Exit Code: {ExitCode}", timestamp, icon, exitCode);
    }

    private void LogDuration(CommandLoggingOptions options, TimeSpan? runTime)
    {
        // Detailed and above shows duration, or if explicitly requested
        if (options.Verbosity < CommandLogVerbosity.Detailed && !options.ShowExecutionTime)
        {
            return;
        }

        var timestamp = GetTimestampPrefix(options);
        Logger.LogInformation("{Timestamp}Duration: {Duration}", timestamp, runTime?.ToDisplayString());
    }

    private void LogOutput(CommandLoggingOptions options, CommandExecutionOptions? execOpts, string standardOutput)
    {
        if (string.IsNullOrWhiteSpace(standardOutput))
        {
            return;
        }

        // Verbosity >= Normal shows output by default; ShowStandardOutput can disable it
        if (options.Verbosity < CommandLogVerbosity.Normal || !options.ShowStandardOutput)
        {
            return;
        }

        var timestamp = GetTimestampPrefix(options);
        Logger.LogInformation("{Timestamp}Output:\n{Output}", timestamp, _secretObfuscator.Obfuscate(standardOutput, execOpts));
    }

    private void LogError(CommandLoggingOptions options, CommandExecutionOptions? execOpts, int? exitCode, string standardError)
    {
        if (string.IsNullOrWhiteSpace(standardError))
        {
            return;
        }

        // Verbosity >= Normal shows error on failure by default; ShowStandardError can disable it
        var showDueToVerbosity = options.Verbosity >= CommandLogVerbosity.Normal && exitCode != 0;
        if (!showDueToVerbosity || !options.ShowStandardError)
        {
            return;
        }

        var timestamp = GetTimestampPrefix(options);
        Logger.LogInformation("{Timestamp}✗ Error:\n{Error}", timestamp, _secretObfuscator.Obfuscate(standardError, execOpts));
    }

    private void LogWorkingDirectory(CommandLoggingOptions options, string workingDirectory)
    {
        // Diagnostic shows working directory, or if explicitly requested
        if (options.Verbosity < CommandLogVerbosity.Diagnostic && !options.ShowWorkingDirectory)
        {
            return;
        }

        var timestamp = GetTimestampPrefix(options);
        Logger.LogInformation("{Timestamp}Working Directory: {WorkingDirectory}", timestamp, workingDirectory);
    }

    private static string GetTimestampPrefix(CommandLoggingOptions options)
    {
        // Diagnostic automatically includes timestamps, or if explicitly requested
        if (options.Verbosity < CommandLogVerbosity.Diagnostic && !options.IncludeTimestamps)
        {
            return string.Empty;
        }

        return $"[{DateTime.UtcNow:HH:mm:ss.fff}] ";
    }

    private static bool ShouldShowInput(CommandLoggingOptions options)
    {
        // ShowCommandArguments controls whether to show full command or obfuscated
        return options.ShowCommandArguments;
    }
}
