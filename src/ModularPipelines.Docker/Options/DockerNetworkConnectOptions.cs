using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerNetworkConnectOptions : DockerOptions
{
    public DockerNetworkConnectOptions(
        string network,
        string container
    )
    {
        CommandParts = ["network", "connect"];

        Network = network;

        Container = container;
    }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Network { get; set; }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Container { get; set; }

    [CommandSwitch("--alias")]
    public virtual string? Alias { get; set; }

    [CommandSwitch("--driver-opt")]
    public virtual string? DriverOpt { get; set; }

    [CommandSwitch("--ip")]
    public virtual string? Ip { get; set; }

    [CommandSwitch("--ip6")]
    public virtual string? Ip6 { get; set; }

    [CommandSwitch("--link")]
    public virtual string? Link { get; set; }

    [CommandSwitch("--link-local-ip")]
    public virtual string? LinkLocalIp { get; set; }
}
