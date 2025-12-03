using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("autoscaling-plans", "describe-scaling-plan-resources")]
public record AwsAutoscalingPlansDescribeScalingPlanResourcesOptions(
[property: CliOption("--scaling-plan-name")] string ScalingPlanName,
[property: CliOption("--scaling-plan-version")] long ScalingPlanVersion
) : AwsOptions
{
    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}