using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("deploy", "delete-deployment-group")]
public record AwsDeployDeleteDeploymentGroupOptions(
[property: CliOption("--application-name")] string ApplicationName,
[property: CliOption("--deployment-group-name")] string DeploymentGroupName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}