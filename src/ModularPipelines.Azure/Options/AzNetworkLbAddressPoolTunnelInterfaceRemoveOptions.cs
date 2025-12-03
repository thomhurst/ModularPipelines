using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "lb", "address-pool", "tunnel-interface", "remove")]
public record AzNetworkLbAddressPoolTunnelInterfaceRemoveOptions(
[property: CliOption("--address-pool")] string AddressPool,
[property: CliOption("--index")] string Index,
[property: CliOption("--lb-name")] string LbName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}