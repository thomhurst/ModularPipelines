using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("waf", "update-rate-based-rule")]
public record AwsWafUpdateRateBasedRuleOptions(
[property: CliOption("--rule-id")] string RuleId,
[property: CliOption("--change-token")] string ChangeToken,
[property: CliOption("--updates")] string[] Updates,
[property: CliOption("--rate-limit")] long RateLimit
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}