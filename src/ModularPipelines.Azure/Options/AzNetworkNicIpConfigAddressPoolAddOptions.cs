using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "nic", "ip-config", "address-pool", "add")]
public record AzNetworkNicIpConfigAddressPoolAddOptions(
[property: CommandSwitch("--address-pool")] string AddressPool,
[property: CommandSwitch("--ip-config-name")] string IpConfigName,
[property: CommandSwitch("--nic-name")] string NicName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--gateway-name")]
    public string? GatewayName { get; set; }

    [CommandSwitch("--lb-name")]
    public string? LbName { get; set; }
}