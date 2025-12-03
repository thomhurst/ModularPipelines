using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elasticache", "delete-serverless-cache")]
public record AwsElasticacheDeleteServerlessCacheOptions(
[property: CliOption("--serverless-cache-name")] string ServerlessCacheName
) : AwsOptions
{
    [CliOption("--final-snapshot-name")]
    public string? FinalSnapshotName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}