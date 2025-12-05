using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "application-gateway", "rewrite-rule", "condition", "delete")]
public record AzNetworkApplicationGatewayRewriteRuleConditionDeleteOptions(
[property: CliOption("--gateway-name")] string GatewayName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--rule-name")] string RuleName,
[property: CliOption("--rule-set-name")] string RuleSetName,
[property: CliOption("--variable")] string Variable
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}