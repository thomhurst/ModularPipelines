using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "application-gateway", "waf-policy", "managed-rule", "exclusion", "list")]
public record AzNetworkApplicationGatewayWafPolicyManagedRuleExclusionListOptions(
[property: CliOption("--policy-name")] string PolicyName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;