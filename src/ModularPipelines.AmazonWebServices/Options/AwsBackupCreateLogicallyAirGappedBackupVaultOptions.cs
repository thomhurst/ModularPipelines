using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("backup", "create-logically-air-gapped-backup-vault")]
public record AwsBackupCreateLogicallyAirGappedBackupVaultOptions(
[property: CommandSwitch("--backup-vault-name")] string BackupVaultName,
[property: CommandSwitch("--min-retention-days")] long MinRetentionDays,
[property: CommandSwitch("--max-retention-days")] long MaxRetentionDays
) : AwsOptions
{
    [CommandSwitch("--backup-vault-tags")]
    public IEnumerable<KeyValue>? BackupVaultTags { get; set; }

    [CommandSwitch("--creator-request-id")]
    public string? CreatorRequestId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}