using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "front-door", "waf-policy", "managed-rule-definition", "list")]
public record AzNetworkFrontDoorWafPolicyManagedRuleDefinitionListOptions : AzOptions;