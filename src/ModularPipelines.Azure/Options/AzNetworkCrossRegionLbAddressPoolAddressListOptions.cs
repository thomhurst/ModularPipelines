using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "cross-region-lb", "address-pool", "address", "list")]
public record AzNetworkCrossRegionLbAddressPoolAddressListOptions(
[property: CommandSwitch("--lb-name")] string LbName,
[property: CommandSwitch("--pool-name")] string PoolName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;