using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("autoscaling-plans", "delete-scaling-plan")]
public record AwsAutoscalingPlansDeleteScalingPlanOptions(
[property: CommandSwitch("--scaling-plan-name")] string ScalingPlanName,
[property: CommandSwitch("--scaling-plan-version")] long ScalingPlanVersion
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}