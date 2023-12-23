using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker-edge", "get-deployments")]
public record AwsSagemakerEdgeGetDeploymentsOptions(
[property: CommandSwitch("--device-name")] string DeviceName,
[property: CommandSwitch("--device-fleet-name")] string DeviceFleetName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}