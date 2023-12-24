using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appconfig", "get-deployment-strategy")]
public record AwsAppconfigGetDeploymentStrategyOptions(
[property: CommandSwitch("--deployment-strategy-id")] string DeploymentStrategyId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}