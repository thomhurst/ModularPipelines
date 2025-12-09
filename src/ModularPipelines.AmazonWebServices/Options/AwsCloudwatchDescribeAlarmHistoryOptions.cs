using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudwatch", "describe-alarm-history")]
public record AwsCloudwatchDescribeAlarmHistoryOptions : AwsOptions
{
    [CliOption("--alarm-name")]
    public string? AlarmName { get; set; }

    [CliOption("--alarm-types")]
    public string[]? AlarmTypes { get; set; }

    [CliOption("--history-item-type")]
    public string? HistoryItemType { get; set; }

    [CliOption("--start-date")]
    public long? StartDate { get; set; }

    [CliOption("--end-date")]
    public long? EndDate { get; set; }

    [CliOption("--scan-by")]
    public string? ScanBy { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}