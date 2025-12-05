using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("greengrass", "reset-deployments")]
public record AwsGreengrassResetDeploymentsOptions(
[property: CliOption("--group-id")] string GroupId
) : AwsOptions
{
    [CliOption("--amzn-client-token")]
    public string? AmznClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}