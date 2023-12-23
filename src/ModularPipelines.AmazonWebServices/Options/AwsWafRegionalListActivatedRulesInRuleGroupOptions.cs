using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("waf-regional", "list-activated-rules-in-rule-group")]
public record AwsWafRegionalListActivatedRulesInRuleGroupOptions : AwsOptions
{
    [CommandSwitch("--rule-group-id")]
    public string? RuleGroupId { get; set; }

    [CommandSwitch("--next-marker")]
    public string? NextMarker { get; set; }

    [CommandSwitch("--limit")]
    public int? Limit { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}