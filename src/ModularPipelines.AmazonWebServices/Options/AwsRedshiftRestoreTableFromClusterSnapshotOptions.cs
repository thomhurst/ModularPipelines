using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redshift", "restore-table-from-cluster-snapshot")]
public record AwsRedshiftRestoreTableFromClusterSnapshotOptions(
[property: CommandSwitch("--cluster-identifier")] string ClusterIdentifier,
[property: CommandSwitch("--snapshot-identifier")] string SnapshotIdentifier,
[property: CommandSwitch("--source-database-name")] string SourceDatabaseName,
[property: CommandSwitch("--source-table-name")] string SourceTableName,
[property: CommandSwitch("--new-table-name")] string NewTableName
) : AwsOptions
{
    [CommandSwitch("--source-schema-name")]
    public string? SourceSchemaName { get; set; }

    [CommandSwitch("--target-database-name")]
    public string? TargetDatabaseName { get; set; }

    [CommandSwitch("--target-schema-name")]
    public string? TargetSchemaName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}