using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("autoscaling", "put-lifecycle-hook")]
public record AwsAutoscalingPutLifecycleHookOptions(
[property: CliOption("--lifecycle-hook-name")] string LifecycleHookName,
[property: CliOption("--auto-scaling-group-name")] string AutoScalingGroupName
) : AwsOptions
{
    [CliOption("--lifecycle-transition")]
    public string? LifecycleTransition { get; set; }

    [CliOption("--role-arn")]
    public string? RoleArn { get; set; }

    [CliOption("--notification-target-arn")]
    public string? NotificationTargetArn { get; set; }

    [CliOption("--notification-metadata")]
    public string? NotificationMetadata { get; set; }

    [CliOption("--heartbeat-timeout")]
    public int? HeartbeatTimeout { get; set; }

    [CliOption("--default-result")]
    public string? DefaultResult { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}