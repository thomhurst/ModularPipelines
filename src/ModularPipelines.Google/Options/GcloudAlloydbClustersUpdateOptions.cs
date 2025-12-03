using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("alloydb", "clusters", "update")]
public record GcloudAlloydbClustersUpdateOptions(
[property: CliArgument] string Cluster,
[property: CliOption("--region")] string Region
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliFlag("--clear-automated-backup")]
    public bool? ClearAutomatedBackup { get; set; }

    [CliFlag("--disable-automated-backup")]
    public bool? DisableAutomatedBackup { get; set; }

    [CliOption("--automated-backup-days-of-week")]
    public string[]? AutomatedBackupDaysOfWeek { get; set; }

    [CliOption("--automated-backup-start-times")]
    public string[]? AutomatedBackupStartTimes { get; set; }

    [CliOption("--automated-backup-window")]
    public string? AutomatedBackupWindow { get; set; }

    [CliOption("--automated-backup-encryption-key")]
    public string? AutomatedBackupEncryptionKey { get; set; }

    [CliOption("--automated-backup-encryption-key-keyring")]
    public string? AutomatedBackupEncryptionKeyKeyring { get; set; }

    [CliOption("--automated-backup-encryption-key-location")]
    public string? AutomatedBackupEncryptionKeyLocation { get; set; }

    [CliOption("--automated-backup-encryption-key-project")]
    public string? AutomatedBackupEncryptionKeyProject { get; set; }

    [CliOption("--automated-backup-retention-count")]
    public string? AutomatedBackupRetentionCount { get; set; }

    [CliOption("--automated-backup-retention-period")]
    public string? AutomatedBackupRetentionPeriod { get; set; }

    [CliOption("--continuous-backup-recovery-window-days")]
    public string? ContinuousBackupRecoveryWindowDays { get; set; }

    [CliFlag("--enable-continuous-backup")]
    public bool? EnableContinuousBackup { get; set; }

    [CliFlag("--clear-continuous-backup-encryption-key")]
    public bool? ClearContinuousBackupEncryptionKey { get; set; }

    [CliOption("--continuous-backup-encryption-key")]
    public string? ContinuousBackupEncryptionKey { get; set; }

    [CliOption("--continuous-backup-encryption-key-keyring")]
    public string? ContinuousBackupEncryptionKeyKeyring { get; set; }

    [CliOption("--continuous-backup-encryption-key-location")]
    public string? ContinuousBackupEncryptionKeyLocation { get; set; }

    [CliOption("--continuous-backup-encryption-key-project")]
    public string? ContinuousBackupEncryptionKeyProject { get; set; }
}