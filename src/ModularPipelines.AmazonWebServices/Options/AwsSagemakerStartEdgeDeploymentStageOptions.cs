using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "start-edge-deployment-stage")]
public record AwsSagemakerStartEdgeDeploymentStageOptions(
[property: CommandSwitch("--edge-deployment-plan-name")] string EdgeDeploymentPlanName,
[property: CommandSwitch("--stage-name")] string StageName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}