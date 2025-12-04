using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "nic", "ip-config", "address-pool", "add")]
public record AzNetworkNicIpConfigAddressPoolAddOptions(
[property: CliOption("--address-pool")] string AddressPool,
[property: CliOption("--ip-config-name")] string IpConfigName,
[property: CliOption("--nic-name")] string NicName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--gateway-name")]
    public string? GatewayName { get; set; }

    [CliOption("--lb-name")]
    public string? LbName { get; set; }
}