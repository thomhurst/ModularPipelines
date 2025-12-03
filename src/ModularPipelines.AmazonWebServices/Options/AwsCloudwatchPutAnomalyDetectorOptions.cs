using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudwatch", "put-anomaly-detector")]
public record AwsCloudwatchPutAnomalyDetectorOptions : AwsOptions
{
    [CliOption("--namespace")]
    public string? Namespace { get; set; }

    [CliOption("--metric-name")]
    public string? MetricName { get; set; }

    [CliOption("--dimensions")]
    public string[]? Dimensions { get; set; }

    [CliOption("--stat")]
    public string? Stat { get; set; }

    [CliOption("--configuration")]
    public string? Configuration { get; set; }

    [CliOption("--single-metric-anomaly-detector")]
    public string? SingleMetricAnomalyDetector { get; set; }

    [CliOption("--metric-math-anomaly-detector")]
    public string? MetricMathAnomalyDetector { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}