using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("alloydb", "clusters", "create")]
public record GcloudAlloydbClustersCreateOptions(
[property: PositionalArgument] string Cluster,
[property: CommandSwitch("--password")] string Password,
[property: CommandSwitch("--region")] string Region
) : GcloudOptions
{
    [CommandSwitch("--allocated-ip-range-name")]
    public string? AllocatedIpRangeName { get; set; }

    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--database-version")]
    public string? DatabaseVersion { get; set; }

    [CommandSwitch("--network")]
    public string? Network { get; set; }

    [CommandSwitch("--continuous-backup-recovery-window-days")]
    public string? ContinuousBackupRecoveryWindowDays { get; set; }

    [BooleanCommandSwitch("--enable-continuous-backup")]
    public bool? EnableContinuousBackup { get; set; }

    [CommandSwitch("--continuous-backup-encryption-key")]
    public string? ContinuousBackupEncryptionKey { get; set; }

    [CommandSwitch("--continuous-backup-encryption-key-keyring")]
    public string? ContinuousBackupEncryptionKeyKeyring { get; set; }

    [CommandSwitch("--continuous-backup-encryption-key-location")]
    public string? ContinuousBackupEncryptionKeyLocation { get; set; }

    [CommandSwitch("--continuous-backup-encryption-key-project")]
    public string? ContinuousBackupEncryptionKeyProject { get; set; }

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

    [CommandSwitch("--kms-key")]
    public string? KmsKey { get; set; }

    [CommandSwitch("--kms-keyring")]
    public string? KmsKeyring { get; set; }

    [CommandSwitch("--kms-location")]
    public string? KmsLocation { get; set; }

    [CommandSwitch("--kms-project")]
    public string? KmsProject { get; set; }
}