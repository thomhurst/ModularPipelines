using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "application-gateway", "waf-config", "set")]
public record AzNetworkApplicationGatewayWafConfigSetOptions(
[property: BooleanCommandSwitch("--enabled")] bool Enabled
) : AzOptions
{
    [BooleanCommandSwitch("--disabled-rule-groups")]
    public bool? DisabledRuleGroups { get; set; }

    [BooleanCommandSwitch("--disabled-rules")]
    public bool? DisabledRules { get; set; }

    [CommandSwitch("--exclusion")]
    public string? Exclusion { get; set; }

    [CommandSwitch("--file-upload-limit")]
    public string? FileUploadLimit { get; set; }

    [CommandSwitch("--firewall-mode")]
    public string? FirewallMode { get; set; }

    [CommandSwitch("--gateway-name")]
    public string? GatewayName { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--max-request-body-size")]
    public string? MaxRequestBodySize { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [BooleanCommandSwitch("--request-body-check")]
    public bool? RequestBodyCheck { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--rule-set-type")]
    public string? RuleSetType { get; set; }

    [CommandSwitch("--rule-set-version")]
    public string? RuleSetVersion { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}

