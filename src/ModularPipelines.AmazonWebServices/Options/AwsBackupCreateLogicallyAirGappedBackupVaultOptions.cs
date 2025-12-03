using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("backup", "create-logically-air-gapped-backup-vault")]
public record AwsBackupCreateLogicallyAirGappedBackupVaultOptions(
[property: CliOption("--backup-vault-name")] string BackupVaultName,
[property: CliOption("--min-retention-days")] long MinRetentionDays,
[property: CliOption("--max-retention-days")] long MaxRetentionDays
) : AwsOptions
{
    [CliOption("--backup-vault-tags")]
    public IEnumerable<KeyValue>? BackupVaultTags { get; set; }

    [CliOption("--creator-request-id")]
    public string? CreatorRequestId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}