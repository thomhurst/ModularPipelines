using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudwatch", "get-insight-rule-report")]
public record AwsCloudwatchGetInsightRuleReportOptions(
[property: CliOption("--rule-name")] string RuleName,
[property: CliOption("--start-time")] long StartTime,
[property: CliOption("--end-time")] long EndTime,
[property: CliOption("--period")] int Period
) : AwsOptions
{
    [CliOption("--max-contributor-count")]
    public int? MaxContributorCount { get; set; }

    [CliOption("--metrics")]
    public string[]? Metrics { get; set; }

    [CliOption("--order-by")]
    public string? OrderBy { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}