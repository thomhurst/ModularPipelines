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

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual string? Network { get; set; }

    [CliOption("--attachable")]
    public virtual string? Attachable { get; set; }

    [CliOption("--aux-address")]
    public virtual string? AuxAddress { get; set; }

    [CliOption("--config-from")]
    public virtual string? ConfigFrom { get; set; }

    [CliOption("--config-only")]
    public virtual string? ConfigOnly { get; set; }

    [CliOption("--driver")]
    public virtual string? Driver { get; set; }

    [CliOption("--gateway")]
    public virtual string? Gateway { get; set; }

    [CliOption("--ingress")]
    public virtual string? Ingress { get; set; }

    [CliOption("--internal")]
    public virtual string? Internal { get; set; }

    [CliOption("--ip-range")]
    public virtual string? IpRange { get; set; }

    [CliOption("--ipam-driver")]
    public virtual string? IpamDriver { get; set; }

    [CliOption("--ipam-opt")]
    public virtual string? IpamOpt { get; set; }

    [CliOption("--ipv6")]
    public virtual string? Ipv6 { get; set; }

    [CliOption("--label")]
    public virtual string? Label { get; set; }

    [CliOption("--opt")]
    public virtual string? Opt { get; set; }

    [CliOption("--scope")]
    public virtual string? Scope { get; set; }

    [CliOption("--subnet")]
    public virtual string? Subnet { get; set; }
}
