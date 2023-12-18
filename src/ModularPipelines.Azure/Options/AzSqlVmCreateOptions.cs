using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "vm", "create")]
public record AzSqlVmCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
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

    [CommandSwitch("--full-backup-duration")]
    public string? FullBackupDuration { get; set; }

    [CommandSwitch("--full-backup-frequency")]
    public string? FullBackupFrequency { get; set; }

    [CommandSwitch("--full-backup-start-hour")]
    public string? FullBackupStartHour { get; set; }

    [CommandSwitch("--image-offer")]
    public string? ImageOffer { get; set; }

    [CommandSwitch("--image-sku")]
    public string? ImageSku { get; set; }

    [CommandSwitch("--key-vault")]
    public string? KeyVault { get; set; }

    [CommandSwitch("--least-privilege-mode")]
    public string? LeastPrivilegeMode { get; set; }

    [CommandSwitch("--license-type")]
    public string? LicenseType { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--log-backup-frequency")]
    public string? LogBackupFrequency { get; set; }

    [CommandSwitch("--maintenance-window-duration")]
    public string? MaintenanceWindowDuration { get; set; }

    [CommandSwitch("--maintenance-window-start-hour")]
    public string? MaintenanceWindowStartHour { get; set; }

    [CommandSwitch("--port")]
    public int? Port { get; set; }

    [CommandSwitch("--retention-period")]
    public string? RetentionPeriod { get; set; }

    [CommandSwitch("--sa-key")]
    public string? SaKey { get; set; }

    [CommandSwitch("--sp-name")]
    public string? SpName { get; set; }

    [CommandSwitch("--sp-secret")]
    public string? SpSecret { get; set; }

    [CommandSwitch("--sql-auth-update-pwd")]
    public string? SqlAuthUpdatePwd { get; set; }

    [CommandSwitch("--sql-auth-update-username")]
    public string? SqlAuthUpdateUsername { get; set; }

    [CommandSwitch("--sql-mgmt-type")]
    public string? SqlMgmtType { get; set; }

    [CommandSwitch("--sql-workload-type")]
    public string? SqlWorkloadType { get; set; }

    [CommandSwitch("--storage-account")]
    public int? StorageAccount { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}