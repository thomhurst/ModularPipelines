using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("autoscaling", "batch-delete-scheduled-action")]
public record AwsAutoscalingBatchDeleteScheduledActionOptions(
[property: CommandSwitch("--auto-scaling-group-name")] string AutoScalingGroupName,
[property: CommandSwitch("--scheduled-action-names")] string[] ScheduledActionNames
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}