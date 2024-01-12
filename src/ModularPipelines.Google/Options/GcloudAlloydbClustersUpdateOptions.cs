using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("alloydb", "clusters", "update")]
public record GcloudAlloydbClustersUpdateOptions(
[property: PositionalArgument] string Cluster,
[property: CommandSwitch("--region")] string Region
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [BooleanCommandSwitch("--clear-automated-backup")]
    public bool? ClearAutomatedBackup { get; set; }

    [BooleanCommandSwitch("--disable-automated-backup")]
    public bool? DisableAutomatedBackup { get; set; }

    [CommandSwitch("--automated-backup-days-of-week")]
    public string[]? AutomatedBackupDaysOfWeek { get; set; }

    [CommandSwitch("--automated-backup-start-times")]
    public string[]? AutomatedBackupStartTimes { get; set; }

    [CommandSwitch("--automated-backup-window")]
    public string? AutomatedBackupWindow { get; set; }

    [CommandSwitch("--automated-backup-encryption-key")]
    public string? AutomatedBackupEncryptionKey { get; set; }

    [CommandSwitch("--automated-backup-encryption-key-keyring")]
    public string? AutomatedBackupEncryptionKeyKeyring { get; set; }

    [CommandSwitch("--automated-backup-encryption-key-location")]
    public string? AutomatedBackupEncryptionKeyLocation { get; set; }

    [CommandSwitch("--automated-backup-encryption-key-project")]
    public string? AutomatedBackupEncryptionKeyProject { get; set; }

    [CommandSwitch("--automated-backup-retention-count")]
    public string? AutomatedBackupRetentionCount { get; set; }

    [CommandSwitch("--automated-backup-retention-period")]
    public string? AutomatedBackupRetentionPeriod { get; set; }

    [CommandSwitch("--continuous-backup-recovery-window-days")]
    public string? ContinuousBackupRecoveryWindowDays { get; set; }

    [BooleanCommandSwitch("--enable-continuous-backup")]
    public bool? EnableContinuousBackup { get; set; }

    [BooleanCommandSwitch("--clear-continuous-backup-encryption-key")]
    public bool? ClearContinuousBackupEncryptionKey { get; set; }

    [CommandSwitch("--continuous-backup-encryption-key")]
    public string? ContinuousBackupEncryptionKey { get; set; }

    [CommandSwitch("--continuous-backup-encryption-key-keyring")]
    public string? ContinuousBackupEncryptionKeyKeyring { get; set; }

    [CommandSwitch("--continuous-backup-encryption-key-location")]
    public string? ContinuousBackupEncryptionKeyLocation { get; set; }

    [CommandSwitch("--continuous-backup-encryption-key-project")]
    public string? ContinuousBackupEncryptionKeyProject { get; set; }
}