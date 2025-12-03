using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("autoscaling-plans", "update-scaling-plan")]
public record AwsAutoscalingPlansUpdateScalingPlanOptions(
[property: CliOption("--scaling-plan-name")] string ScalingPlanName,
[property: CliOption("--scaling-plan-version")] long ScalingPlanVersion
) : AwsOptions
{
    [CliOption("--application-source")]
    public string? ApplicationSource { get; set; }

    [CliOption("--scaling-instructions")]
    public string[]? ScalingInstructions { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}