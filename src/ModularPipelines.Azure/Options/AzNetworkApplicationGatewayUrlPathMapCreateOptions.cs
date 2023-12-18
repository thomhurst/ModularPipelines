using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "application-gateway", "url-path-map", "create")]
public record AzNetworkApplicationGatewayUrlPathMapCreateOptions(
[property: CommandSwitch("--gateway-name")] string GatewayName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--paths")] string Paths,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--address-pool")]
    public string? AddressPool { get; set; }

    [CommandSwitch("--default-address-pool")]
    public string? DefaultAddressPool { get; set; }

    [CommandSwitch("--default-http-settings")]
    public string? DefaultHttpSettings { get; set; }

    [CommandSwitch("--default-redirect-config")]
    public string? DefaultRedirectConfig { get; set; }

    [CommandSwitch("--default-rewrite-rule-set")]
    public string? DefaultRewriteRuleSet { get; set; }

    [CommandSwitch("--http-settings")]
    public string? HttpSettings { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--redirect-config")]
    public string? RedirectConfig { get; set; }

    [CommandSwitch("--rewrite-rule-set")]
    public string? RewriteRuleSet { get; set; }

    [CommandSwitch("--rule-name")]
    public string? RuleName { get; set; }

    [CommandSwitch("--rules")]
    public string? Rules { get; set; }

    [CommandSwitch("--waf-policy")]
    public string? WafPolicy { get; set; }
}