using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("autoscaling-plans", "create-scaling-plan")]
public record AwsAutoscalingPlansCreateScalingPlanOptions(
[property: CommandSwitch("--scaling-plan-name")] string ScalingPlanName,
[property: CommandSwitch("--application-source")] string ApplicationSource,
[property: CommandSwitch("--scaling-instructions")] string[] ScalingInstructions
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}