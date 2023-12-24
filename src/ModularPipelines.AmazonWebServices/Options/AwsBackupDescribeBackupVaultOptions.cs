using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("backup", "describe-backup-vault")]
public record AwsBackupDescribeBackupVaultOptions(
[property: CommandSwitch("--backup-vault-name")] string BackupVaultName
) : AwsOptions
{
    [CommandSwitch("--backup-vault-account-id")]
    public string? BackupVaultAccountId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}