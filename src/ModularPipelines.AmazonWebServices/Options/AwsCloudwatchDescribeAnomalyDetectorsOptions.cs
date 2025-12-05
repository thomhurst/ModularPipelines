using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudwatch", "describe-anomaly-detectors")]
public record AwsCloudwatchDescribeAnomalyDetectorsOptions : AwsOptions
{
    [CliOption("--namespace")]
    public string? Namespace { get; set; }

    [CliOption("--metric-name")]
    public string? MetricName { get; set; }

    [CliOption("--dimensions")]
    public string[]? Dimensions { get; set; }

    [CliOption("--anomaly-detector-types")]
    public string[]? AnomalyDetectorTypes { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}