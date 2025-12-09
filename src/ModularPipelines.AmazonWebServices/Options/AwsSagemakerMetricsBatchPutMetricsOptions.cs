using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker-metrics", "batch-put-metrics")]
public record AwsSagemakerMetricsBatchPutMetricsOptions(
[property: CliOption("--trial-component-name")] string TrialComponentName,
[property: CliOption("--metric-data")] string[] MetricData
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}