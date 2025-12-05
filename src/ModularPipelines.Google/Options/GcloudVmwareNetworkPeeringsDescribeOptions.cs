using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vmware", "network-peerings", "describe")]
public record GcloudVmwareNetworkPeeringsDescribeOptions(
[property: CliArgument] string NetworkPeering
) : GcloudOptions;