using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connect", "get-metric-data-v2")]
public record AwsConnectGetMetricDataV2Options(
[property: CommandSwitch("--resource-arn")] string ResourceArn,
[property: CommandSwitch("--start-time")] long StartTime,
[property: CommandSwitch("--end-time")] long EndTime,
[property: CommandSwitch("--filters")] string[] Filters,
[property: CommandSwitch("--metrics")] string[] Metrics
) : AwsOptions
{
    [CommandSwitch("--interval")]
    public string? Interval { get; set; }

    [CommandSwitch("--groupings")]
    public string[]? Groupings { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}