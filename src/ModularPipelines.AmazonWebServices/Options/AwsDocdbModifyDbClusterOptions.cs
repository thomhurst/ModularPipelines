using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("docdb", "modify-db-cluster")]
public record AwsDocdbModifyDbClusterOptions(
[property: CommandSwitch("--db-cluster-identifier")] string DbClusterIdentifier
) : AwsOptions
{
    [CommandSwitch("--new-db-cluster-identifier")]
    public string? NewDbClusterIdentifier { get; set; }

    [CommandSwitch("--backup-retention-period")]
    public int? BackupRetentionPeriod { get; set; }

    [CommandSwitch("--db-cluster-parameter-group-name")]
    public string? DbClusterParameterGroupName { get; set; }

    [CommandSwitch("--vpc-security-group-ids")]
    public string[]? VpcSecurityGroupIds { get; set; }

    [CommandSwitch("--port")]
    public int? Port { get; set; }

    [CommandSwitch("--master-user-password")]
    public string? MasterUserPassword { get; set; }

    [CommandSwitch("--preferred-backup-window")]
    public string? PreferredBackupWindow { get; set; }

    [CommandSwitch("--preferred-maintenance-window")]
    public string? PreferredMaintenanceWindow { get; set; }

    [CommandSwitch("--cloudwatch-logs-export-configuration")]
    public string? CloudwatchLogsExportConfiguration { get; set; }

    [CommandSwitch("--engine-version")]
    public string? EngineVersion { get; set; }

    [CommandSwitch("--storage-type")]
    public string? StorageType { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}