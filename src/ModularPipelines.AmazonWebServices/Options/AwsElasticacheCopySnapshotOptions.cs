using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elasticache", "copy-snapshot")]
public record AwsElasticacheCopySnapshotOptions(
[property: CliOption("--source-snapshot-name")] string SourceSnapshotName,
[property: CliOption("--target-snapshot-name")] string TargetSnapshotName
) : AwsOptions
{
    [CliOption("--target-bucket")]
    public string? TargetBucket { get; set; }

    [CliOption("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}