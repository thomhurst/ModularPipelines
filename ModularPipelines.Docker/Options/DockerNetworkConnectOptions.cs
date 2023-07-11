using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("network connect")]
public record DockerNetworkConnectOptions([property: PositionalArgument(Position = Position.AfterArguments)] string Network, [property: PositionalArgument(Position = Position.AfterArguments)] string Container) : DockerOptions
{

    [CommandSwitch("--driver-opt")]
    public string? DriverOpt { get; set; }

    [CommandSwitch("--ip6")]
    public string? Ip6 { get; set; }

    [CommandSwitch("--link-local-ip")]
    public string? LinkLocalIp { get; set; }

    [CommandSwitch("--alias")]
    public string? Alias { get; set; }

    [CommandSwitch("--ip")]
    public string? Ip { get; set; }

    [CommandSwitch("--link")]
    public string? Link { get; set; }

}