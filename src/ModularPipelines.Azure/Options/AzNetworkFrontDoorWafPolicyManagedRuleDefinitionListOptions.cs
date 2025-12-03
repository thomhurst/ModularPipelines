using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "front-door", "waf-policy", "managed-rule-definition", "list")]
public record AzNetworkFrontDoorWafPolicyManagedRuleDefinitionListOptions : AzOptions;