using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("context rm")]
public record DockerContextRmOptions([property: PositionalArgument(Position = Position.AfterArguments)] IEnumerable<string> Context) : DockerOptions
{
    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }
}