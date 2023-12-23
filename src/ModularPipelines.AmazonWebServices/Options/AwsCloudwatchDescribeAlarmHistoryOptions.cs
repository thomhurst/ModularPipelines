using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudwatch", "describe-alarm-history")]
public record AwsCloudwatchDescribeAlarmHistoryOptions : AwsOptions
{
    [CommandSwitch("--alarm-name")]
    public string? AlarmName { get; set; }

    [CommandSwitch("--alarm-types")]
    public string[]? AlarmTypes { get; set; }

    [CommandSwitch("--history-item-type")]
    public string? HistoryItemType { get; set; }

    [CommandSwitch("--start-date")]
    public long? StartDate { get; set; }

    [CommandSwitch("--end-date")]
    public long? EndDate { get; set; }

    [CommandSwitch("--scan-by")]
    public string? ScanBy { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}