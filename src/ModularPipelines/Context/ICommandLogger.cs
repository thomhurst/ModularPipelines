using ModularPipelines.Options;

namespace ModularPipelines.Context;

internal interface ICommandLogger
{
    void Log(CommandLineToolOptions options, string? inputToLog, int? resultExitCode, string? outputToLog, string? errorToLog);
}