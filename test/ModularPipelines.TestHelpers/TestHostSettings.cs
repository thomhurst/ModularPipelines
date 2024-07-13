using Microsoft.Extensions.Logging;
using ModularPipelines.Enums;

namespace ModularPipelines.TestHelpers;

public record TestHostSettings
{
    public CommandLogging CommandLogging { get; init; } = CommandLogging.Input | CommandLogging.Error;
    public LogLevel LogLevel { get; init; } = LogLevel.Warning;
    public bool ClearLogProviders { get; init; } = true;
}