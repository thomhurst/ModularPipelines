using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "lb", "address-pool", "delete")]
public record AzNetworkLbAddressPoolDeleteOptions(
[property: CommandSwitch("--address-pool-name")] string AddressPoolName,
[property: CommandSwitch("--lb-name")] string LbName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }
}