using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "vnet-gateway", "aad", "assign")]
public record AzNetworkVnetGatewayAadAssignOptions(
[property: CliOption("--audience")] string Audience,
[property: CliOption("--gateway-name")] string GatewayName,
[property: CliOption("--issuer")] string Issuer,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--tenant")] string Tenant
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}