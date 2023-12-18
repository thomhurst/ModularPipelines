using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "vnet-gateway", "aad", "assign")]
public record AzNetworkVnetGatewayAadAssignOptions(
[property: CommandSwitch("--audience")] string Audience,
[property: CommandSwitch("--gateway-name")] string GatewayName,
[property: CommandSwitch("--issuer")] string Issuer,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--tenant")] string Tenant
) : AzOptions
{
    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }
}

