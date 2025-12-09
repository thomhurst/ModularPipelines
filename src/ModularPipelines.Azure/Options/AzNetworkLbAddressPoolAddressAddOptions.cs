using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "lb", "address-pool", "address", "add")]
public record AzNetworkLbAddressPoolAddressAddOptions(
[property: CliOption("--ip-address")] string IpAddress,
[property: CliOption("--lb-name")] string LbName,
[property: CliOption("--name")] string Name,
[property: CliOption("--pool-name")] string PoolName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--admin-state")]
    public string? AdminState { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--subnet")]
    public string? Subnet { get; set; }

    [CliOption("--virtual-network")]
    public string? VirtualNetwork { get; set; }
}