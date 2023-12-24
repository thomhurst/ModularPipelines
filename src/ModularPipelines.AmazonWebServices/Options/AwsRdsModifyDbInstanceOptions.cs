using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rds", "modify-db-instance")]
public record AwsRdsModifyDbInstanceOptions(
[property: CommandSwitch("--db-instance-identifier")] string DbInstanceIdentifier
) : AwsOptions
{
    [CommandSwitch("--allocated-storage")]
    public int? AllocatedStorage { get; set; }

    [CommandSwitch("--db-instance-class")]
    public string? DbInstanceClass { get; set; }

    [CommandSwitch("--db-subnet-group-name")]
    public string? DbSubnetGroupName { get; set; }

    [CommandSwitch("--db-security-groups")]
    public string[]? DbSecurityGroups { get; set; }

    [CommandSwitch("--vpc-security-group-ids")]
    public string[]? VpcSecurityGroupIds { get; set; }

    [CommandSwitch("--master-user-password")]
    public string? MasterUserPassword { get; set; }

    [CommandSwitch("--db-parameter-group-name")]
    public string? DbParameterGroupName { get; set; }

    [CommandSwitch("--backup-retention-period")]
    public int? BackupRetentionPeriod { get; set; }

    [CommandSwitch("--preferred-backup-window")]
    public string? PreferredBackupWindow { get; set; }

    [CommandSwitch("--preferred-maintenance-window")]
    public string? PreferredMaintenanceWindow { get; set; }

    [CommandSwitch("--engine-version")]
    public string? EngineVersion { get; set; }

    [CommandSwitch("--license-model")]
    public string? LicenseModel { get; set; }

    [CommandSwitch("--iops")]
    public int? Iops { get; set; }

    [CommandSwitch("--option-group-name")]
    public string? OptionGroupName { get; set; }

    [CommandSwitch("--new-db-instance-identifier")]
    public string? NewDbInstanceIdentifier { get; set; }

    [CommandSwitch("--storage-type")]
    public string? StorageType { get; set; }

    [CommandSwitch("--tde-credential-arn")]
    public string? TdeCredentialArn { get; set; }

    [CommandSwitch("--tde-credential-password")]
    public string? TdeCredentialPassword { get; set; }

    [CommandSwitch("--ca-certificate-identifier")]
    public string? CaCertificateIdentifier { get; set; }

    [CommandSwitch("--domain")]
    public string? Domain { get; set; }

    [CommandSwitch("--domain-fqdn")]
    public string? DomainFqdn { get; set; }

    [CommandSwitch("--domain-ou")]
    public string? DomainOu { get; set; }

    [CommandSwitch("--domain-auth-secret-arn")]
    public string? DomainAuthSecretArn { get; set; }

    [CommandSwitch("--domain-dns-ips")]
    public string[]? DomainDnsIps { get; set; }

    [CommandSwitch("--monitoring-interval")]
    public int? MonitoringInterval { get; set; }

    [CommandSwitch("--db-port-number")]
    public int? DbPortNumber { get; set; }

    [CommandSwitch("--monitoring-role-arn")]
    public string? MonitoringRoleArn { get; set; }

    [CommandSwitch("--domain-iam-role-name")]
    public string? DomainIamRoleName { get; set; }

    [CommandSwitch("--promotion-tier")]
    public int? PromotionTier { get; set; }

    [CommandSwitch("--performance-insights-kms-key-id")]
    public string? PerformanceInsightsKmsKeyId { get; set; }

    [CommandSwitch("--performance-insights-retention-period")]
    public int? PerformanceInsightsRetentionPeriod { get; set; }

    [CommandSwitch("--cloudwatch-logs-export-configuration")]
    public string? CloudwatchLogsExportConfiguration { get; set; }

    [CommandSwitch("--processor-features")]
    public string[]? ProcessorFeatures { get; set; }

    [CommandSwitch("--max-allocated-storage")]
    public int? MaxAllocatedStorage { get; set; }

    [CommandSwitch("--replica-mode")]
    public string? ReplicaMode { get; set; }

    [CommandSwitch("--aws-backup-recovery-point-arn")]
    public string? AwsBackupRecoveryPointArn { get; set; }

    [CommandSwitch("--automation-mode")]
    public string? AutomationMode { get; set; }

    [CommandSwitch("--resume-full-automation-mode-minutes")]
    public int? ResumeFullAutomationModeMinutes { get; set; }

    [CommandSwitch("--network-type")]
    public string? NetworkType { get; set; }

    [CommandSwitch("--storage-throughput")]
    public int? StorageThroughput { get; set; }

    [CommandSwitch("--master-user-secret-kms-key-id")]
    public string? MasterUserSecretKmsKeyId { get; set; }

    [CommandSwitch("--engine")]
    public string? Engine { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}