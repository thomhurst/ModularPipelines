using Microsoft.Extensions.Logging;
using ModularPipelines.Enums;
using ModularPipelines.Logging;
using ModularPipelines.Options;

namespace ModularPipelines.Context;

internal class CommandLogger : ICommandLogger
{
    private readonly IModuleLoggerProvider _moduleLoggerProvider;
    
    public CommandLogger(IModuleLoggerProvider moduleLoggerProvider)
    {
        _moduleLoggerProvider = moduleLoggerProvider;
    }

    private ILogger Logger => _moduleLoggerProvider.GetLogger();

    public void Log(CommandLineToolOptions options, string? inputToLog, int? resultExitCode, string? outputToLog, string? errorToLog)
    {
        if (options.CommandLogging == CommandLogging.None)
        {
            return;
        }
        
        if (options.InternalDryRun && ShouldLogInput(options))
        {
            Logger.LogInformation("---Executing Command---\r\n\t{Input}", inputToLog);
            Logger.LogInformation("---Dry-Run Command - No Output---");
        }

        if (ShouldLogInput(options) && ShouldLogOutput(options) && !ShouldLogError(options, resultExitCode))
        {
            Logger.LogInformation("""
                                  ---Executing Command---
                                  {Input}

                                  ---Command Result---
                                  Exit Code: {ExitCode}

                                  Output:
                                  {Output}

                                  """,
                inputToLog, resultExitCode, outputToLog);
        }

        if (ShouldLogInput(options) && !ShouldLogOutput(options) && !ShouldLogError(options, resultExitCode))
        {
            Logger.LogInformation("""
                                  ---Executing Command---
                                  {Input}
                                  
                                  """,
                inputToLog);
        }
        
        if (ShouldLogInput(options) && ShouldLogOutput(options) && ShouldLogError(options, resultExitCode))
        {
            Logger.LogInformation("""
                                  ---Executing Command---
                                  {Input}

                                  ---Command Result---
                                  Exit Code: {ExitCode}

                                  Output:
                                  {Output}

                                  Error:
                                  {Error}
                                  """,
                inputToLog, resultExitCode, outputToLog, errorToLog);
        }

        if (!ShouldLogInput(options) && ShouldLogOutput(options) && !ShouldLogError(options, resultExitCode))
        {
            Logger.LogInformation("""
                                  ---Executing Command---
                                  ********

                                  ---Command Result---
                                  Exit Code: {ExitCode}

                                  Output:
                                  {Output}

                                  """,
                resultExitCode, outputToLog);
        }
        
        if (!ShouldLogInput(options) && ShouldLogOutput(options) && ShouldLogError(options, resultExitCode))
        {
            Logger.LogInformation("""
                                  ---Executing Command---
                                  ********

                                  ---Command Result---
                                  Exit Code: {ExitCode}

                                  Output:
                                  {Output}

                                  Error:
                                  {Error}
                                  """,
                resultExitCode, outputToLog, errorToLog);
        }
        
        if (!ShouldLogInput(options) && !ShouldLogOutput(options) && ShouldLogError(options, resultExitCode))
        {
            Logger.LogInformation("""
                                  ---Executing Command---
                                  ********

                                  ---Command Result---
                                  Exit Code: {ExitCode}

                                  Error:
                                  {Error}
                                  """,
                resultExitCode, errorToLog);
        }
        
        if (ShouldLogInput(options) && !ShouldLogOutput(options) && ShouldLogError(options, resultExitCode))
        {
            Logger.LogInformation("""
                                  ---Executing Command---
                                  {Input}

                                  ---Command Result---
                                  Exit Code: {ExitCode}

                                  Error:
                                  {Error}
                                  """,
                inputToLog, resultExitCode, errorToLog);
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
        return resultCode != 0 && options.CommandLogging.HasFlag(CommandLogging.Output);
    }
}