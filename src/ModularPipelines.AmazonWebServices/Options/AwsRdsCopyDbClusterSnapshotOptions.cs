using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rds", "copy-db-cluster-snapshot")]
public record AwsRdsCopyDbClusterSnapshotOptions(
[property: CliOption("--source-db-cluster-snapshot-identifier")] string SourceDbClusterSnapshotIdentifier,
[property: CliOption("--target-db-cluster-snapshot-identifier")] string TargetDbClusterSnapshotIdentifier
) : AwsOptions
{
    [CliOption("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CliOption("--pre-signed-url")]
    public string? PreSignedUrl { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--source-region")]
    public string? SourceRegion { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}