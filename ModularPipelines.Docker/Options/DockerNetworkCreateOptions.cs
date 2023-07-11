using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("network create")]
public record DockerNetworkCreateOptions([property: PositionalArgument(Position = Position.AfterArguments)] string Network) : DockerOptions
{
    [CommandSwitch("--attachable")]
    public string? Attachable { get; set; }

    [CommandSwitch("--aux-address")]
    public string? AuxAddress { get; set; }

    [CommandSwitch("--config-from")]
    public string? ConfigFrom { get; set; }

    [CommandSwitch("--config-only")]
    public string? ConfigOnly { get; set; }

    [CommandSwitch("--driver")]
    public string? Driver { get; set; }

    [CommandSwitch("--gateway")]
    public string? Gateway { get; set; }

    [CommandSwitch("--ip-range")]
    public string? IpRange { get; set; }

    [CommandSwitch("--ipam-driver")]
    public string? IpamDriver { get; set; }

    [CommandSwitch("--ipam-opt")]
    public string? IpamOpt { get; set; }

    [CommandSwitch("--ipv6")]
    public string? Ipv6 { get; set; }

    [CommandSwitch("--label")]
    public string? Label { get; set; }

    [CommandSwitch("--opt")]
    public string? Opt { get; set; }

    [CommandSwitch("--scope")]
    public string? Scope { get; set; }

    [CommandSwitch("--subnet")]
    public string? Subnet { get; set; }

    [CommandSwitch("--ingress")]
    public string? Ingress { get; set; }

    [CommandSwitch("--internal")]
    public string? Internal { get; set; }

}