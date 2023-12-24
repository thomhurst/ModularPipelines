using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudwatch", "describe-anomaly-detectors")]
public record AwsCloudwatchDescribeAnomalyDetectorsOptions : AwsOptions
{
    [CommandSwitch("--namespace")]
    public string? Namespace { get; set; }

    [CommandSwitch("--metric-name")]
    public string? MetricName { get; set; }

    [CommandSwitch("--dimensions")]
    public string[]? Dimensions { get; set; }

    [CommandSwitch("--anomaly-detector-types")]
    public string[]? AnomalyDetectorTypes { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}