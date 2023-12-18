using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "vm", "update")]
public record AzSqlVmUpdateOptions : AzOptions
{
    [CommandSwitch("--add")]
    public string? Add { get; set; }

    [CommandSwitch("--agent-rg")]
    public string? AgentRg { get; set; }

    [CommandSwitch("--am-day")]
    public string? AmDay { get; set; }

    [CommandSwitch("--am-month-occ")]
    public string? AmMonthOcc { get; set; }

    [BooleanCommandSwitch("--am-schedule")]
    public bool? AmSchedule { get; set; }

    [CommandSwitch("--am-time")]
    public string? AmTime { get; set; }

    [CommandSwitch("--am-week-int")]
    public string? AmWeekInt { get; set; }

    [CommandSwitch("--backup-pwd")]
    public string? BackupPwd { get; set; }

    [CommandSwitch("--backup-schedule-type")]
    public string? BackupScheduleType { get; set; }

    [BooleanCommandSwitch("--backup-system-dbs")]
    public bool? BackupSystemDbs { get; set; }

    [CommandSwitch("--connectivity-type")]
    public string? ConnectivityType { get; set; }

    [CommandSwitch("--credential-name")]
    public string? CredentialName { get; set; }

    [CommandSwitch("--day-of-week")]
    public string? DayOfWeek { get; set; }

    [BooleanCommandSwitch("--enable-assessment")]
    public bool? EnableAssessment { get; set; }

    [BooleanCommandSwitch("--enable-auto-backup")]
    public bool? EnableAutoBackup { get; set; }

    [BooleanCommandSwitch("--enable-auto-patching")]
    public bool? EnableAutoPatching { get; set; }

    [BooleanCommandSwitch("--enable-encryption")]
    public bool? EnableEncryption { get; set; }

    [BooleanCommandSwitch("--enable-key-vault-credential")]
    public bool? EnableKeyVaultCredential { get; set; }

    [BooleanCommandSwitch("--enable-r-services")]
    public bool? EnableRServices { get; set; }

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [CommandSwitch("--full-backup-duration")]
    public string? FullBackupDuration { get; set; }

    [CommandSwitch("--full-backup-frequency")]
    public string? FullBackupFrequency { get; set; }

    [CommandSwitch("--full-backup-start-hour")]
    public string? FullBackupStartHour { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--image-sku")]
    public string? ImageSku { get; set; }

    [CommandSwitch("--key-vault")]
    public string? KeyVault { get; set; }

    [CommandSwitch("--least-privilege-mode")]
    public string? LeastPrivilegeMode { get; set; }

    [CommandSwitch("--license-type")]
    public string? LicenseType { get; set; }

    [CommandSwitch("--log-backup-frequency")]
    public string? LogBackupFrequency { get; set; }

    [CommandSwitch("--maintenance-window-duration")]
    public string? MaintenanceWindowDuration { get; set; }

    [CommandSwitch("--maintenance-window-start-hour")]
    public string? MaintenanceWindowStartHour { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--port")]
    public int? Port { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--retention-period")]
    public string? RetentionPeriod { get; set; }

    [CommandSwitch("--sa-key")]
    public string? SaKey { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [CommandSwitch("--sp-name")]
    public string? SpName { get; set; }

    [CommandSwitch("--sp-secret")]
    public string? SpSecret { get; set; }

    [CommandSwitch("--sql-mgmt-type")]
    public string? SqlMgmtType { get; set; }

    [CommandSwitch("--sql-workload-type")]
    public string? SqlWorkloadType { get; set; }

    [CommandSwitch("--storage-account")]
    public int? StorageAccount { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--workspace-name")]
    public string? WorkspaceName { get; set; }

    [CommandSwitch("--workspace-rg")]
    public string? WorkspaceRg { get; set; }

    [CommandSwitch("--workspace-sub")]
    public string? WorkspaceSub { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}