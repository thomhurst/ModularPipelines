using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("alloydb", "clusters", "create")]
public record GcloudAlloydbClustersCreateOptions(
[property: CliArgument] string Cluster,
[property: CliOption("--password")] string Password,
[property: CliOption("--region")] string Region
) : GcloudOptions
{
    [CliOption("--allocated-ip-range-name")]
    public string? AllocatedIpRangeName { get; set; }

    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--database-version")]
    public string? DatabaseVersion { get; set; }

    [CliOption("--network")]
    public string? Network { get; set; }

    [CliOption("--continuous-backup-recovery-window-days")]
    public string? ContinuousBackupRecoveryWindowDays { get; set; }

    [CliFlag("--enable-continuous-backup")]
    public bool? EnableContinuousBackup { get; set; }

    [CliOption("--continuous-backup-encryption-key")]
    public string? ContinuousBackupEncryptionKey { get; set; }

    [CliOption("--continuous-backup-encryption-key-keyring")]
    public string? ContinuousBackupEncryptionKeyKeyring { get; set; }

    [CliOption("--continuous-backup-encryption-key-location")]
    public string? ContinuousBackupEncryptionKeyLocation { get; set; }

    [CliOption("--continuous-backup-encryption-key-project")]
    public string? ContinuousBackupEncryptionKeyProject { get; set; }

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

    [CliOption("--kms-key")]
    public string? KmsKey { get; set; }

    [CliOption("--kms-keyring")]
    public string? KmsKeyring { get; set; }

    [CliOption("--kms-location")]
    public string? KmsLocation { get; set; }

    [CliOption("--kms-project")]
    public string? KmsProject { get; set; }
}