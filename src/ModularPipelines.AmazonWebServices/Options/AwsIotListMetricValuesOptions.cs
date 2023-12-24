using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "list-metric-values")]
public record AwsIotListMetricValuesOptions(
[property: CommandSwitch("--thing-name")] string ThingName,
[property: CommandSwitch("--metric-name")] string MetricName,
[property: CommandSwitch("--start-time")] long StartTime,
[property: CommandSwitch("--end-time")] long EndTime
) : AwsOptions
{
    [CommandSwitch("--dimension-name")]
    public string? DimensionName { get; set; }

    [CommandSwitch("--dimension-value-operator")]
    public string? DimensionValueOperator { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}