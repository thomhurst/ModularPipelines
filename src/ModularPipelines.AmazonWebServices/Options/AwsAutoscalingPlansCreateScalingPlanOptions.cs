using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("autoscaling-plans", "create-scaling-plan")]
public record AwsAutoscalingPlansCreateScalingPlanOptions(
[property: CliOption("--scaling-plan-name")] string ScalingPlanName,
[property: CliOption("--application-source")] string ApplicationSource,
[property: CliOption("--scaling-instructions")] string[] ScalingInstructions
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}