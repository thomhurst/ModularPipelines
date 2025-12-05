using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lookoutmetrics", "create-metric-set")]
public record AwsLookoutmetricsCreateMetricSetOptions(
[property: CliOption("--anomaly-detector-arn")] string AnomalyDetectorArn,
[property: CliOption("--metric-set-name")] string MetricSetName,
[property: CliOption("--metric-list")] string[] MetricList,
[property: CliOption("--metric-source")] string MetricSource
) : AwsOptions
{
    [CliOption("--metric-set-description")]
    public string? MetricSetDescription { get; set; }

    [CliOption("--offset")]
    public int? Offset { get; set; }

    [CliOption("--timestamp-column")]
    public string? TimestampColumn { get; set; }

    [CliOption("--dimension-list")]
    public string[]? DimensionList { get; set; }

    [CliOption("--metric-set-frequency")]
    public string? MetricSetFrequency { get; set; }

    [CliOption("--timezone")]
    public string? Timezone { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--dimension-filter-list")]
    public string[]? DimensionFilterList { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}