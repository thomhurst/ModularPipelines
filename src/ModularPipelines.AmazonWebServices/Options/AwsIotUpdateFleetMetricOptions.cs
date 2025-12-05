using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "update-fleet-metric")]
public record AwsIotUpdateFleetMetricOptions(
[property: CliOption("--metric-name")] string MetricName,
[property: CliOption("--index-name")] string IndexName
) : AwsOptions
{
    [CliOption("--query-string")]
    public string? QueryString { get; set; }

    [CliOption("--aggregation-type")]
    public string? AggregationType { get; set; }

    [CliOption("--period")]
    public int? Period { get; set; }

    [CliOption("--aggregation-field")]
    public string? AggregationField { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--query-version")]
    public string? QueryVersion { get; set; }

    [CliOption("--unit")]
    public string? Unit { get; set; }

    [CliOption("--expected-version")]
    public long? ExpectedVersion { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}