using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("network disconnect")]
public record DockerNetworkDisconnectOptions([property: PositionalArgument(Position = Position.AfterArguments)] string Network, [property: PositionalArgument(Position = Position.AfterArguments)] string Container) : DockerOptions
{
    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }

}