using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "application-gateway", "waf-policy", "custom-rule", "match-condition", "remove")]
public record AzNetworkApplicationGatewayWafPolicyCustomRuleMatchConditionRemoveOptions(
[property: CommandSwitch("--index")] string Index,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--policy-name")] string PolicyName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;