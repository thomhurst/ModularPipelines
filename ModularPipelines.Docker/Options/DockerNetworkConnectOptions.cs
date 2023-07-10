using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("network connect")]
public record DockerNetworkConnectOptions : DockerOptions
{
    [CommandLongSwitch("driver-opt")]
    public string? DriverOpt { get; set; }

    [CommandLongSwitch("ip6")]
    public string? Ip6 { get; set; }

    [CommandLongSwitch("link-local-ip")]
    public string? LinkLocalIp { get; set; }

}