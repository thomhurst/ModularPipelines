using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vmware", "network-peerings", "routes", "list")]
public record GcloudVmwareNetworkPeeringsRoutesListOptions(
[property: CommandSwitch("--network-peering")] string NetworkPeering
) : GcloudOptions;