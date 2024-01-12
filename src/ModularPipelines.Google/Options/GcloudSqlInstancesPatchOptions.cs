using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "instances", "patch")]
public record GcloudSqlInstancesPatchOptions(
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

    [CommandSwitch("--availability-type")]
    public string? AvailabilityType { get; set; }

    [BooleanCommandSwitch("--clear-password-policy")]
    public bool? ClearPasswordPolicy { get; set; }

    [CommandSwitch("--connector-enforcement")]
    public string? ConnectorEnforcement { get; set; }

    [CommandSwitch("--cpu")]
    public string? Cpu { get; set; }

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

    [BooleanCommandSwitch("--diff")]
    public bool? Diff { get; set; }

    [CommandSwitch("--edition")]
    public string? Edition { get; set; }

    [CommandSwitch("--[no-]enable-bin-log")]
    public string[]? NoEnableBinLog { get; set; }

    [BooleanCommandSwitch("--enable-data-cache")]
    public bool? EnableDataCache { get; set; }

    [CommandSwitch("--[no-]enable-database-replication")]
    public string[]? NoEnableDatabaseReplication { get; set; }

    [CommandSwitch("--[no-]enable-google-private-path")]
    public string[]? NoEnableGooglePrivatePath { get; set; }

    [BooleanCommandSwitch("--enable-password-policy")]
    public bool? EnablePasswordPolicy { get; set; }

    [BooleanCommandSwitch("--enable-point-in-time-recovery")]
    public bool? EnablePointInTimeRecovery { get; set; }

    [CommandSwitch("--follow-gae-app")]
    public string? FollowGaeApp { get; set; }

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

    [CommandSwitch("--maintenance-version")]
    public string? MaintenanceVersion { get; set; }

    [BooleanCommandSwitch("--maintenance-window-any")]
    public bool? MaintenanceWindowAny { get; set; }

    [CommandSwitch("--maintenance-window-day")]
    public string? MaintenanceWindowDay { get; set; }

    [CommandSwitch("--maintenance-window-hour")]
    public string? MaintenanceWindowHour { get; set; }

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

    [CommandSwitch("--pricing-plan")]
    public string? PricingPlan { get; set; }

    [CommandSwitch("--[no-]recreate-replicas-on-primary-crash")]
    public string[]? NoRecreateReplicasOnPrimaryCrash { get; set; }

    [BooleanCommandSwitch("--remove-deny-maintenance-period")]
    public bool? RemoveDenyMaintenancePeriod { get; set; }

    [CommandSwitch("--replication")]
    public string? Replication { get; set; }

    [CommandSwitch("--[no-]require-ssl")]
    public string[]? NoRequireSsl { get; set; }

    [CommandSwitch("--ssl-mode")]
    public string? SslMode { get; set; }

    [CommandSwitch("--[no-]storage-auto-increase")]
    public string[]? NoStorageAutoIncrease { get; set; }

    [CommandSwitch("--storage-size")]
    public string? StorageSize { get; set; }

    [CommandSwitch("--threads-per-core")]
    public string? ThreadsPerCore { get; set; }

    [CommandSwitch("--tier")]
    public string? Tier { get; set; }

    [BooleanCommandSwitch("--upgrade-sql-network-architecture")]
    public bool? UpgradeSqlNetworkArchitecture { get; set; }

    [CommandSwitch("--allowed-psc-projects")]
    public string[]? AllowedPscProjects { get; set; }

    [BooleanCommandSwitch("--clear-allowed-psc-projects")]
    public bool? ClearAllowedPscProjects { get; set; }

    [CommandSwitch("--authorized-gae-apps")]
    public string[]? AuthorizedGaeApps { get; set; }

    [BooleanCommandSwitch("--clear-gae-apps")]
    public bool? ClearGaeApps { get; set; }

    [CommandSwitch("--authorized-networks")]
    public string[]? AuthorizedNetworks { get; set; }

    [BooleanCommandSwitch("--clear-authorized-networks")]
    public bool? ClearAuthorizedNetworks { get; set; }

    [BooleanCommandSwitch("--clear-database-flags")]
    public bool? ClearDatabaseFlags { get; set; }

    [CommandSwitch("--database-flags")]
    public string[]? DatabaseFlags { get; set; }

    [CommandSwitch("--gce-zone")]
    public string? GceZone { get; set; }

    [CommandSwitch("--secondary-zone")]
    public string? SecondaryZone { get; set; }

    [CommandSwitch("--zone")]
    public string? Zone { get; set; }

    [BooleanCommandSwitch("--no-backup")]
    public bool? NoBackup { get; set; }

    [CommandSwitch("--backup-location")]
    public string? BackupLocation { get; set; }

    [CommandSwitch("--backup-start-time")]
    public string? BackupStartTime { get; set; }

    [CommandSwitch("--retained-backups-count")]
    public string? RetainedBackupsCount { get; set; }

    [CommandSwitch("--retained-transaction-log-days")]
    public string? RetainedTransactionLogDays { get; set; }
}