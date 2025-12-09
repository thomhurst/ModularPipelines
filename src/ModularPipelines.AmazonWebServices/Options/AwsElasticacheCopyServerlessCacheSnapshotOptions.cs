using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elasticache", "copy-serverless-cache-snapshot")]
public record AwsElasticacheCopyServerlessCacheSnapshotOptions(
[property: CliOption("--source-serverless-cache-snapshot-name")] string SourceServerlessCacheSnapshotName,
[property: CliOption("--target-serverless-cache-snapshot-name")] string TargetServerlessCacheSnapshotName
) : AwsOptions
{
    [CliOption("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}