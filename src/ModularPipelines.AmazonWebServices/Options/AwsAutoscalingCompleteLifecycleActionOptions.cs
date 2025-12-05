using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("autoscaling", "complete-lifecycle-action")]
public record AwsAutoscalingCompleteLifecycleActionOptions(
[property: CliOption("--lifecycle-hook-name")] string LifecycleHookName,
[property: CliOption("--auto-scaling-group-name")] string AutoScalingGroupName,
[property: CliOption("--lifecycle-action-result")] string LifecycleActionResult
) : AwsOptions
{
    [CliOption("--lifecycle-action-token")]
    public string? LifecycleActionToken { get; set; }

    [CliOption("--instance-id")]
    public string? InstanceId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}