using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sql", "vm", "create")]
public record AzSqlVmCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
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

    [CliOption("--full-backup-duration")]
    public string? FullBackupDuration { get; set; }

    [CliOption("--full-backup-frequency")]
    public string? FullBackupFrequency { get; set; }

    [CliOption("--full-backup-start-hour")]
    public string? FullBackupStartHour { get; set; }

    [CliOption("--image-offer")]
    public string? ImageOffer { get; set; }

    [CliOption("--image-sku")]
    public string? ImageSku { get; set; }

    [CliOption("--key-vault")]
    public string? KeyVault { get; set; }

    [CliOption("--least-privilege-mode")]
    public string? LeastPrivilegeMode { get; set; }

    [CliOption("--license-type")]
    public string? LicenseType { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--log-backup-frequency")]
    public string? LogBackupFrequency { get; set; }

    [CliOption("--maintenance-window-duration")]
    public string? MaintenanceWindowDuration { get; set; }

    [CliOption("--maintenance-window-start-hour")]
    public string? MaintenanceWindowStartHour { get; set; }

    [CliOption("--port")]
    public int? Port { get; set; }

    [CliOption("--retention-period")]
    public string? RetentionPeriod { get; set; }

    [CliOption("--sa-key")]
    public string? SaKey { get; set; }

    [CliOption("--sp-name")]
    public string? SpName { get; set; }

    [CliOption("--sp-secret")]
    public string? SpSecret { get; set; }

    [CliOption("--sql-auth-update-pwd")]
    public string? SqlAuthUpdatePwd { get; set; }

    [CliOption("--sql-auth-update-username")]
    public string? SqlAuthUpdateUsername { get; set; }

    [CliOption("--sql-workload-type")]
    public string? SqlWorkloadType { get; set; }

    [CliOption("--storage-account")]
    public int? StorageAccount { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}