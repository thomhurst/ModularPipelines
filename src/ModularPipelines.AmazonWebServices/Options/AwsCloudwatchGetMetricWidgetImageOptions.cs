using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudwatch", "get-metric-widget-image")]
public record AwsCloudwatchGetMetricWidgetImageOptions(
[property: CommandSwitch("--metric-widget")] string MetricWidget
) : AwsOptions
{
    [CommandSwitch("--output-format")]
    public string? OutputFormat { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}