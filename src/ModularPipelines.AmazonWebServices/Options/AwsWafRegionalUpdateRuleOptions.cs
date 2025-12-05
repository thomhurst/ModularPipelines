using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("waf-regional", "update-rule")]
public record AwsWafRegionalUpdateRuleOptions(
[property: CliOption("--rule-id")] string RuleId,
[property: CliOption("--change-token")] string ChangeToken,
[property: CliOption("--updates")] string[] Updates
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}