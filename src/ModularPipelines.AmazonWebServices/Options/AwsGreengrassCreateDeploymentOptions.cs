using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("greengrass", "create-deployment")]
public record AwsGreengrassCreateDeploymentOptions(
[property: CliOption("--deployment-type")] string DeploymentType,
[property: CliOption("--group-id")] string GroupId
) : AwsOptions
{
    [CliOption("--amzn-client-token")]
    public string? AmznClientToken { get; set; }

    [CliOption("--deployment-id")]
    public string? DeploymentId { get; set; }

    [CliOption("--group-version-id")]
    public string? GroupVersionId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}