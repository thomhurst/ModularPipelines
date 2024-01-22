using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("volume", "update")]
[ExcludeFromCodeCoverage]
public record DockerVolumeUpdateOptions : DockerOptions
{
    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Volume { get; set; }

    [CommandSwitch("--availability")]
    public string? Availability { get; set; }
}
