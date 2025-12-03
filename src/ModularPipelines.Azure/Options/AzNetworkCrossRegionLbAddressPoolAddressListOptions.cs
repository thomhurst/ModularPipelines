using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "cross-region-lb", "address-pool", "address", "list")]
public record AzNetworkCrossRegionLbAddressPoolAddressListOptions(
[property: CliOption("--lb-name")] string LbName,
[property: CliOption("--pool-name")] string PoolName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;