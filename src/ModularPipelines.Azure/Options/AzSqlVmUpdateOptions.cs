using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sql", "vm", "update")]
public record AzSqlVmUpdateOptions : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--agent-rg")]
    public string? AgentRg { get; set; }

    [CliOption("--am-day")]
    public string? AmDay { get; set; }

    [CliOption("--am-month-occ")]
    public string? AmMonthOcc { get; set; }

    [CliFlag("--am-schedule")]
    public bool? AmSchedule { get; set; }

    [CliOption("--am-time")]
    public string? AmTime { get; set; }

    [CliOption("--am-week-int")]
    public string? AmWeekInt { get; set; }

    [CliOption("--backup-pwd")]
    public string? BackupPwd { get; set; }

    [CliOption("--backup-schedule-type")]
    public string? BackupScheduleType { get; set; }

    [CliFlag("--backup-system-dbs")]
    public bool? BackupSystemDbs { get; set; }

    [CliOption("--connectivity-type")]
    public string? ConnectivityType { get; set; }

    [CliOption("--credential-name")]
    public string? CredentialName { get; set; }

    [CliOption("--day-of-week")]
    public string? DayOfWeek { get; set; }

    [CliFlag("--enable-assessment")]
    public bool? EnableAssessment { get; set; }

    [CliFlag("--enable-auto-backup")]
    public bool? EnableAutoBackup { get; set; }

    [CliFlag("--enable-auto-patching")]
    public bool? EnableAutoPatching { get; set; }

    [CliFlag("--enable-encryption")]
    public bool? EnableEncryption { get; set; }

    [CliFlag("--enable-key-vault-credential")]
    public bool? EnableKeyVaultCredential { get; set; }

    [CliFlag("--enable-r-services")]
    public bool? EnableRServices { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--full-backup-duration")]
    public string? FullBackupDuration { get; set; }

    [CliOption("--full-backup-frequency")]
    public string? FullBackupFrequency { get; set; }

    [CliOption("--full-backup-start-hour")]
    public string? FullBackupStartHour { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--image-sku")]
    public string? ImageSku { get; set; }

    [CliOption("--key-vault")]
    public string? KeyVault { get; set; }

    [CliOption("--least-privilege-mode")]
    public string? LeastPrivilegeMode { get; set; }

    [CliOption("--license-type")]
    public string? LicenseType { get; set; }

    [CliOption("--log-backup-frequency")]
    public string? LogBackupFrequency { get; set; }

    [CliOption("--maintenance-window-duration")]
    public string? MaintenanceWindowDuration { get; set; }

    [CliOption("--maintenance-window-start-hour")]
    public string? MaintenanceWindowStartHour { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--port")]
    public int? Port { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--retention-period")]
    public string? RetentionPeriod { get; set; }

    [CliOption("--sa-key")]
    public string? SaKey { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--sp-name")]
    public string? SpName { get; set; }

    [CliOption("--sp-secret")]
    public string? SpSecret { get; set; }

    [CliOption("--sql-workload-type")]
    public string? SqlWorkloadType { get; set; }

    [CliOption("--storage-account")]
    public int? StorageAccount { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--workspace-name")]
    public string? WorkspaceName { get; set; }

    [CliOption("--workspace-rg")]
    public string? WorkspaceRg { get; set; }

    [CliOption("--workspace-sub")]
    public string? WorkspaceSub { get; set; }
}