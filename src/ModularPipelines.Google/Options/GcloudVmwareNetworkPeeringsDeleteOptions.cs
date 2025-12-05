using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vmware", "network-peerings", "delete")]
public record GcloudVmwareNetworkPeeringsDeleteOptions(
[property: CliArgument] string NetworkPeering
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}