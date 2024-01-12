using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vmware", "network-peerings", "describe")]
public record GcloudVmwareNetworkPeeringsDescribeOptions(
[property: PositionalArgument] string NetworkPeering
) : GcloudOptions;