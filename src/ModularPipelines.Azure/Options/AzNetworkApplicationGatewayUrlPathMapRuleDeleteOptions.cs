using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "application-gateway", "url-path-map", "rule", "delete")]
public record AzNetworkApplicationGatewayUrlPathMapRuleDeleteOptions(
[property: CommandSwitch("--gateway-name")] string GatewayName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--path-map-name")] string PathMapName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }
}