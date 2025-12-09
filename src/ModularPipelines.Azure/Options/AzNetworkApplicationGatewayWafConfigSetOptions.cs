using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "application-gateway", "waf-config", "set")]
public record AzNetworkApplicationGatewayWafConfigSetOptions(
[property: CliFlag("--enabled")] bool Enabled
) : AzOptions
{
    [CliFlag("--disabled-rule-groups")]
    public bool? DisabledRuleGroups { get; set; }

    [CliFlag("--disabled-rules")]
    public bool? DisabledRules { get; set; }

    [CliOption("--exclusion")]
    public string? Exclusion { get; set; }

    [CliOption("--file-upload-limit")]
    public string? FileUploadLimit { get; set; }

    [CliOption("--firewall-mode")]
    public string? FirewallMode { get; set; }

    [CliOption("--gateway-name")]
    public string? GatewayName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--max-request-body-size")]
    public string? MaxRequestBodySize { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliFlag("--request-body-check")]
    public bool? RequestBodyCheck { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--rule-set-type")]
    public string? RuleSetType { get; set; }

    [CliOption("--rule-set-version")]
    public string? RuleSetVersion { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}