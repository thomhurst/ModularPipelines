using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudwatch", "get-metric-widget-image")]
public record AwsCloudwatchGetMetricWidgetImageOptions(
[property: CliOption("--metric-widget")] string MetricWidget
) : AwsOptions
{
    [CliOption("--output-format")]
    public string? OutputFormat { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}