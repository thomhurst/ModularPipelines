using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lookoutmetrics", "create-metric-set")]
public record AwsLookoutmetricsCreateMetricSetOptions(
[property: CommandSwitch("--anomaly-detector-arn")] string AnomalyDetectorArn,
[property: CommandSwitch("--metric-set-name")] string MetricSetName,
[property: CommandSwitch("--metric-list")] string[] MetricList,
[property: CommandSwitch("--metric-source")] string MetricSource
) : AwsOptions
{
    [CommandSwitch("--metric-set-description")]
    public string? MetricSetDescription { get; set; }

    [CommandSwitch("--offset")]
    public int? Offset { get; set; }

    [CommandSwitch("--timestamp-column")]
    public string? TimestampColumn { get; set; }

    [CommandSwitch("--dimension-list")]
    public string[]? DimensionList { get; set; }

    [CommandSwitch("--metric-set-frequency")]
    public string? MetricSetFrequency { get; set; }

    [CommandSwitch("--timezone")]
    public string? Timezone { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--dimension-filter-list")]
    public string[]? DimensionFilterList { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}