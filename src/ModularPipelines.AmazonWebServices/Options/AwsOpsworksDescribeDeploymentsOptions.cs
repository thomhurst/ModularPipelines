using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("opsworks", "describe-deployments")]
public record AwsOpsworksDescribeDeploymentsOptions : AwsOptions
{
    [CommandSwitch("--stack-id")]
    public string? StackId { get; set; }

    [CommandSwitch("--app-id")]
    public string? AppId { get; set; }

    [CommandSwitch("--deployment-ids")]
    public string[]? DeploymentIds { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}