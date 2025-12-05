using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sql", "instances", "patch")]
public record GcloudSqlInstancesPatchOptions(
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

    [CliOption("--availability-type")]
    public string? AvailabilityType { get; set; }

    [CliFlag("--clear-password-policy")]
    public bool? ClearPasswordPolicy { get; set; }

    [CliOption("--connector-enforcement")]
    public string? ConnectorEnforcement { get; set; }

    [CliOption("--cpu")]
    public string? Cpu { get; set; }

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

    [CliFlag("--diff")]
    public bool? Diff { get; set; }

    [CliOption("--edition")]
    public string? Edition { get; set; }

    [CliOption("--[no-]enable-bin-log")]
    public string[]? NoEnableBinLog { get; set; }

    [CliFlag("--enable-data-cache")]
    public bool? EnableDataCache { get; set; }

    [CliOption("--[no-]enable-database-replication")]
    public string[]? NoEnableDatabaseReplication { get; set; }

    [CliOption("--[no-]enable-google-private-path")]
    public string[]? NoEnableGooglePrivatePath { get; set; }

    [CliFlag("--enable-password-policy")]
    public bool? EnablePasswordPolicy { get; set; }

    [CliFlag("--enable-point-in-time-recovery")]
    public bool? EnablePointInTimeRecovery { get; set; }

    [CliOption("--follow-gae-app")]
    public string? FollowGaeApp { get; set; }

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

    [CliOption("--maintenance-version")]
    public string? MaintenanceVersion { get; set; }

    [CliFlag("--maintenance-window-any")]
    public bool? MaintenanceWindowAny { get; set; }

    [CliOption("--maintenance-window-day")]
    public string? MaintenanceWindowDay { get; set; }

    [CliOption("--maintenance-window-hour")]
    public string? MaintenanceWindowHour { get; set; }

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

    [CliOption("--pricing-plan")]
    public string? PricingPlan { get; set; }

    [CliOption("--[no-]recreate-replicas-on-primary-crash")]
    public string[]? NoRecreateReplicasOnPrimaryCrash { get; set; }

    [CliFlag("--remove-deny-maintenance-period")]
    public bool? RemoveDenyMaintenancePeriod { get; set; }

    [CliOption("--replication")]
    public string? Replication { get; set; }

    [CliOption("--[no-]require-ssl")]
    public string[]? NoRequireSsl { get; set; }

    [CliOption("--ssl-mode")]
    public string? SslMode { get; set; }

    [CliOption("--[no-]storage-auto-increase")]
    public string[]? NoStorageAutoIncrease { get; set; }

    [CliOption("--storage-size")]
    public string? StorageSize { get; set; }

    [CliOption("--threads-per-core")]
    public string? ThreadsPerCore { get; set; }

    [CliOption("--tier")]
    public string? Tier { get; set; }

    [CliFlag("--upgrade-sql-network-architecture")]
    public bool? UpgradeSqlNetworkArchitecture { get; set; }

    [CliOption("--allowed-psc-projects")]
    public string[]? AllowedPscProjects { get; set; }

    [CliFlag("--clear-allowed-psc-projects")]
    public bool? ClearAllowedPscProjects { get; set; }

    [CliOption("--authorized-gae-apps")]
    public string[]? AuthorizedGaeApps { get; set; }

    [CliFlag("--clear-gae-apps")]
    public bool? ClearGaeApps { get; set; }

    [CliOption("--authorized-networks")]
    public string[]? AuthorizedNetworks { get; set; }

    [CliFlag("--clear-authorized-networks")]
    public bool? ClearAuthorizedNetworks { get; set; }

    [CliFlag("--clear-database-flags")]
    public bool? ClearDatabaseFlags { get; set; }

    [CliOption("--database-flags")]
    public string[]? DatabaseFlags { get; set; }

    [CliOption("--gce-zone")]
    public string? GceZone { get; set; }

    [CliOption("--secondary-zone")]
    public string? SecondaryZone { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }

    [CliFlag("--no-backup")]
    public bool? NoBackup { get; set; }

    [CliOption("--backup-location")]
    public string? BackupLocation { get; set; }

    [CliOption("--backup-start-time")]
    public string? BackupStartTime { get; set; }

    [CliOption("--retained-backups-count")]
    public string? RetainedBackupsCount { get; set; }

    [CliOption("--retained-transaction-log-days")]
    public string? RetainedTransactionLogDays { get; set; }
}