using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "application-gateway", "waf-policy", "custom-rule", "match-condition", "add")]
public record AzNetworkApplicationGatewayWafPolicyCustomRuleMatchConditionAddOptions(
[property: CliOption("--match-variables")] string MatchVariables,
[property: CliOption("--name")] string Name,
[property: CliOption("--operator")] string Operator,
[property: CliOption("--policy-name")] string PolicyName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--index")]
    public string? Index { get; set; }

    [CliFlag("--negate")]
    public bool? Negate { get; set; }

    [CliOption("--transforms")]
    public string? Transforms { get; set; }

    [CliOption("--values")]
    public string? Values { get; set; }
}