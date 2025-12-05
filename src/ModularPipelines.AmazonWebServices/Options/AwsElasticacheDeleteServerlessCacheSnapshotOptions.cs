using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elasticache", "delete-serverless-cache-snapshot")]
public record AwsElasticacheDeleteServerlessCacheSnapshotOptions(
[property: CliOption("--serverless-cache-snapshot-name")] string ServerlessCacheSnapshotName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}