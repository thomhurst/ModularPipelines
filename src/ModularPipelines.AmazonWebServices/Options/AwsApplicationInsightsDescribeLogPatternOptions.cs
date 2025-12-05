using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("application-insights", "describe-log-pattern")]
public record AwsApplicationInsightsDescribeLogPatternOptions(
[property: CliOption("--resource-group-name")] string ResourceGroupName,
[property: CliOption("--pattern-set-name")] string PatternSetName,
[property: CliOption("--pattern-name")] string PatternName
) : AwsOptions
{
    [CliOption("--account-id")]
    public string? AccountId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}