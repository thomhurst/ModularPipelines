using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("network rm")]
public record DockerNetworkRmOptions([property: PositionalArgument(Position = Position.AfterArguments)] IEnumerable<string> Network) : DockerOptions
{
    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }

}