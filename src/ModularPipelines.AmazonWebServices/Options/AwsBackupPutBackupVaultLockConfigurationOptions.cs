using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("backup", "put-backup-vault-lock-configuration")]
public record AwsBackupPutBackupVaultLockConfigurationOptions(
[property: CommandSwitch("--backup-vault-name")] string BackupVaultName
) : AwsOptions
{
    [CommandSwitch("--min-retention-days")]
    public long? MinRetentionDays { get; set; }

    [CommandSwitch("--max-retention-days")]
    public long? MaxRetentionDays { get; set; }

    [CommandSwitch("--changeable-for-days")]
    public long? ChangeableForDays { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}