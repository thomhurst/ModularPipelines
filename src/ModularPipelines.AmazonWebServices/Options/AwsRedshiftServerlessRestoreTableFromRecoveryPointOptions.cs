using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redshift-serverless", "restore-table-from-recovery-point")]
public record AwsRedshiftServerlessRestoreTableFromRecoveryPointOptions(
[property: CliOption("--namespace-name")] string NamespaceName,
[property: CliOption("--new-table-name")] string NewTableName,
[property: CliOption("--recovery-point-id")] string RecoveryPointId,
[property: CliOption("--source-database-name")] string SourceDatabaseName,
[property: CliOption("--source-table-name")] string SourceTableName,
[property: CliOption("--workgroup-name")] string WorkgroupName
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