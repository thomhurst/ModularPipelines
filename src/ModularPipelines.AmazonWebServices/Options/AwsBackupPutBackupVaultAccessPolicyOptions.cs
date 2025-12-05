using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("backup", "put-backup-vault-access-policy")]
public record AwsBackupPutBackupVaultAccessPolicyOptions(
[property: CliOption("--backup-vault-name")] string BackupVaultName
) : AwsOptions
{
    [CliOption("--policy")]
    public string? Policy { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}