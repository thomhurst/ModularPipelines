using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("backup", "put-backup-vault-notifications")]
public record AwsBackupPutBackupVaultNotificationsOptions(
[property: CommandSwitch("--backup-vault-name")] string BackupVaultName,
[property: CommandSwitch("--sns-topic-arn")] string SnsTopicArn,
[property: CommandSwitch("--backup-vault-events")] string[] BackupVaultEvents
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}