using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "vnet-gateway", "ipsec-policy", "list")]
public record AzNetworkVnetGatewayIpsecPolicyListOptions(
[property: CliOption("--gateway-name")] string GatewayName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;