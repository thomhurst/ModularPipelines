using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerSwarmJoinOptions : DockerOptions
{
    public DockerSwarmJoinOptions(
        string host
    )
    {
        CommandParts = ["swarm", "join"];

        JoinHost = host;
    }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? JoinHost { get; set; }

    [CommandSwitch("--advertise-addr")]
    public virtual string? AdvertiseAddr { get; set; }

    [CommandSwitch("--availability")]
    public virtual string? Availability { get; set; }

    [CommandSwitch("--data-path-addr")]
    public virtual string? DataPathAddr { get; set; }

    [CommandSwitch("--listen-addr")]
    public virtual string? ListenAddr { get; set; }

    [CommandSwitch("--token")]
    public virtual string? Token { get; set; }
}
