using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vmware", "network-peerings", "create")]
public record GcloudVmwareNetworkPeeringsCreateOptions(
[property: PositionalArgument] string NetworkPeering,
[property: CommandSwitch("--peer-network")] string PeerNetwork,
[property: CommandSwitch("--peer-network-type")] string PeerNetworkType,
[property: CommandSwitch("--vmware-engine-network")] string VmwareEngineNetwork
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [BooleanCommandSwitch("--exchange-subnet-routes")]
    public bool? ExchangeSubnetRoutes { get; set; }

    [BooleanCommandSwitch("--export-custom-routes")]
    public bool? ExportCustomRoutes { get; set; }

    [BooleanCommandSwitch("--export-custom-routes-with-public-ip")]
    public bool? ExportCustomRoutesWithPublicIp { get; set; }

    [BooleanCommandSwitch("--import-custom-routes")]
    public bool? ImportCustomRoutes { get; set; }

    [BooleanCommandSwitch("--import-custom-routes-with-public-ip")]
    public bool? ImportCustomRoutesWithPublicIp { get; set; }

    [CommandSwitch("--peer-mtu")]
    public string? PeerMtu { get; set; }

    [CommandSwitch("--peer-project")]
    public string? PeerProject { get; set; }
}