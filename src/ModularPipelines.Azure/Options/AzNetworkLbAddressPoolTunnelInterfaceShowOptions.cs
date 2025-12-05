using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "lb", "address-pool", "tunnel-interface", "show")]
public record AzNetworkLbAddressPoolTunnelInterfaceShowOptions(
[property: CliOption("--address-pool")] string AddressPool,
[property: CliOption("--index")] string Index,
[property: CliOption("--lb-name")] string LbName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;