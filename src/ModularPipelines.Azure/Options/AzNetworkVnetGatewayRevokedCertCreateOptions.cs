using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "vnet-gateway", "revoked-cert", "create")]
public record AzNetworkVnetGatewayRevokedCertCreateOptions(
[property: CliOption("--gateway-name")] string GatewayName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--thumbprint")] string Thumbprint
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}