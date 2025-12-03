using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vmware", "network-peerings", "create")]
public record GcloudVmwareNetworkPeeringsCreateOptions(
[property: CliArgument] string NetworkPeering,
[property: CliOption("--peer-network")] string PeerNetwork,
[property: CliOption("--peer-network-type")] string PeerNetworkType,
[property: CliOption("--vmware-engine-network")] string VmwareEngineNetwork
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliFlag("--exchange-subnet-routes")]
    public bool? ExchangeSubnetRoutes { get; set; }

    [CliFlag("--export-custom-routes")]
    public bool? ExportCustomRoutes { get; set; }

    [CliFlag("--export-custom-routes-with-public-ip")]
    public bool? ExportCustomRoutesWithPublicIp { get; set; }

    [CliFlag("--import-custom-routes")]
    public bool? ImportCustomRoutes { get; set; }

    [CliFlag("--import-custom-routes-with-public-ip")]
    public bool? ImportCustomRoutesWithPublicIp { get; set; }

    [CliOption("--peer-mtu")]
    public string? PeerMtu { get; set; }

    [CliOption("--peer-project")]
    public string? PeerProject { get; set; }
}