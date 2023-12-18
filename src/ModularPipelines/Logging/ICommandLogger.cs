using CliWrap.Buffered;
using ModularPipelines.Options;

namespace ModularPipelines.Logging;

internal interface ICommandLogger
{
    void Log(CommandLineToolOptions options, string? inputToLog, BufferedCommandResult commandResult);
}