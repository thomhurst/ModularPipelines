using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerNetworkCreateOptions : DockerOptions
{
    public DockerNetworkCreateOptions(
        string network
    )
    {
        CommandParts = ["network", "create"];

        Network = network;
    }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Network { get; set; }

    [CommandSwitch("--attachable")]
    public virtual string? Attachable { get; set; }

    [CommandSwitch("--aux-address")]
    public virtual string? AuxAddress { get; set; }

    [CommandSwitch("--config-from")]
    public virtual string? ConfigFrom { get; set; }

    [CommandSwitch("--config-only")]
    public virtual string? ConfigOnly { get; set; }

    [CommandSwitch("--driver")]
    public virtual string? Driver { get; set; }

    [CommandSwitch("--gateway")]
    public virtual string? Gateway { get; set; }

    [CommandSwitch("--ingress")]
    public virtual string? Ingress { get; set; }

    [CommandSwitch("--internal")]
    public virtual string? Internal { get; set; }

    [CommandSwitch("--ip-range")]
    public virtual string? IpRange { get; set; }

    [CommandSwitch("--ipam-driver")]
    public virtual string? IpamDriver { get; set; }

    [CommandSwitch("--ipam-opt")]
    public virtual string? IpamOpt { get; set; }

    [CommandSwitch("--ipv6")]
    public virtual string? Ipv6 { get; set; }

    [CommandSwitch("--label")]
    public virtual string? Label { get; set; }

    [CommandSwitch("--opt")]
    public virtual string? Opt { get; set; }

    [CommandSwitch("--scope")]
    public virtual string? Scope { get; set; }

    [CommandSwitch("--subnet")]
    public virtual string? Subnet { get; set; }
}
