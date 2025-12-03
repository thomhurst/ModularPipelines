using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vmware", "network-policies", "create")]
public record GcloudVmwareNetworkPoliciesCreateOptions(
[property: CliArgument] string NetworkPolicy,
[property: CliArgument] string Location,
[property: CliOption("--edge-services-cidr")] string EdgeServicesCidr,
[property: CliOption("--vmware-engine-network")] string VmwareEngineNetwork
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliFlag("--external-ip-access")]
    public bool? ExternalIpAccess { get; set; }

    [CliFlag("--internet-access")]
    public bool? InternetAccess { get; set; }
}