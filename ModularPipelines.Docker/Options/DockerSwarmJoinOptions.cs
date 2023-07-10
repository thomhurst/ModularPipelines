using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("swarm join")]
public record DockerSwarmJoinOptions : DockerOptions
{
    [CommandLongSwitch("advertise-addr")]
    public string? AdvertiseAddr { get; set; }

    [CommandLongSwitch("availability")]
    public string? Availability { get; set; }

    [CommandLongSwitch("data-path-addr")]
    public string? DataPathAddr { get; set; }

    [CommandLongSwitch("listen-addr")]
    public string? ListenAddr { get; set; }

    [CommandLongSwitch("token")]
    public string? Token { get; set; }

}