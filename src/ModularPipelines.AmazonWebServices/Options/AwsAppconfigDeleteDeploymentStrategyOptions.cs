using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appconfig", "delete-deployment-strategy")]
public record AwsAppconfigDeleteDeploymentStrategyOptions(
[property: CliOption("--deployment-strategy-id")] string DeploymentStrategyId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}