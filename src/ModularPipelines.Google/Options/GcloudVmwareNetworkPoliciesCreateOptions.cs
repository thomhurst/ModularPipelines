using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vmware", "network-policies", "create")]
public record GcloudVmwareNetworkPoliciesCreateOptions(
[property: PositionalArgument] string NetworkPolicy,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--edge-services-cidr")] string EdgeServicesCidr,
[property: CommandSwitch("--vmware-engine-network")] string VmwareEngineNetwork
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [BooleanCommandSwitch("--external-ip-access")]
    public bool? ExternalIpAccess { get; set; }

    [BooleanCommandSwitch("--internet-access")]
    public bool? InternetAccess { get; set; }
}