using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "instances", "create")]
public record GcloudSqlInstancesCreateOptions(
[property: PositionalArgument] string Instance
) : GcloudOptions
{
    [CommandSwitch("--activation-policy")]
    public string? ActivationPolicy { get; set; }

    [CommandSwitch("--active-directory-domain")]
    public string? ActiveDirectoryDomain { get; set; }

    [CommandSwitch("--[no-]assign-ip")]
    public string[]? NoAssignIp { get; set; }

    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--audit-bucket-path")]
    public string? AuditBucketPath { get; set; }

    [CommandSwitch("--audit-retention-interval")]
    public string? AuditRetentionInterval { get; set; }

    [CommandSwitch("--audit-upload-interval")]
    public string? AuditUploadInterval { get; set; }

    [CommandSwitch("--authorized-networks")]
    public string[]? AuthorizedNetworks { get; set; }

    [CommandSwitch("--availability-type")]
    public string? AvailabilityType { get; set; }

    [BooleanCommandSwitch("--backup")]
    public bool? Backup { get; set; }

    [CommandSwitch("--backup-location")]
    public string? BackupLocation { get; set; }

    [CommandSwitch("--backup-start-time")]
    public string? BackupStartTime { get; set; }

    [BooleanCommandSwitch("--cascadable-replica")]
    public bool? CascadableReplica { get; set; }

    [CommandSwitch("--collation")]
    public string? Collation { get; set; }

    [CommandSwitch("--connector-enforcement")]
    public string? ConnectorEnforcement { get; set; }

    [CommandSwitch("--cpu")]
    public string? Cpu { get; set; }

    [CommandSwitch("--database-flags")]
    public string[]? DatabaseFlags { get; set; }

    [CommandSwitch("--database-version")]
    public string? DatabaseVersion { get; set; }

    [CommandSwitch("--[no-]deletion-protection")]
    public string[]? NoDeletionProtection { get; set; }

    [CommandSwitch("--deny-maintenance-period-end-date")]
    public string? DenyMaintenancePeriodEndDate { get; set; }

    [CommandSwitch("--deny-maintenance-period-start-date")]
    public string? DenyMaintenancePeriodStartDate { get; set; }

    [CommandSwitch("--deny-maintenance-period-time")]
    public string? DenyMaintenancePeriodTime { get; set; }

    [CommandSwitch("--edition")]
    public string? Edition { get; set; }

    [BooleanCommandSwitch("--enable-bin-log")]
    public bool? EnableBinLog { get; set; }

    [BooleanCommandSwitch("--enable-data-cache")]
    public bool? EnableDataCache { get; set; }

    [BooleanCommandSwitch("--enable-google-private-path")]
    public bool? EnableGooglePrivatePath { get; set; }

    [BooleanCommandSwitch("--enable-password-policy")]
    public bool? EnablePasswordPolicy { get; set; }

    [BooleanCommandSwitch("--enable-point-in-time-recovery")]
    public bool? EnablePointInTimeRecovery { get; set; }

    [CommandSwitch("--failover-replica-name")]
    public string? FailoverReplicaName { get; set; }

    [CommandSwitch("--[no-]insights-config-query-insights-enabled")]
    public string[]? NoInsightsConfigQueryInsightsEnabled { get; set; }

    [CommandSwitch("--insights-config-query-plans-per-minute")]
    public string? InsightsConfigQueryPlansPerMinute { get; set; }

    [CommandSwitch("--insights-config-query-string-length")]
    public string? InsightsConfigQueryStringLength { get; set; }

    [CommandSwitch("--[no-]insights-config-record-application-tags")]
    public string[]? NoInsightsConfigRecordApplicationTags { get; set; }

    [CommandSwitch("--[no-]insights-config-record-client-address")]
    public string[]? NoInsightsConfigRecordClientAddress { get; set; }

    [CommandSwitch("--maintenance-release-channel")]
    public string? MaintenanceReleaseChannel { get; set; }

    [CommandSwitch("--maintenance-window-day")]
    public string? MaintenanceWindowDay { get; set; }

    [CommandSwitch("--maintenance-window-hour")]
    public string? MaintenanceWindowHour { get; set; }

    [CommandSwitch("--master-instance-name")]
    public string? MasterInstanceName { get; set; }

    [CommandSwitch("--memory")]
    public string? Memory { get; set; }

    [CommandSwitch("--network")]
    public string? Network { get; set; }

    [CommandSwitch("--password-policy-complexity")]
    public string? PasswordPolicyComplexity { get; set; }

    [CommandSwitch("--[no-]password-policy-disallow-username-substring")]
    public string[]? NoPasswordPolicyDisallowUsernameSubstring { get; set; }

    [CommandSwitch("--password-policy-min-length")]
    public string? PasswordPolicyMinLength { get; set; }

    [CommandSwitch("--password-policy-password-change-interval")]
    public string? PasswordPolicyPasswordChangeInterval { get; set; }

    [CommandSwitch("--password-policy-reuse-interval")]
    public string? PasswordPolicyReuseInterval { get; set; }

    [CommandSwitch("--[no-]recreate-replicas-on-primary-crash")]
    public string[]? NoRecreateReplicasOnPrimaryCrash { get; set; }

    [CommandSwitch("--replica-type")]
    public string? ReplicaType { get; set; }

    [CommandSwitch("--replication")]
    public string? Replication { get; set; }

    [BooleanCommandSwitch("--require-ssl")]
    public bool? RequireSsl { get; set; }

    [CommandSwitch("--retained-backups-count")]
    public string? RetainedBackupsCount { get; set; }

    [CommandSwitch("--retained-transaction-log-days")]
    public string? RetainedTransactionLogDays { get; set; }

    [CommandSwitch("--root-password")]
    public string? RootPassword { get; set; }

    [CommandSwitch("--ssl-mode")]
    public string? SslMode { get; set; }

    [CommandSwitch("--[no-]storage-auto-increase")]
    public string[]? NoStorageAutoIncrease { get; set; }

    [CommandSwitch("--storage-size")]
    public string? StorageSize { get; set; }

    [CommandSwitch("--storage-type")]
    public string? StorageType { get; set; }

    [CommandSwitch("--threads-per-core")]
    public string? ThreadsPerCore { get; set; }

    [CommandSwitch("--tier")]
    public string? Tier { get; set; }

    [CommandSwitch("--time-zone")]
    public string? TimeZone { get; set; }

    [CommandSwitch("--timeout")]
    public string? Timeout { get; set; }

    [CommandSwitch("--allowed-psc-projects")]
    public string[]? AllowedPscProjects { get; set; }

    [BooleanCommandSwitch("--enable-private-service-connect")]
    public bool? EnablePrivateServiceConnect { get; set; }

    [CommandSwitch("--disk-encryption-key")]
    public string? DiskEncryptionKey { get; set; }

    [CommandSwitch("--disk-encryption-key-keyring")]
    public string? DiskEncryptionKeyKeyring { get; set; }

    [CommandSwitch("--disk-encryption-key-location")]
    public string? DiskEncryptionKeyLocation { get; set; }

    [CommandSwitch("--disk-encryption-key-project")]
    public string? DiskEncryptionKeyProject { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }

    [CommandSwitch("--gce-zone")]
    public string? GceZone { get; set; }

    [CommandSwitch("--secondary-zone")]
    public string? SecondaryZone { get; set; }

    [CommandSwitch("--zone")]
    public string? Zone { get; set; }
}