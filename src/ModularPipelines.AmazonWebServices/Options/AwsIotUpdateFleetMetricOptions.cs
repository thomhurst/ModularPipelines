using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "update-fleet-metric")]
public record AwsIotUpdateFleetMetricOptions(
[property: CommandSwitch("--metric-name")] string MetricName,
[property: CommandSwitch("--index-name")] string IndexName
) : AwsOptions
{
    [CommandSwitch("--query-string")]
    public string? QueryString { get; set; }

    [CommandSwitch("--aggregation-type")]
    public string? AggregationType { get; set; }

    [CommandSwitch("--period")]
    public int? Period { get; set; }

    [CommandSwitch("--aggregation-field")]
    public string? AggregationField { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--query-version")]
    public string? QueryVersion { get; set; }

    [CommandSwitch("--unit")]
    public string? Unit { get; set; }

    [CommandSwitch("--expected-version")]
    public long? ExpectedVersion { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}