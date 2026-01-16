using System.Text;
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

        // Use compact logging format for cleaner output
        LogCompact(effectiveOptions, execOpts, commandWorkingDirPath, inputToLog, exitCode, runTime, standardOutput, standardError);
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
            ? _secretObfuscator.Obfuscate(input, execOpts)
            : LoggingConstants.CommandMask;

        // Build the main command line with inline metadata
        var mainLine = new StringBuilder();
        mainLine.Append(workingDirectory);
        mainLine.Append("> ");
        mainLine.Append(obfuscatedInput);

        // Add inline output for short, single-line output on successful commands
        var trimmedOutput = standardOutput.Trim();
        var hasShortOutput = !string.IsNullOrEmpty(trimmedOutput)
            && !trimmedOutput.Contains('\n')
            && trimmedOutput.Length <= 100
            && options.Verbosity >= CommandLogVerbosity.Normal
            && options.ShowStandardOutput;

        if (hasShortOutput && isSuccess)
        {
            mainLine.Append(" → ");
            mainLine.Append(_secretObfuscator.Obfuscate(trimmedOutput, execOpts));
        }

        // Add status indicator and metadata
        if (options.Verbosity >= CommandLogVerbosity.Detailed || options.ShowExitCode || options.ShowExecutionTime)
        {
            mainLine.Append(' ');
            mainLine.Append(isSuccess ? '✓' : '✗');

            var hasMetadata = false;
            if (options.Verbosity >= CommandLogVerbosity.Detailed || options.ShowExecutionTime)
            {
                mainLine.Append(" [");
                mainLine.Append(runTime?.ToDisplayString() ?? "?");
                hasMetadata = true;
            }

            if (options.Verbosity >= CommandLogVerbosity.Detailed || options.ShowExitCode)
            {
                if (hasMetadata)
                {
                    mainLine.Append(", ");
                }
                else
                {
                    mainLine.Append(" [");
                    hasMetadata = true;
                }

                mainLine.Append("exit ");
                mainLine.Append(exitCode);
            }

            if (hasMetadata)
            {
                mainLine.Append(']');
            }
        }

        Logger.LogInformation("{Message}", mainLine.ToString());

        // Log multi-line or long output on separate lines (only if not already shown inline)
        if (!hasShortOutput && !string.IsNullOrWhiteSpace(trimmedOutput)
            && options.Verbosity >= CommandLogVerbosity.Normal
            && options.ShowStandardOutput)
        {
            var outputToLog = execOpts?.OutputLoggingManipulator is not null
                ? execOpts.OutputLoggingManipulator(trimmedOutput)
                : trimmedOutput;
            Logger.LogInformation("  ↳ {Output}", _secretObfuscator.Obfuscate(outputToLog, execOpts));
        }

        // Log errors on separate line
        if (!string.IsNullOrWhiteSpace(standardError)
            && options.Verbosity >= CommandLogVerbosity.Normal
            && options.ShowStandardError
            && exitCode != 0)
        {
            var errorToLog = execOpts?.OutputLoggingManipulator is not null
                ? execOpts.OutputLoggingManipulator(standardError)
                : standardError;
            Logger.LogWarning("  ✗ {Error}", _secretObfuscator.Obfuscate(errorToLog, execOpts));
        }

        // Log working directory only at Diagnostic level (separate line, indented)
        if (options.Verbosity >= CommandLogVerbosity.Diagnostic || options.ShowWorkingDirectory)
        {
            Logger.LogInformation("  Working Directory: {WorkingDirectory}", workingDirectory);
        }
    }

    private static bool ShouldShowInput(CommandLoggingOptions options)
    {
        // ShowCommandArguments controls whether to show full command or obfuscated
        return options.ShowCommandArguments;
    }
}
