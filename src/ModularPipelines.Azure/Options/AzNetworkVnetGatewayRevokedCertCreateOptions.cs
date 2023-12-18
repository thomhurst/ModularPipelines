using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "vnet-gateway", "revoked-cert", "create")]
public record AzNetworkVnetGatewayRevokedCertCreateOptions(
[property: CommandSwitch("--gateway-name")] string GatewayName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--thumbprint")] string Thumbprint
) : AzOptions
{
    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }
}