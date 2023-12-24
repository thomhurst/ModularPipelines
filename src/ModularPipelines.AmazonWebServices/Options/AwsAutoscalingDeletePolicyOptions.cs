using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("autoscaling", "delete-policy")]
public record AwsAutoscalingDeletePolicyOptions(
[property: CommandSwitch("--policy-name")] string PolicyName
) : AwsOptions
{
    [CommandSwitch("--auto-scaling-group-name")]
    public string? AutoScalingGroupName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}