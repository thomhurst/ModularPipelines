using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "lb", "address-pool", "address", "remove")]
public record AzNetworkLbAddressPoolAddressRemoveOptions(
[property: CommandSwitch("--lb-name")] string LbName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--pool-name")] string PoolName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }
}