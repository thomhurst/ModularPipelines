using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("personalize", "update-metric-attribution")]
public record AwsPersonalizeUpdateMetricAttributionOptions : AwsOptions
{
    [CommandSwitch("--add-metrics")]
    public string[]? AddMetrics { get; set; }

    [CommandSwitch("--remove-metrics")]
    public string[]? RemoveMetrics { get; set; }

    [CommandSwitch("--metrics-output-config")]
    public string? MetricsOutputConfig { get; set; }

    [CommandSwitch("--metric-attribution-arn")]
    public string? MetricAttributionArn { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}