using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("waf-regional", "list-activated-rules-in-rule-group")]
public record AwsWafRegionalListActivatedRulesInRuleGroupOptions : AwsOptions
{
    [CliOption("--rule-group-id")]
    public string? RuleGroupId { get; set; }

    [CliOption("--next-marker")]
    public string? NextMarker { get; set; }

    [CliOption("--limit")]
    public int? Limit { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}