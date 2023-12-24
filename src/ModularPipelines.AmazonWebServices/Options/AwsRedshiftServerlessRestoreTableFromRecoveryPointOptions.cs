using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redshift-serverless", "restore-table-from-recovery-point")]
public record AwsRedshiftServerlessRestoreTableFromRecoveryPointOptions(
[property: CommandSwitch("--namespace-name")] string NamespaceName,
[property: CommandSwitch("--new-table-name")] string NewTableName,
[property: CommandSwitch("--recovery-point-id")] string RecoveryPointId,
[property: CommandSwitch("--source-database-name")] string SourceDatabaseName,
[property: CommandSwitch("--source-table-name")] string SourceTableName,
[property: CommandSwitch("--workgroup-name")] string WorkgroupName
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