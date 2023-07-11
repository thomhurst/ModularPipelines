using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("node rm")]
public record DockerNodeRmOptions([property: PositionalArgument(Position = Position.AfterArguments)] IEnumerable<string> Node) : DockerOptions
{
    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }
}