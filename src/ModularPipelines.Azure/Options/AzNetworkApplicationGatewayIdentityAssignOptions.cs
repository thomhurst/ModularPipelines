using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "application-gateway", "identity", "assign")]
public record AzNetworkApplicationGatewayIdentityAssignOptions(
[property: CommandSwitch("--gateway-name")] string GatewayName,
[property: CommandSwitch("--identity")] string Identity,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }
}