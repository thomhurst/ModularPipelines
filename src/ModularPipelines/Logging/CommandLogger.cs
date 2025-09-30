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

        var optionsCommandLogging = options.CommandLogging ?? _pipelineOptions.Value.DefaultCommandLogging;

        lock (_lock)
        {
            if (options.InternalDryRun && ShouldLogInput(optionsCommandLogging))
            {
                Logger.LogInformation("[bold cyan]Command (Dry-Run):[/] {WorkingDirectory}> {Input}", commandWorkingDirPath, inputToLog);
                Logger.LogInformation("[yellow]Dry-Run: No actual execution[/]");
                return;
            }

            if (ShouldLogInput(optionsCommandLogging))
            {
                Logger.LogInformation("[bold cyan]Command:[/] {WorkingDirectory}> {Input}",
                    commandWorkingDirPath,
                    _secretObfuscator.Obfuscate(inputToLog, options));
            }
            else
            {
                Logger.LogInformation("[bold cyan]Command:[/] {WorkingDirectory}> ********", commandWorkingDirPath);
            }

            if (optionsCommandLogging.HasFlag(CommandLogging.ExitCode))
            {
                var exitCodeColor = exitCode == 0 ? "green" : "red";
                Logger.LogInformation("[bold]Exit Code:[/] [{0}]{1}[/]", exitCodeColor, exitCode);
            }

            if (optionsCommandLogging.HasFlag(CommandLogging.Duration))
            {
                Logger.LogInformation("[bold]Duration:[/] {Duration}", runTime?.ToDisplayString());
            }

            if (ShouldLogOutput(optionsCommandLogging))
            {
                if (!string.IsNullOrWhiteSpace(standardOutput))
                {
                    Logger.LogInformation("[bold]Output:[/]\n{Output}", _secretObfuscator.Obfuscate(standardOutput, options));
                }
            }

            if (ShouldLogError(optionsCommandLogging, exitCode))
            {
                if (!string.IsNullOrWhiteSpace(standardError))
                {
                    Logger.LogInformation("[bold red]Error:[/]\n{Error}", _secretObfuscator.Obfuscate(standardError, options));
                }
            }
        }
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