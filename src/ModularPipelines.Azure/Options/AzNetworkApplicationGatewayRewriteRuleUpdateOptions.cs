using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "application-gateway", "rewrite-rule", "update")]
public record AzNetworkApplicationGatewayRewriteRuleUpdateOptions(
[property: CommandSwitch("--gateway-name")] string GatewayName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--rule-set-name")] string RuleSetName
) : AzOptions
{
    [CommandSwitch("--add")]
    public string? Add { get; set; }

    [CommandSwitch("--conditions")]
    public string? Conditions { get; set; }

    [BooleanCommandSwitch("--enable-reroute")]
    public bool? EnableReroute { get; set; }

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [CommandSwitch("--modified-path")]
    public string? ModifiedPath { get; set; }

    [CommandSwitch("--modified-query-string")]
    public string? ModifiedQueryString { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--request-headers")]
    public string? RequestHeaders { get; set; }

    [CommandSwitch("--response-headers")]
    public string? ResponseHeaders { get; set; }

    [CommandSwitch("--sequence")]
    public string? Sequence { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }
}