using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("deploy", "continue-deployment")]
public record AwsDeployContinueDeploymentOptions : AwsOptions
{
    [CliOption("--deployment-id")]
    public string? DeploymentId { get; set; }

    [CliOption("--deployment-wait-type")]
    public string? DeploymentWaitType { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}