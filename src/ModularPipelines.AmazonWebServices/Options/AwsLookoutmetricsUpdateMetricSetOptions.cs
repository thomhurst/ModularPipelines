using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lookoutmetrics", "update-metric-set")]
public record AwsLookoutmetricsUpdateMetricSetOptions(
[property: CommandSwitch("--metric-set-arn")] string MetricSetArn
) : AwsOptions
{
    [CommandSwitch("--metric-set-description")]
    public string? MetricSetDescription { get; set; }

    [CommandSwitch("--metric-list")]
    public string[]? MetricList { get; set; }

    [CommandSwitch("--offset")]
    public int? Offset { get; set; }

    [CommandSwitch("--timestamp-column")]
    public string? TimestampColumn { get; set; }

    [CommandSwitch("--dimension-list")]
    public string[]? DimensionList { get; set; }

    [CommandSwitch("--metric-set-frequency")]
    public string? MetricSetFrequency { get; set; }

    [CommandSwitch("--metric-source")]
    public string? MetricSource { get; set; }

    [CommandSwitch("--dimension-filter-list")]
    public string[]? DimensionFilterList { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}