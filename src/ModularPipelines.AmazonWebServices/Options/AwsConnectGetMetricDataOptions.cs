using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "get-metric-data")]
public record AwsConnectGetMetricDataOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--start-time")] long StartTime,
[property: CliOption("--end-time")] long EndTime,
[property: CliOption("--filters")] string Filters,
[property: CliOption("--historical-metrics")] string[] HistoricalMetrics
) : AwsOptions
{
    [CliOption("--groupings")]
    public string[]? Groupings { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}