using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("backup", "delete-backup-vault-notifications")]
public record AwsBackupDeleteBackupVaultNotificationsOptions(
[property: CliOption("--backup-vault-name")] string BackupVaultName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}