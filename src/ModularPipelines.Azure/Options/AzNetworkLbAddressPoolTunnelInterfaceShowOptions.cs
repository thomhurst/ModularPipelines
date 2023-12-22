using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "lb", "address-pool", "tunnel-interface", "show")]
public record AzNetworkLbAddressPoolTunnelInterfaceShowOptions(
[property: CommandSwitch("--address-pool")] string AddressPool,
[property: CommandSwitch("--index")] string Index,
[property: CommandSwitch("--lb-name")] string LbName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;