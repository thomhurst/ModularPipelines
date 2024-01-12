using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vmware", "network-policies", "update")]
public record GcloudVmwareNetworkPoliciesUpdateOptions(
[property: PositionalArgument] string NetworkPolicy,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--edge-services-cidr")]
    public string? EdgeServicesCidr { get; set; }

    [BooleanCommandSwitch("--external-ip-access")]
    public bool? ExternalIpAccess { get; set; }

    [BooleanCommandSwitch("--internet-access")]
    public bool? InternetAccess { get; set; }
}