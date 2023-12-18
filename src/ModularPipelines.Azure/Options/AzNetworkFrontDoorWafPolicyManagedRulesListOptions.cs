using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "front-door", "waf-policy", "managed-rules", "list")]
public record AzNetworkFrontDoorWafPolicyManagedRulesListOptions(
[property: CommandSwitch("--policy-name")] string PolicyName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;