using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("backup", "disassociate-recovery-point")]
public record AwsBackupDisassociateRecoveryPointOptions(
[property: CliOption("--backup-vault-name")] string BackupVaultName,
[property: CliOption("--recovery-point-arn")] string RecoveryPointArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}