using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("backup", "update-recovery-point-lifecycle")]
public record AwsBackupUpdateRecoveryPointLifecycleOptions(
[property: CommandSwitch("--backup-vault-name")] string BackupVaultName,
[property: CommandSwitch("--recovery-point-arn")] string RecoveryPointArn
) : AwsOptions
{
    [CommandSwitch("--lifecycle")]
    public string? Lifecycle { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}