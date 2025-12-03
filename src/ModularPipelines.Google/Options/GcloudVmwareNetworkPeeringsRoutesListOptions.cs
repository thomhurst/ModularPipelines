using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vmware", "network-peerings", "routes", "list")]
public record GcloudVmwareNetworkPeeringsRoutesListOptions(
[property: CliOption("--network-peering")] string NetworkPeering
) : GcloudOptions;