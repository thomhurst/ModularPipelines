using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("autoscaling-plans", "update-scaling-plan")]
public record AwsAutoscalingPlansUpdateScalingPlanOptions(
[property: CommandSwitch("--scaling-plan-name")] string ScalingPlanName,
[property: CommandSwitch("--scaling-plan-version")] long ScalingPlanVersion
) : AwsOptions
{
    [CommandSwitch("--application-source")]
    public string? ApplicationSource { get; set; }

    [CommandSwitch("--scaling-instructions")]
    public string[]? ScalingInstructions { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}