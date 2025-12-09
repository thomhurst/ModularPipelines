using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("backup", "get-recovery-point-restore-metadata")]
public record AwsBackupGetRecoveryPointRestoreMetadataOptions(
[property: CliOption("--backup-vault-name")] string BackupVaultName,
[property: CliOption("--recovery-point-arn")] string RecoveryPointArn
) : AwsOptions
{
    [CliOption("--backup-vault-account-id")]
    public string? BackupVaultAccountId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}