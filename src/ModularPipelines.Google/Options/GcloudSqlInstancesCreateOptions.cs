using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sql", "instances", "create")]
public record GcloudSqlInstancesCreateOptions(
[property: CliArgument] string Instance
) : GcloudOptions
{
    [CliOption("--activation-policy")]
    public string? ActivationPolicy { get; set; }

    [CliOption("--active-directory-domain")]
    public string? ActiveDirectoryDomain { get; set; }

    [CliOption("--[no-]assign-ip")]
    public string[]? NoAssignIp { get; set; }

    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--audit-bucket-path")]
    public string? AuditBucketPath { get; set; }

    [CliOption("--audit-retention-interval")]
    public string? AuditRetentionInterval { get; set; }

    [CliOption("--audit-upload-interval")]
    public string? AuditUploadInterval { get; set; }

    [CliOption("--authorized-networks")]
    public string[]? AuthorizedNetworks { get; set; }

    [CliOption("--availability-type")]
    public string? AvailabilityType { get; set; }

    [CliFlag("--backup")]
    public bool? Backup { get; set; }

    [CliOption("--backup-location")]
    public string? BackupLocation { get; set; }

    [CliOption("--backup-start-time")]
    public string? BackupStartTime { get; set; }

    [CliFlag("--cascadable-replica")]
    public bool? CascadableReplica { get; set; }

    [CliOption("--collation")]
    public string? Collation { get; set; }

    [CliOption("--connector-enforcement")]
    public string? ConnectorEnforcement { get; set; }

    [CliOption("--cpu")]
    public string? Cpu { get; set; }

    [CliOption("--database-flags")]
    public string[]? DatabaseFlags { get; set; }

    [CliOption("--database-version")]
    public string? DatabaseVersion { get; set; }

    [CliOption("--[no-]deletion-protection")]
    public string[]? NoDeletionProtection { get; set; }

    [CliOption("--deny-maintenance-period-end-date")]
    public string? DenyMaintenancePeriodEndDate { get; set; }

    [CliOption("--deny-maintenance-period-start-date")]
    public string? DenyMaintenancePeriodStartDate { get; set; }

    [CliOption("--deny-maintenance-period-time")]
    public string? DenyMaintenancePeriodTime { get; set; }

    [CliOption("--edition")]
    public string? Edition { get; set; }

    [CliFlag("--enable-bin-log")]
    public bool? EnableBinLog { get; set; }

    [CliFlag("--enable-data-cache")]
    public bool? EnableDataCache { get; set; }

    [CliFlag("--enable-google-private-path")]
    public bool? EnableGooglePrivatePath { get; set; }

    [CliFlag("--enable-password-policy")]
    public bool? EnablePasswordPolicy { get; set; }

    [CliFlag("--enable-point-in-time-recovery")]
    public bool? EnablePointInTimeRecovery { get; set; }

    [CliOption("--failover-replica-name")]
    public string? FailoverReplicaName { get; set; }

    [CliOption("--[no-]insights-config-query-insights-enabled")]
    public string[]? NoInsightsConfigQueryInsightsEnabled { get; set; }

    [CliOption("--insights-config-query-plans-per-minute")]
    public string? InsightsConfigQueryPlansPerMinute { get; set; }

    [CliOption("--insights-config-query-string-length")]
    public string? InsightsConfigQueryStringLength { get; set; }

    [CliOption("--[no-]insights-config-record-application-tags")]
    public string[]? NoInsightsConfigRecordApplicationTags { get; set; }

    [CliOption("--[no-]insights-config-record-client-address")]
    public string[]? NoInsightsConfigRecordClientAddress { get; set; }

    [CliOption("--maintenance-release-channel")]
    public string? MaintenanceReleaseChannel { get; set; }

    [CliOption("--maintenance-window-day")]
    public string? MaintenanceWindowDay { get; set; }

    [CliOption("--maintenance-window-hour")]
    public string? MaintenanceWindowHour { get; set; }

    [CliOption("--master-instance-name")]
    public string? MasterInstanceName { get; set; }

    [CliOption("--memory")]
    public string? Memory { get; set; }

    [CliOption("--network")]
    public string? Network { get; set; }

    [CliOption("--password-policy-complexity")]
    public string? PasswordPolicyComplexity { get; set; }

    [CliOption("--[no-]password-policy-disallow-username-substring")]
    public string[]? NoPasswordPolicyDisallowUsernameSubstring { get; set; }

    [CliOption("--password-policy-min-length")]
    public string? PasswordPolicyMinLength { get; set; }

    [CliOption("--password-policy-password-change-interval")]
    public string? PasswordPolicyPasswordChangeInterval { get; set; }

    [CliOption("--password-policy-reuse-interval")]
    public string? PasswordPolicyReuseInterval { get; set; }

    [CliOption("--[no-]recreate-replicas-on-primary-crash")]
    public string[]? NoRecreateReplicasOnPrimaryCrash { get; set; }

    [CliOption("--replica-type")]
    public string? ReplicaType { get; set; }

    [CliOption("--replication")]
    public string? Replication { get; set; }

    [CliFlag("--require-ssl")]
    public bool? RequireSsl { get; set; }

    [CliOption("--retained-backups-count")]
    public string? RetainedBackupsCount { get; set; }

    [CliOption("--retained-transaction-log-days")]
    public string? RetainedTransactionLogDays { get; set; }

    [CliOption("--root-password")]
    public string? RootPassword { get; set; }

    [CliOption("--ssl-mode")]
    public string? SslMode { get; set; }

    [CliOption("--[no-]storage-auto-increase")]
    public string[]? NoStorageAutoIncrease { get; set; }

    [CliOption("--storage-size")]
    public string? StorageSize { get; set; }

    [CliOption("--storage-type")]
    public string? StorageType { get; set; }

    [CliOption("--threads-per-core")]
    public string? ThreadsPerCore { get; set; }

    [CliOption("--tier")]
    public string? Tier { get; set; }

    [CliOption("--time-zone")]
    public string? TimeZone { get; set; }

    [CliOption("--timeout")]
    public string? Timeout { get; set; }

    [CliOption("--allowed-psc-projects")]
    public string[]? AllowedPscProjects { get; set; }

    [CliFlag("--enable-private-service-connect")]
    public bool? EnablePrivateServiceConnect { get; set; }

    [CliOption("--disk-encryption-key")]
    public string? DiskEncryptionKey { get; set; }

    [CliOption("--disk-encryption-key-keyring")]
    public string? DiskEncryptionKeyKeyring { get; set; }

    [CliOption("--disk-encryption-key-location")]
    public string? DiskEncryptionKeyLocation { get; set; }

    [CliOption("--disk-encryption-key-project")]
    public string? DiskEncryptionKeyProject { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }

    [CliOption("--gce-zone")]
    public string? GceZone { get; set; }

    [CliOption("--secondary-zone")]
    public string? SecondaryZone { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }
}