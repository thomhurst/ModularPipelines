using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("backup", "put-backup-vault-lock-configuration")]
public record AwsBackupPutBackupVaultLockConfigurationOptions(
[property: CliOption("--backup-vault-name")] string BackupVaultName
) : AwsOptions
{
    [CliOption("--min-retention-days")]
    public long? MinRetentionDays { get; set; }

    [CliOption("--max-retention-days")]
    public long? MaxRetentionDays { get; set; }

    [CliOption("--changeable-for-days")]
    public long? ChangeableForDays { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}