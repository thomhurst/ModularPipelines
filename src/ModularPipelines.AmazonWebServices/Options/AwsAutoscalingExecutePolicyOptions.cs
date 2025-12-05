using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("autoscaling", "execute-policy")]
public record AwsAutoscalingExecutePolicyOptions(
[property: CliOption("--policy-name")] string PolicyName
) : AwsOptions
{
    [CliOption("--auto-scaling-group-name")]
    public string? AutoScalingGroupName { get; set; }

    [CliOption("--metric-value")]
    public double? MetricValue { get; set; }

    [CliOption("--breach-threshold")]
    public double? BreachThreshold { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}