using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "create-edge-deployment-plan")]
public record AwsSagemakerCreateEdgeDeploymentPlanOptions(
[property: CliOption("--edge-deployment-plan-name")] string EdgeDeploymentPlanName,
[property: CliOption("--model-configs")] string[] ModelConfigs,
[property: CliOption("--device-fleet-name")] string DeviceFleetName
) : AwsOptions
{
    [CliOption("--stages")]
    public string[]? Stages { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}