using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "create-edge-deployment-stage")]
public record AwsSagemakerCreateEdgeDeploymentStageOptions(
[property: CliOption("--edge-deployment-plan-name")] string EdgeDeploymentPlanName,
[property: CliOption("--stages")] string[] Stages
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}