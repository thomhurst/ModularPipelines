using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("volume rm")]
public record DockerVolumeRmOptions([property: PositionalArgument(Position = Position.AfterArguments)] IEnumerable<string> Volume) : DockerOptions
{
    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }
}