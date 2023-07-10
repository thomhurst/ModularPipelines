using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("network create")]
public record DockerNetworkCreateOptions([property: PositionalArgument] string Network) : DockerOptions
{
    [CommandLongSwitch("attachable")]
    public string? Attachable { get; set; }

    [CommandLongSwitch("aux-address")]
    public string? AuxAddress { get; set; }

    [CommandLongSwitch("config-from")]
    public string? ConfigFrom { get; set; }

    [CommandLongSwitch("config-only")]
    public string? ConfigOnly { get; set; }

    [CommandLongSwitch("driver")]
    public string? Driver { get; set; }

    [CommandLongSwitch("gateway")]
    public string? Gateway { get; set; }

    [CommandLongSwitch("ip-range")]
    public string? IpRange { get; set; }

    [CommandLongSwitch("ipam-driver")]
    public string? IpamDriver { get; set; }

    [CommandLongSwitch("ipam-opt")]
    public string? IpamOpt { get; set; }

    [CommandLongSwitch("ipv6")]
    public string? Ipv6 { get; set; }

    [CommandLongSwitch("label")]
    public string? Label { get; set; }

    [CommandLongSwitch("opt")]
    public string? Opt { get; set; }

    [CommandLongSwitch("scope")]
    public string? Scope { get; set; }

    [CommandLongSwitch("subnet")]
    public string? Subnet { get; set; }

}