using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("keyspaces", "restore-table")]
public record AwsKeyspacesRestoreTableOptions(
[property: CliOption("--source-keyspace-name")] string SourceKeyspaceName,
[property: CliOption("--source-table-name")] string SourceTableName,
[property: CliOption("--target-keyspace-name")] string TargetKeyspaceName,
[property: CliOption("--target-table-name")] string TargetTableName
) : AwsOptions
{
    [CliOption("--restore-timestamp")]
    public long? RestoreTimestamp { get; set; }

    [CliOption("--capacity-specification-override")]
    public string? CapacitySpecificationOverride { get; set; }

    [CliOption("--encryption-specification-override")]
    public string? EncryptionSpecificationOverride { get; set; }

    [CliOption("--point-in-time-recovery-override")]
    public string? PointInTimeRecoveryOverride { get; set; }

    [CliOption("--tags-override")]
    public string[]? TagsOverride { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}