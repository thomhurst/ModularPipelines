using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("deploy", "batch-get-deployment-targets")]
public record AwsDeployBatchGetDeploymentTargetsOptions(
[property: CliOption("--deployment-id")] string DeploymentId,
[property: CliOption("--target-ids")] string[] TargetIds
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}