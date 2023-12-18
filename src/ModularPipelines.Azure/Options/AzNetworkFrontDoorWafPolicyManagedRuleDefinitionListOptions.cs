using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "front-door", "waf-policy", "managed-rule-definition", "list")]
public record AzNetworkFrontDoorWafPolicyManagedRuleDefinitionListOptions : AzOptions
{
}