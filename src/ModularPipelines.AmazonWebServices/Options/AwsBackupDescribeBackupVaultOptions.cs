using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("backup", "describe-backup-vault")]
public record AwsBackupDescribeBackupVaultOptions(
[property: CliOption("--backup-vault-name")] string BackupVaultName
) : AwsOptions
{
    [CliOption("--backup-vault-account-id")]
    public string? BackupVaultAccountId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}