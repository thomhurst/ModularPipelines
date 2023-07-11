using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("swarm join")]
public record DockerSwarmJoinOptions([property: PositionalArgument(Position = Position.AfterArguments)] string HostPort) : DockerOptions
{

    [CommandSwitch("--advertise-addr")]
    public string? AdvertiseAddr { get; set; }


    [CommandSwitch("--availability")]
    public string? Availability { get; set; }


    [CommandSwitch("--data-path-addr")]
    public string? DataPathAddr { get; set; }


    [CommandSwitch("--listen-addr")]
    public string? ListenAddr { get; set; }


    [CommandSwitch("--token")]
    public string? Token { get; set; }

}