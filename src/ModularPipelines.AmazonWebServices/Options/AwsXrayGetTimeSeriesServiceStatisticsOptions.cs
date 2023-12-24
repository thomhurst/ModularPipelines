using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("xray", "get-time-series-service-statistics")]
public record AwsXrayGetTimeSeriesServiceStatisticsOptions(
[property: CommandSwitch("--start-time")] long StartTime,
[property: CommandSwitch("--end-time")] long EndTime
) : AwsOptions
{
    [CommandSwitch("--group-name")]
    public string? GroupName { get; set; }

    [CommandSwitch("--group-arn")]
    public string? GroupArn { get; set; }

    [CommandSwitch("--entity-selector-expression")]
    public string? EntitySelectorExpression { get; set; }

    [CommandSwitch("--period")]
    public int? Period { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}