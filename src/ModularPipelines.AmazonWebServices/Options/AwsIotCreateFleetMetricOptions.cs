using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "create-fleet-metric")]
public record AwsIotCreateFleetMetricOptions(
[property: CliOption("--metric-name")] string MetricName,
[property: CliOption("--query-string")] string QueryString,
[property: CliOption("--aggregation-type")] string AggregationType,
[property: CliOption("--period")] int Period,
[property: CliOption("--aggregation-field")] string AggregationField
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--query-version")]
    public string? QueryVersion { get; set; }

    [CliOption("--index-name")]
    public string? IndexName { get; set; }

    [CliOption("--unit")]
    public string? Unit { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}