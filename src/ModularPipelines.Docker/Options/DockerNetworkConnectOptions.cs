using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("network connect")]
[ExcludeFromCodeCoverage]
public record DockerNetworkConnectOptions([property: PositionalArgument(Position = Position.AfterSwitches)] string Network, [property: PositionalArgument(Position = Position.AfterSwitches)] string Container) : DockerOptions
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
