using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("backup", "create-backup-vault")]
public record AwsBackupCreateBackupVaultOptions(
[property: CommandSwitch("--backup-vault-name")] string BackupVaultName
) : AwsOptions
{
    [CommandSwitch("--backup-vault-tags")]
    public IEnumerable<KeyValue>? BackupVaultTags { get; set; }

    [CommandSwitch("--encryption-key-arn")]
    public string? EncryptionKeyArn { get; set; }

    [CommandSwitch("--creator-request-id")]
    public string? CreatorRequestId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}