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

        Logger.LogInformation("[bold cyan]Command (Dry-Run):[/] {WorkingDirectory}> {Input}",
            workingDirectory,
            input);
        Logger.LogInformation("[yellow]Dry-Run: No actual execution[/]");
    }

    private void LogCommandInput(CommandLogging loggingConfig, CommandLineToolOptions options, string workingDirectory, string? input)
    {
        // If no command logging at all, skip
        if (loggingConfig == CommandLogging.None)
        {
            return;
        }

        // If Input flag is set, show actual command; otherwise show obfuscated
        if (ShouldLogInput(loggingConfig))
        {
            Logger.LogInformation("[bold cyan]Command:[/] {WorkingDirectory}> {Input}",
                workingDirectory,
                _secretObfuscator.Obfuscate(input, options));
        }
        else
        {
            // Still log command header with obfuscated input if any other logging is enabled
            Logger.LogInformation("[bold cyan]Command:[/] {WorkingDirectory}> ********",
                workingDirectory);
        }
    }

    private void LogExitCode(CommandLogging loggingConfig, int? exitCode)
    {
        if (!loggingConfig.HasFlag(CommandLogging.ExitCode))
        {
            return;
        }

        var exitCodeColor = MarkupFormatter.GetExitCodeColor(exitCode);
        Logger.LogInformation($"[bold]Exit Code:[/] [{exitCodeColor}]{{ExitCode}}[/]", exitCode);
    }

    private void LogDuration(CommandLogging loggingConfig, TimeSpan? runTime)
    {
        if (!loggingConfig.HasFlag(CommandLogging.Duration))
        {
            return;
        }

        Logger.LogInformation("[bold]Duration:[/] {Duration}", runTime?.ToDisplayString());
    }

    private void LogOutput(CommandLogging loggingConfig, CommandLineToolOptions options, string standardOutput)
    {
        if (!ShouldLogOutput(loggingConfig) || string.IsNullOrWhiteSpace(standardOutput))
        {
            return;
        }

        Logger.LogInformation("[bold]Output:[/]\n{Output}", _secretObfuscator.Obfuscate(standardOutput, options));
    }

    private void LogError(CommandLogging loggingConfig, CommandLineToolOptions options, int? exitCode, string standardError)
    {
        if (!ShouldLogError(loggingConfig, exitCode) || string.IsNullOrWhiteSpace(standardError))
        {
            return;
        }

        Logger.LogInformation("[bold red]Error:[/]\n{Error}", _secretObfuscator.Obfuscate(standardError, options));
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