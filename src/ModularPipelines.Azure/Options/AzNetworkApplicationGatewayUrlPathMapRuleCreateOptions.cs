using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "application-gateway", "url-path-map", "rule", "create")]
public record AzNetworkApplicationGatewayUrlPathMapRuleCreateOptions(
[property: CommandSwitch("--gateway-name")] string GatewayName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--path-map-name")] string PathMapName,
[property: CommandSwitch("--paths")] string Paths,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--address-pool")]
    public string? AddressPool { get; set; }

    [CommandSwitch("--http-settings")]
    public string? HttpSettings { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--redirect-config")]
    public string? RedirectConfig { get; set; }

    [CommandSwitch("--rewrite-rule-set")]
    public string? RewriteRuleSet { get; set; }

    [CommandSwitch("--waf-policy")]
    public string? WafPolicy { get; set; }
}

