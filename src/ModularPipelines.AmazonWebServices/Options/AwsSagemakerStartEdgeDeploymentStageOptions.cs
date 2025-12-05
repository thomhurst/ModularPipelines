using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "start-edge-deployment-stage")]
public record AwsSagemakerStartEdgeDeploymentStageOptions(
[property: CliOption("--edge-deployment-plan-name")] string EdgeDeploymentPlanName,
[property: CliOption("--stage-name")] string StageName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}