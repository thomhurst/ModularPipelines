using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "nic", "ip-config", "address-pool", "remove")]
public record AzNetworkNicIpConfigAddressPoolRemoveOptions(
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

