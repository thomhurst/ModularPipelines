using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("autoscaling", "put-lifecycle-hook")]
public record AwsAutoscalingPutLifecycleHookOptions(
[property: CommandSwitch("--lifecycle-hook-name")] string LifecycleHookName,
[property: CommandSwitch("--auto-scaling-group-name")] string AutoScalingGroupName
) : AwsOptions
{
    [CommandSwitch("--lifecycle-transition")]
    public string? LifecycleTransition { get; set; }

    [CommandSwitch("--role-arn")]
    public string? RoleArn { get; set; }

    [CommandSwitch("--notification-target-arn")]
    public string? NotificationTargetArn { get; set; }

    [CommandSwitch("--notification-metadata")]
    public string? NotificationMetadata { get; set; }

    [CommandSwitch("--heartbeat-timeout")]
    public int? HeartbeatTimeout { get; set; }

    [CommandSwitch("--default-result")]
    public string? DefaultResult { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}