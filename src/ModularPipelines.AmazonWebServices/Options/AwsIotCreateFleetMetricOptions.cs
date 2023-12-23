using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "create-fleet-metric")]
public record AwsIotCreateFleetMetricOptions(
[property: CommandSwitch("--metric-name")] string MetricName,
[property: CommandSwitch("--query-string")] string QueryString,
[property: CommandSwitch("--aggregation-type")] string AggregationType,
[property: CommandSwitch("--period")] int Period,
[property: CommandSwitch("--aggregation-field")] string AggregationField
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--query-version")]
    public string? QueryVersion { get; set; }

    [CommandSwitch("--index-name")]
    public string? IndexName { get; set; }

    [CommandSwitch("--unit")]
    public string? Unit { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}