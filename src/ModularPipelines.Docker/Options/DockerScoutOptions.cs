using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("scout")]
[ExcludeFromCodeCoverage]
public record DockerScoutOptions : DockerOptions
{
    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Command { get; set; }

    [CommandSwitch("--verbose-debug")]
    public string? VerboseDebug { get; set; }
}
