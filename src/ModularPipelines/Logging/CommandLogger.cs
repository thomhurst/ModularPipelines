using Microsoft.Extensions.Logging;
using ModularPipelines.Enums;
using ModularPipelines.Options;

namespace ModularPipelines.Logging;

internal class CommandLogger : ICommandLogger
{
    private readonly IModuleLoggerProvider _moduleLoggerProvider;
    private readonly object _lock = new();

    public CommandLogger(IModuleLoggerProvider moduleLoggerProvider)
    {
        _moduleLoggerProvider = moduleLoggerProvider;
    }

    private ILogger Logger => _moduleLoggerProvider.GetLogger();

    public void Log(CommandLineToolOptions options, string? inputToLog, int? resultExitCode, string? outputToLog,
        string? errorToLog)
    {
        if (options.CommandLogging == CommandLogging.None)
        {
            return;
        }

        lock (_lock)
        {

            if (options.InternalDryRun && ShouldLogInput(options))
            {
                Logger.LogInformation("---Executing Command---\r\n\t{Input}", inputToLog);
                Logger.LogInformation("---Dry-Run Command - No Output---");
                return;
            }

            if (ShouldLogInput(options))
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

            if (ShouldLogOutput(options))
            {
                Logger.LogInformation("""
                                      ---Command Result---
                                      {Output}
                                      """, outputToLog);
            }

            if (ShouldLogError(options, resultExitCode))
            {
                Logger.LogInformation("""
                                      ---Command Error (Result code - {ResultCode})---
                                      {Error}
                                      """, resultExitCode, errorToLog);
            }
        }
    }

    private static bool ShouldLogInput(CommandLineOptions options)
    {
        return options.CommandLogging.HasFlag(CommandLogging.Input);
    }

    private static bool ShouldLogOutput(CommandLineOptions options)
    {
        return options.CommandLogging.HasFlag(CommandLogging.Output);
    }

    private static bool ShouldLogError(CommandLineOptions options, int? resultCode)
    {
        return resultCode != 0 && options.CommandLogging.HasFlag(CommandLogging.Error);
    }
}