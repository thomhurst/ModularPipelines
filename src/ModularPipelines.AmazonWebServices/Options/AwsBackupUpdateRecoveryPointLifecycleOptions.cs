using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("backup", "update-recovery-point-lifecycle")]
public record AwsBackupUpdateRecoveryPointLifecycleOptions(
[property: CliOption("--backup-vault-name")] string BackupVaultName,
[property: CliOption("--recovery-point-arn")] string RecoveryPointArn
) : AwsOptions
{
    [CliOption("--lifecycle")]
    public string? Lifecycle { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}