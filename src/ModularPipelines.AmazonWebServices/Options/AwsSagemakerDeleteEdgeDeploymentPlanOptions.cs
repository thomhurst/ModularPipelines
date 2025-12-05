using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "delete-edge-deployment-plan")]
public record AwsSagemakerDeleteEdgeDeploymentPlanOptions(
[property: CliOption("--edge-deployment-plan-name")] string EdgeDeploymentPlanName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}