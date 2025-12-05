using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elasticache", "create-serverless-cache-snapshot")]
public record AwsElasticacheCreateServerlessCacheSnapshotOptions(
[property: CliOption("--serverless-cache-snapshot-name")] string ServerlessCacheSnapshotName,
[property: CliOption("--serverless-cache-name")] string ServerlessCacheName
) : AwsOptions
{
    [CliOption("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}