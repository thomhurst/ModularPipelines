using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "application-gateway", "waf-policy", "custom-rule", "list")]
public record AzNetworkApplicationGatewayWafPolicyCustomRuleListOptions(
[property: CommandSwitch("--policy-name")] string PolicyName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;