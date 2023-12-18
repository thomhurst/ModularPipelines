using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "application-gateway", "rewrite-rule", "create")]
public record AzNetworkApplicationGatewayRewriteRuleCreateOptions(
[property: CommandSwitch("--gateway-name")] string GatewayName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--rule-set-name")] string RuleSetName
) : AzOptions
{
    [CommandSwitch("--conditions")]
    public string? Conditions { get; set; }

    [BooleanCommandSwitch("--enable-reroute")]
    public bool? EnableReroute { get; set; }

    [CommandSwitch("--modified-path")]
    public string? ModifiedPath { get; set; }

    [CommandSwitch("--modified-query-string")]
    public string? ModifiedQueryString { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--request-headers")]
    public string? RequestHeaders { get; set; }

    [CommandSwitch("--response-headers")]
    public string? ResponseHeaders { get; set; }

    [CommandSwitch("--sequence")]
    public string? Sequence { get; set; }
}