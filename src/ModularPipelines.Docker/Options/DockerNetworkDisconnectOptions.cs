using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerNetworkDisconnectOptions : DockerOptions
{
    public DockerNetworkDisconnectOptions(
        string network,
        string container
    )
    {
        CommandParts = ["network", "disconnect"];

        Network = network;

        Container = container;
    }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Network { get; set; }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Container { get; set; }

    [BooleanCommandSwitch("--force")]
    public virtual bool? Force { get; set; }
}
