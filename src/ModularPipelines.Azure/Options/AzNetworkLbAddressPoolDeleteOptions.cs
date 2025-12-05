using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "lb", "address-pool", "delete")]
public record AzNetworkLbAddressPoolDeleteOptions(
[property: CliOption("--address-pool-name")] string AddressPoolName,
[property: CliOption("--lb-name")] string LbName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}