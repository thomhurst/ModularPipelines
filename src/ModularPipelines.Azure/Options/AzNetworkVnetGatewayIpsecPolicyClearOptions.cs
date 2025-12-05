using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "vnet-gateway", "ipsec-policy", "clear")]
public record AzNetworkVnetGatewayIpsecPolicyClearOptions(
[property: CliOption("--gateway-name")] string GatewayName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}