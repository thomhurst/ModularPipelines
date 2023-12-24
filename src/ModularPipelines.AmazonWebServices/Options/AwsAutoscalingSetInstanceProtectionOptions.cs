using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("autoscaling", "set-instance-protection")]
public record AwsAutoscalingSetInstanceProtectionOptions(
[property: CommandSwitch("--instance-ids")] string[] InstanceIds,
[property: CommandSwitch("--auto-scaling-group-name")] string AutoScalingGroupName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}