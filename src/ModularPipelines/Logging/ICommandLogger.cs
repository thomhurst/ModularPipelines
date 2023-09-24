using ModularPipelines.Options;

namespace ModularPipelines.Logging;

internal interface ICommandLogger
{
    void Log(CommandLineToolOptions options, string? inputToLog, int? resultExitCode, string? outputToLog, string? errorToLog);
}