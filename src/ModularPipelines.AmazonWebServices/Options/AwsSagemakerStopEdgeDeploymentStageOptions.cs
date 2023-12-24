using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "stop-edge-deployment-stage")]
public record AwsSagemakerStopEdgeDeploymentStageOptions(
[property: CommandSwitch("--edge-deployment-plan-name")] string EdgeDeploymentPlanName,
[property: CommandSwitch("--stage-name")] string StageName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}