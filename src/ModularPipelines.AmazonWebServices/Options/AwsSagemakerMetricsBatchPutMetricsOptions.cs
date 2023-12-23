using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker-metrics", "batch-put-metrics")]
public record AwsSagemakerMetricsBatchPutMetricsOptions(
[property: CommandSwitch("--trial-component-name")] string TrialComponentName,
[property: CommandSwitch("--metric-data")] string[] MetricData
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}