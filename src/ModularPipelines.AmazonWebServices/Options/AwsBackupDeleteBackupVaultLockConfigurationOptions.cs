using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("backup", "delete-backup-vault-lock-configuration")]
public record AwsBackupDeleteBackupVaultLockConfigurationOptions(
[property: CommandSwitch("--backup-vault-name")] string BackupVaultName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}