using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "application-gateway", "url-path-map", "show")]
public record AzNetworkApplicationGatewayUrlPathMapShowOptions(
[property: CommandSwitch("--gateway-name")] string GatewayName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--add")]
    public string? Add { get; set; }

    [CommandSwitch("--default-address-pool")]
    public string? DefaultAddressPool { get; set; }

    [CommandSwitch("--default-http-settings")]
    public string? DefaultHttpSettings { get; set; }

    [CommandSwitch("--default-redirect-config")]
    public string? DefaultRedirectConfig { get; set; }

    [CommandSwitch("--default-rewrite-rule-set")]
    public string? DefaultRewriteRuleSet { get; set; }

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--rules")]
    public string? Rules { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }
}