using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("backup", "delete-backup-vault-access-policy")]
public record AwsBackupDeleteBackupVaultAccessPolicyOptions(
[property: CommandSwitch("--backup-vault-name")] string BackupVaultName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}