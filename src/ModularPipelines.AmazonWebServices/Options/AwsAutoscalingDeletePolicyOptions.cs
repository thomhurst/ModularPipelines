using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("autoscaling", "delete-policy")]
public record AwsAutoscalingDeletePolicyOptions(
[property: CliOption("--policy-name")] string PolicyName
) : AwsOptions
{
    [CliOption("--auto-scaling-group-name")]
    public string? AutoScalingGroupName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}