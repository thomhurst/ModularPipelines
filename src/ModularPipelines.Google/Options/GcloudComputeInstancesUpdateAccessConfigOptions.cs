using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "instances", "update-access-config")]
public record GcloudComputeInstancesUpdateAccessConfigOptions(
[property: CliArgument] string InstanceName
) : GcloudOptions
{
    [CliOption("--network-interface")]
    public string? NetworkInterface { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }

    [CliFlag("--no-ipv6-public-ptr")]
    public bool? NoIpv6PublicPtr { get; set; }

    [CliOption("--ipv6-public-ptr-domain")]
    public string? Ipv6PublicPtrDomain { get; set; }

    [CliFlag("--public-ptr")]
    public bool? PublicPtr { get; set; }

    [CliFlag("--no-public-ptr")]
    public bool? NoPublicPtr { get; set; }

    [CliOption("--public-ptr-domain")]
    public string? PublicPtrDomain { get; set; }

    [CliFlag("--no-public-ptr-domain")]
    public bool? NoPublicPtrDomain { get; set; }
}