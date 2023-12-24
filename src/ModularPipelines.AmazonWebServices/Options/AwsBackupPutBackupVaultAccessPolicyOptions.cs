using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("backup", "put-backup-vault-access-policy")]
public record AwsBackupPutBackupVaultAccessPolicyOptions(
[property: CommandSwitch("--backup-vault-name")] string BackupVaultName
) : AwsOptions
{
    [CommandSwitch("--policy")]
    public string? Policy { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}