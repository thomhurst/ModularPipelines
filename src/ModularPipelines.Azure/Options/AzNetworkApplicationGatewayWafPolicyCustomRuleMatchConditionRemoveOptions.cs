using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "application-gateway", "waf-policy", "custom-rule", "match-condition", "remove")]
public record AzNetworkApplicationGatewayWafPolicyCustomRuleMatchConditionRemoveOptions(
[property: CliOption("--index")] string Index,
[property: CliOption("--name")] string Name,
[property: CliOption("--policy-name")] string PolicyName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;