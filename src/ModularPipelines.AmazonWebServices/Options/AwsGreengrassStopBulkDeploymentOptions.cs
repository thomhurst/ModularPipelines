using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("greengrass", "stop-bulk-deployment")]
public record AwsGreengrassStopBulkDeploymentOptions(
[property: CliOption("--bulk-deployment-id")] string BulkDeploymentId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}