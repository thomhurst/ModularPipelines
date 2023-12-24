using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "create-edge-deployment-stage")]
public record AwsSagemakerCreateEdgeDeploymentStageOptions(
[property: CommandSwitch("--edge-deployment-plan-name")] string EdgeDeploymentPlanName,
[property: CommandSwitch("--stages")] string[] Stages
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}