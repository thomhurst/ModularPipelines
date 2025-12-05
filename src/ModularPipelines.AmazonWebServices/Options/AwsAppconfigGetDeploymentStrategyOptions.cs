using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appconfig", "get-deployment-strategy")]
public record AwsAppconfigGetDeploymentStrategyOptions(
[property: CliOption("--deployment-strategy-id")] string DeploymentStrategyId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}