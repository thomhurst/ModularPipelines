using ModularPipelines.Enums;

namespace ModularPipelines.TestHelpers;

public record TestHostSettings
{
    public CommandLogging CommandLogging { get; init; } = CommandLogging.Input | CommandLogging.Error;
}