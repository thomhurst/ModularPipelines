using ModularPipelines.Enums;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace ModularPipelines.TestHelpers;

public record TestHostSettings
{
    public CommandLogging CommandLogging { get; init; } = CommandLogging.Input | CommandLogging.Error;
    public LogLevel LogLevel { get; init; } = LogLevel.Warning;
    public bool ClearLogProviders { get; init; } = true;
    public bool ShowProgressInConsole { get; init; } = false;
}