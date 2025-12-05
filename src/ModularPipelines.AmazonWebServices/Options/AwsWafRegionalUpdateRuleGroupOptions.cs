using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("waf-regional", "update-rule-group")]
public record AwsWafRegionalUpdateRuleGroupOptions(
[property: CliOption("--rule-group-id")] string RuleGroupId,
[property: CliOption("--updates")] string[] Updates,
[property: CliOption("--change-token")] string ChangeToken
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}