using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("autoscaling", "disable-metrics-collection")]
public record AwsAutoscalingDisableMetricsCollectionOptions(
[property: CliOption("--auto-scaling-group-name")] string AutoScalingGroupName
) : AwsOptions
{
    [CliOption("--metrics")]
    public string[]? Metrics { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}