using ModularPipelines.Options;

namespace ModularPipelines.Logging;

internal interface ICommandOutputLogger
{
    void LogStandardOutputLine(
        CommandLineToolOptions options,
        CommandExecutionOptions executionOptions,
        string line);

    void LogStandardErrorLine(
        CommandLineToolOptions options,
        CommandExecutionOptions executionOptions,
        string line);
}
