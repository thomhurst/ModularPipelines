using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connect", "get-metric-data")]
public record AwsConnectGetMetricDataOptions(
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--start-time")] long StartTime,
[property: CommandSwitch("--end-time")] long EndTime,
[property: CommandSwitch("--filters")] string Filters,
[property: CommandSwitch("--historical-metrics")] string[] HistoricalMetrics
) : AwsOptions
{
    [CommandSwitch("--groupings")]
    public string[]? Groupings { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}