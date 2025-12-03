using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("deploy", "batch-get-deployment-groups")]
public record AwsDeployBatchGetDeploymentGroupsOptions(
[property: CliOption("--application-name")] string ApplicationName,
[property: CliOption("--deployment-group-names")] string[] DeploymentGroupNames
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}