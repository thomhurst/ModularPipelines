using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lookoutmetrics", "update-metric-set")]
public record AwsLookoutmetricsUpdateMetricSetOptions(
[property: CliOption("--metric-set-arn")] string MetricSetArn
) : AwsOptions
{
    [CliOption("--metric-set-description")]
    public string? MetricSetDescription { get; set; }

    [CliOption("--metric-list")]
    public string[]? MetricList { get; set; }

    [CliOption("--offset")]
    public int? Offset { get; set; }

    [CliOption("--timestamp-column")]
    public string? TimestampColumn { get; set; }

    [CliOption("--dimension-list")]
    public string[]? DimensionList { get; set; }

    [CliOption("--metric-set-frequency")]
    public string? MetricSetFrequency { get; set; }

    [CliOption("--metric-source")]
    public string? MetricSource { get; set; }

    [CliOption("--dimension-filter-list")]
    public string[]? DimensionFilterList { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}