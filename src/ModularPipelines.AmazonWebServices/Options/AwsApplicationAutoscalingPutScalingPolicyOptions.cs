using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("application-autoscaling", "put-scaling-policy")]
public record AwsApplicationAutoscalingPutScalingPolicyOptions(
[property: CliOption("--policy-name")] string PolicyName,
[property: CliOption("--service-namespace")] string ServiceNamespace,
[property: CliOption("--resource-id")] string ResourceId,
[property: CliOption("--scalable-dimension")] string ScalableDimension
) : AwsOptions
{
    [CliOption("--policy-type")]
    public string? PolicyType { get; set; }

    [CliOption("--step-scaling-policy-configuration")]
    public string? StepScalingPolicyConfiguration { get; set; }

    [CliOption("--target-tracking-scaling-policy-configuration")]
    public string? TargetTrackingScalingPolicyConfiguration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}