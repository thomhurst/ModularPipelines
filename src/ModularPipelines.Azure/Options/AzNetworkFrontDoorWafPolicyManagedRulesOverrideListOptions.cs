using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "front-door", "waf-policy", "managed-rules", "override", "list")]
public record AzNetworkFrontDoorWafPolicyManagedRulesOverrideListOptions(
[property: CliOption("--policy-name")] string PolicyName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--type")] string Type
) : AzOptions;