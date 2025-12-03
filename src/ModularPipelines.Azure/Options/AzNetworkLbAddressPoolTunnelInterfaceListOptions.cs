using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "lb", "address-pool", "tunnel-interface", "list")]
public record AzNetworkLbAddressPoolTunnelInterfaceListOptions(
[property: CliOption("--address-pool")] string AddressPool,
[property: CliOption("--lb-name")] string LbName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;