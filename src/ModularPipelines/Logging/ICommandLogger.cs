using ModularPipelines.Options;

namespace ModularPipelines.Logging;

public interface ICommandLogger
{
    void Log(CommandLineToolOptions options,
        string? inputToLog,
        int? exitCode,
        TimeSpan? runTime,
        string standardOutput,
        string standardError, string commandWorkingDirPath);
}