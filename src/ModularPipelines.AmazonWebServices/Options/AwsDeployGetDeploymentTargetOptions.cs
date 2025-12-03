using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("deploy", "get-deployment-target")]
public record AwsDeployGetDeploymentTargetOptions(
[property: CliOption("--deployment-id")] string DeploymentId,
[property: CliOption("--target-id")] string TargetId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}