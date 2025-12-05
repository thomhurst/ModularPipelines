using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("docdb", "modify-db-cluster")]
public record AwsDocdbModifyDbClusterOptions(
[property: CliOption("--db-cluster-identifier")] string DbClusterIdentifier
) : AwsOptions
{
    [CliOption("--new-db-cluster-identifier")]
    public string? NewDbClusterIdentifier { get; set; }

    [CliOption("--backup-retention-period")]
    public int? BackupRetentionPeriod { get; set; }

    [CliOption("--db-cluster-parameter-group-name")]
    public string? DbClusterParameterGroupName { get; set; }

    [CliOption("--vpc-security-group-ids")]
    public string[]? VpcSecurityGroupIds { get; set; }

    [CliOption("--port")]
    public int? Port { get; set; }

    [CliOption("--master-user-password")]
    public string? MasterUserPassword { get; set; }

    [CliOption("--preferred-backup-window")]
    public string? PreferredBackupWindow { get; set; }

    [CliOption("--preferred-maintenance-window")]
    public string? PreferredMaintenanceWindow { get; set; }

    [CliOption("--cloudwatch-logs-export-configuration")]
    public string? CloudwatchLogsExportConfiguration { get; set; }

    [CliOption("--engine-version")]
    public string? EngineVersion { get; set; }

    [CliOption("--storage-type")]
    public string? StorageType { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}