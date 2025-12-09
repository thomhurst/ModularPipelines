using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("greengrass", "get-bulk-deployment-status")]
public record AwsGreengrassGetBulkDeploymentStatusOptions(
[property: CliOption("--bulk-deployment-id")] string BulkDeploymentId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}