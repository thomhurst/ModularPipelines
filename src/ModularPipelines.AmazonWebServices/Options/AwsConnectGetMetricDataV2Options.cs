using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "get-metric-data-v2")]
public record AwsConnectGetMetricDataV2Options(
[property: CliOption("--resource-arn")] string ResourceArn,
[property: CliOption("--start-time")] long StartTime,
[property: CliOption("--end-time")] long EndTime,
[property: CliOption("--filters")] string[] Filters,
[property: CliOption("--metrics")] string[] Metrics
) : AwsOptions
{
    [CliOption("--interval")]
    public string? Interval { get; set; }

    [CliOption("--groupings")]
    public string[]? Groupings { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}