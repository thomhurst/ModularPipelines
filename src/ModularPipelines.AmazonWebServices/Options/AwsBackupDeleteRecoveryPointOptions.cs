using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("backup", "delete-recovery-point")]
public record AwsBackupDeleteRecoveryPointOptions(
[property: CliOption("--backup-vault-name")] string BackupVaultName,
[property: CliOption("--recovery-point-arn")] string RecoveryPointArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}