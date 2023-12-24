using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("backup", "disassociate-recovery-point-from-parent")]
public record AwsBackupDisassociateRecoveryPointFromParentOptions(
[property: CommandSwitch("--backup-vault-name")] string BackupVaultName,
[property: CommandSwitch("--recovery-point-arn")] string RecoveryPointArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}