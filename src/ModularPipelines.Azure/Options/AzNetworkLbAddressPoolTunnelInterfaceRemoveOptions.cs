using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "lb", "address-pool", "tunnel-interface", "remove")]
public record AzNetworkLbAddressPoolTunnelInterfaceRemoveOptions(
[property: CommandSwitch("--address-pool")] string AddressPool,
[property: CommandSwitch("--index")] string Index,
[property: CommandSwitch("--lb-name")] string LbName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }
}

