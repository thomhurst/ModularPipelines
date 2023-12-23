using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "delete-edge-deployment-plan")]
public record AwsSagemakerDeleteEdgeDeploymentPlanOptions(
[property: CommandSwitch("--edge-deployment-plan-name")] string EdgeDeploymentPlanName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}