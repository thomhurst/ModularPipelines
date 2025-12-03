using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redshift", "restore-table-from-cluster-snapshot")]
public record AwsRedshiftRestoreTableFromClusterSnapshotOptions(
[property: CliOption("--cluster-identifier")] string ClusterIdentifier,
[property: CliOption("--snapshot-identifier")] string SnapshotIdentifier,
[property: CliOption("--source-database-name")] string SourceDatabaseName,
[property: CliOption("--source-table-name")] string SourceTableName,
[property: CliOption("--new-table-name")] string NewTableName
) : AwsOptions
{
    [CliOption("--source-schema-name")]
    public string? SourceSchemaName { get; set; }

    [CliOption("--target-database-name")]
    public string? TargetDatabaseName { get; set; }

    [CliOption("--target-schema-name")]
    public string? TargetSchemaName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}