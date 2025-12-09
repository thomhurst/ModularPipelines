using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "lb", "address-pool", "address", "list")]
public record AzNetworkLbAddressPoolAddressListOptions(
[property: CliOption("--lb-name")] string LbName,
[property: CliOption("--pool-name")] string PoolName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;