using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("docdb", "copy-db-cluster-snapshot")]
public record AwsDocdbCopyDbClusterSnapshotOptions(
[property: CommandSwitch("--source-db-cluster-snapshot-identifier")] string SourceDbClusterSnapshotIdentifier,
[property: CommandSwitch("--target-db-cluster-snapshot-identifier")] string TargetDbClusterSnapshotIdentifier
) : AwsOptions
{
    [CommandSwitch("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CommandSwitch("--pre-signed-url")]
    public string? PreSignedUrl { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--source-region")]
    public string? SourceRegion { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}