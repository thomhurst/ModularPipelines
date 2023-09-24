using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Enums;
using ModularPipelines.Options;

namespace ModularPipelines.Logging;

internal class CommandLogger : ICommandLogger
{
    private readonly IModuleLoggerProvider _moduleLoggerProvider;
    private readonly IOptions<PipelineOptions> _pipelineOptions;
    private readonly object _lock = new();

    public CommandLogger(IModuleLoggerProvider moduleLoggerProvider, IOptions<PipelineOptions> pipelineOptions)
    {
        _moduleLoggerProvider = moduleLoggerProvider;
        _pipelineOptions = pipelineOptions;
    }

    private ILogger Logger => _moduleLoggerProvider.GetLogger();

    public void Log(CommandLineToolOptions options, string? inputToLog, int? resultExitCode, string? outputToLog,
        string? errorToLog)
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
                Logger.LogInformation("---Executing Command---\r\n\t{Input}", inputToLog);
                Logger.LogInformation("---Dry-Run Command - No Output---");
                return;
            }

            if (ShouldLogInput(optionsCommandLogging))
            {
                Logger.LogInformation("""
                                      ---Executing Command---
                                      {Input}
                                      """,
                    inputToLog);
            }
            else
            {
                Logger.LogInformation("""
                                      ---Executing Command---
                                      ********
                                      """);
            }

            if (ShouldLogOutput(optionsCommandLogging))
            {
                Logger.LogInformation("""
                                      ---Command Result---
                                      {Output}
                                      """, outputToLog);
            }

            if (ShouldLogError(optionsCommandLogging, resultExitCode))
            {
                Logger.LogInformation("""
                                      ---Command Error (Result code - {ResultCode})---
                                      {Error}
                                      """, resultExitCode, errorToLog);
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