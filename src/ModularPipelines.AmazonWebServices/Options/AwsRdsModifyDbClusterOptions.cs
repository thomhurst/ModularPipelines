using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rds", "modify-db-cluster")]
public record AwsRdsModifyDbClusterOptions(
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

    [CliOption("--option-group-name")]
    public string? OptionGroupName { get; set; }

    [CliOption("--preferred-backup-window")]
    public string? PreferredBackupWindow { get; set; }

    [CliOption("--preferred-maintenance-window")]
    public string? PreferredMaintenanceWindow { get; set; }

    [CliOption("--backtrack-window")]
    public long? BacktrackWindow { get; set; }

    [CliOption("--cloudwatch-logs-export-configuration")]
    public string? CloudwatchLogsExportConfiguration { get; set; }

    [CliOption("--engine-version")]
    public string? EngineVersion { get; set; }

    [CliOption("--db-instance-parameter-group-name")]
    public string? DbInstanceParameterGroupName { get; set; }

    [CliOption("--domain")]
    public string? Domain { get; set; }

    [CliOption("--domain-iam-role-name")]
    public string? DomainIamRoleName { get; set; }

    [CliOption("--scaling-configuration")]
    public string? ScalingConfiguration { get; set; }

    [CliOption("--db-cluster-instance-class")]
    public string? DbClusterInstanceClass { get; set; }

    [CliOption("--allocated-storage")]
    public int? AllocatedStorage { get; set; }

    [CliOption("--storage-type")]
    public string? StorageType { get; set; }

    [CliOption("--iops")]
    public int? Iops { get; set; }

    [CliOption("--monitoring-interval")]
    public int? MonitoringInterval { get; set; }

    [CliOption("--monitoring-role-arn")]
    public string? MonitoringRoleArn { get; set; }

    [CliOption("--performance-insights-kms-key-id")]
    public string? PerformanceInsightsKmsKeyId { get; set; }

    [CliOption("--performance-insights-retention-period")]
    public int? PerformanceInsightsRetentionPeriod { get; set; }

    [CliOption("--serverless-v2-scaling-configuration")]
    public string? ServerlessV2ScalingConfiguration { get; set; }

    [CliOption("--network-type")]
    public string? NetworkType { get; set; }

    [CliOption("--master-user-secret-kms-key-id")]
    public string? MasterUserSecretKmsKeyId { get; set; }

    [CliOption("--engine-mode")]
    public string? EngineMode { get; set; }

    [CliOption("--aws-backup-recovery-point-arn")]
    public string? AwsBackupRecoveryPointArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}