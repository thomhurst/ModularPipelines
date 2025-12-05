using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("backup", "put-backup-vault-notifications")]
public record AwsBackupPutBackupVaultNotificationsOptions(
[property: CliOption("--backup-vault-name")] string BackupVaultName,
[property: CliOption("--sns-topic-arn")] string SnsTopicArn,
[property: CliOption("--backup-vault-events")] string[] BackupVaultEvents
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}