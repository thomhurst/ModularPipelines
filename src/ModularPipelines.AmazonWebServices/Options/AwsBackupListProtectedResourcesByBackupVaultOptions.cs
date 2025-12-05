using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("backup", "list-protected-resources-by-backup-vault")]
public record AwsBackupListProtectedResourcesByBackupVaultOptions(
[property: CliOption("--backup-vault-name")] string BackupVaultName
) : AwsOptions
{
    [CliOption("--backup-vault-account-id")]
    public string? BackupVaultAccountId { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}