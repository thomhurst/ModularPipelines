using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "create-edge-deployment-plan")]
public record AwsSagemakerCreateEdgeDeploymentPlanOptions(
[property: CommandSwitch("--edge-deployment-plan-name")] string EdgeDeploymentPlanName,
[property: CommandSwitch("--model-configs")] string[] ModelConfigs,
[property: CommandSwitch("--device-fleet-name")] string DeviceFleetName
) : AwsOptions
{
    [CommandSwitch("--stages")]
    public string[]? Stages { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}