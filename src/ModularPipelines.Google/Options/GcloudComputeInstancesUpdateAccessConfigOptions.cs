using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "instances", "update-access-config")]
public record GcloudComputeInstancesUpdateAccessConfigOptions(
[property: PositionalArgument] string InstanceName
) : GcloudOptions
{
    [CommandSwitch("--network-interface")]
    public string? NetworkInterface { get; set; }

    [CommandSwitch("--zone")]
    public string? Zone { get; set; }

    [BooleanCommandSwitch("--no-ipv6-public-ptr")]
    public bool? NoIpv6PublicPtr { get; set; }

    [CommandSwitch("--ipv6-public-ptr-domain")]
    public string? Ipv6PublicPtrDomain { get; set; }

    [BooleanCommandSwitch("--public-ptr")]
    public bool? PublicPtr { get; set; }

    [BooleanCommandSwitch("--no-public-ptr")]
    public bool? NoPublicPtr { get; set; }

    [CommandSwitch("--public-ptr-domain")]
    public string? PublicPtrDomain { get; set; }

    [BooleanCommandSwitch("--no-public-ptr-domain")]
    public bool? NoPublicPtrDomain { get; set; }
}