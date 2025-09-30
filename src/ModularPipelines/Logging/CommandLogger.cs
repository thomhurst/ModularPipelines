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

    public void Log(CommandLineToolOptions options, string? inputToLog, int? exitCode, TimeSpan? runTime,
        string standardOutput, string standardError, string commandWorkingDirPath)
    {
        if (options.CommandLogging == CommandLogging.None)
        {
            return;
        }

        var loggingConfig = options.CommandLogging ?? _pipelineOptions.Value.DefaultCommandLogging;

        lock (_lock)
        {
            if (options.InternalDryRun)
            {
                LogDryRunCommand(loggingConfig, commandWorkingDirPath, inputToLog);
                return;
            }

            LogCommandInput(loggingConfig, options, commandWorkingDirPath, inputToLog);
            LogExitCode(loggingConfig, exitCode);
            LogDuration(loggingConfig, runTime);
            LogOutput(loggingConfig, options, standardOutput);
            LogError(loggingConfig, options, exitCode, standardError);
        }
    }

    private void LogDryRunCommand(CommandLogging loggingConfig, string workingDirectory, string? input)
    {
        if (!ShouldLogInput(loggingConfig))
        {
            return;
        }

        Logger.LogInformation("{Header} {WorkingDirectory}> {Input}",
            MarkupFormatter.FormatColoredHeader("Command (Dry-Run)", "cyan"),
            workingDirectory,
            input);
        Logger.LogInformation("[yellow]Dry-Run: No actual execution[/]");
    }

    private void LogCommandInput(CommandLogging loggingConfig, CommandLineToolOptions options, string workingDirectory, string? input)
    {
        if (!ShouldLogInput(loggingConfig))
        {
            return;
        }

        if (options.CommandLogging == CommandLogging.None)
        {
            Logger.LogInformation("{Header} {WorkingDirectory}> ********",
                MarkupFormatter.FormatColoredHeader("Command", "cyan"),
                workingDirectory);
        }
        else
        {
            Logger.LogInformation("{Header} {WorkingDirectory}> {Input}",
                MarkupFormatter.FormatColoredHeader("Command", "cyan"),
                workingDirectory,
                _secretObfuscator.Obfuscate(input, options));
        }
    }

    private void LogExitCode(CommandLogging loggingConfig, int? exitCode)
    {
        if (!loggingConfig.HasFlag(CommandLogging.ExitCode))
        {
            return;
        }

        var exitCodeColor = MarkupFormatter.GetExitCodeColor(exitCode);
        Logger.LogInformation("{Header} [{0}]{1}[/]", MarkupFormatter.FormatHeader("Exit Code"), exitCodeColor, exitCode);
    }

    private void LogDuration(CommandLogging loggingConfig, TimeSpan? runTime)
    {
        if (!loggingConfig.HasFlag(CommandLogging.Duration))
        {
            return;
        }

        Logger.LogInformation(MarkupFormatter.FormatDuration(runTime));
    }

    private void LogOutput(CommandLogging loggingConfig, CommandLineToolOptions options, string standardOutput)
    {
        if (!ShouldLogOutput(loggingConfig) || string.IsNullOrWhiteSpace(standardOutput))
        {
            return;
        }

        Logger.LogInformation("{Header}\n{Output}", MarkupFormatter.FormatHeader("Output"), _secretObfuscator.Obfuscate(standardOutput, options));
    }

    private void LogError(CommandLogging loggingConfig, CommandLineToolOptions options, int? exitCode, string standardError)
    {
        if (!ShouldLogError(loggingConfig, exitCode) || string.IsNullOrWhiteSpace(standardError))
        {
            return;
        }

        Logger.LogInformation("{Header}\n{Error}", MarkupFormatter.FormatColoredHeader("Error", "red"), _secretObfuscator.Obfuscate(standardError, options));
    }

    private static bool ShouldLogInput(CommandLogging commandLogging)
    {
        return commandLogging.HasFlag(CommandLogging.Input);
    }

    private static bool ShouldLogOutput(CommandLogging commandLogging)
    {
        return commandLogging.HasFlag(CommandLogging.Output);
    }

    private static bool ShouldLogError(CommandLogging commandLogging, int? resultCode)
    {
        return resultCode != 0 && commandLogging.HasFlag(CommandLogging.Error);
    }
}