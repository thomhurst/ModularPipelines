using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("volume rm")]
[ExcludeFromCodeCoverage]
public record DockerVolumeRmOptions([property: PositionalArgument(Position = Position.AfterSwitches)] IEnumerable<string> Volumes) : DockerOptions
{
    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }
}
