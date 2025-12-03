using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vmware", "network-policies", "update")]
public record GcloudVmwareNetworkPoliciesUpdateOptions(
[property: CliArgument] string NetworkPolicy,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--edge-services-cidr")]
    public string? EdgeServicesCidr { get; set; }

    [CliFlag("--external-ip-access")]
    public bool? ExternalIpAccess { get; set; }

    [CliFlag("--internet-access")]
    public bool? InternetAccess { get; set; }
}