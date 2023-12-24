using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("autoscaling", "complete-lifecycle-action")]
public record AwsAutoscalingCompleteLifecycleActionOptions(
[property: CommandSwitch("--lifecycle-hook-name")] string LifecycleHookName,
[property: CommandSwitch("--auto-scaling-group-name")] string AutoScalingGroupName,
[property: CommandSwitch("--lifecycle-action-result")] string LifecycleActionResult
) : AwsOptions
{
    [CommandSwitch("--lifecycle-action-token")]
    public string? LifecycleActionToken { get; set; }

    [CommandSwitch("--instance-id")]
    public string? InstanceId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}