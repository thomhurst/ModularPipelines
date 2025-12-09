using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("autoscaling-plans", "delete-scaling-plan")]
public record AwsAutoscalingPlansDeleteScalingPlanOptions(
[property: CliOption("--scaling-plan-name")] string ScalingPlanName,
[property: CliOption("--scaling-plan-version")] long ScalingPlanVersion
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}