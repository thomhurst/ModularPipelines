using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "front-door", "waf-policy", "managed-rule-definition", "list")]
public record AzNetworkFrontDoorWafPolicyManagedRuleDefinitionListOptions : AzOptions;