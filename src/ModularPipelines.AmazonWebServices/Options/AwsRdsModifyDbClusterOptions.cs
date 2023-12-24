using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rds", "modify-db-cluster")]
public record AwsRdsModifyDbClusterOptions(
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

    [CommandSwitch("--option-group-name")]
    public string? OptionGroupName { get; set; }

    [CommandSwitch("--preferred-backup-window")]
    public string? PreferredBackupWindow { get; set; }

    [CommandSwitch("--preferred-maintenance-window")]
    public string? PreferredMaintenanceWindow { get; set; }

    [CommandSwitch("--backtrack-window")]
    public long? BacktrackWindow { get; set; }

    [CommandSwitch("--cloudwatch-logs-export-configuration")]
    public string? CloudwatchLogsExportConfiguration { get; set; }

    [CommandSwitch("--engine-version")]
    public string? EngineVersion { get; set; }

    [CommandSwitch("--db-instance-parameter-group-name")]
    public string? DbInstanceParameterGroupName { get; set; }

    [CommandSwitch("--domain")]
    public string? Domain { get; set; }

    [CommandSwitch("--domain-iam-role-name")]
    public string? DomainIamRoleName { get; set; }

    [CommandSwitch("--scaling-configuration")]
    public string? ScalingConfiguration { get; set; }

    [CommandSwitch("--db-cluster-instance-class")]
    public string? DbClusterInstanceClass { get; set; }

    [CommandSwitch("--allocated-storage")]
    public int? AllocatedStorage { get; set; }

    [CommandSwitch("--storage-type")]
    public string? StorageType { get; set; }

    [CommandSwitch("--iops")]
    public int? Iops { get; set; }

    [CommandSwitch("--monitoring-interval")]
    public int? MonitoringInterval { get; set; }

    [CommandSwitch("--monitoring-role-arn")]
    public string? MonitoringRoleArn { get; set; }

    [CommandSwitch("--performance-insights-kms-key-id")]
    public string? PerformanceInsightsKmsKeyId { get; set; }

    [CommandSwitch("--performance-insights-retention-period")]
    public int? PerformanceInsightsRetentionPeriod { get; set; }

    [CommandSwitch("--serverless-v2-scaling-configuration")]
    public string? ServerlessV2ScalingConfiguration { get; set; }

    [CommandSwitch("--network-type")]
    public string? NetworkType { get; set; }

    [CommandSwitch("--master-user-secret-kms-key-id")]
    public string? MasterUserSecretKmsKeyId { get; set; }

    [CommandSwitch("--engine-mode")]
    public string? EngineMode { get; set; }

    [CommandSwitch("--aws-backup-recovery-point-arn")]
    public string? AwsBackupRecoveryPointArn { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}