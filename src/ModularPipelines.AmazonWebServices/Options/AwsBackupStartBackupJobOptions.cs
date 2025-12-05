using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("backup", "start-backup-job")]
public record AwsBackupStartBackupJobOptions(
[property: CliOption("--backup-vault-name")] string BackupVaultName,
[property: CliOption("--resource-arn")] string ResourceArn,
[property: CliOption("--iam-role-arn")] string IamRoleArn
) : AwsOptions
{
    [CliOption("--idempotency-token")]
    public string? IdempotencyToken { get; set; }

    [CliOption("--start-window-minutes")]
    public long? StartWindowMinutes { get; set; }

    [CliOption("--complete-window-minutes")]
    public long? CompleteWindowMinutes { get; set; }

    [CliOption("--lifecycle")]
    public string? Lifecycle { get; set; }

    [CliOption("--recovery-point-tags")]
    public IEnumerable<KeyValue>? RecoveryPointTags { get; set; }

    [CliOption("--backup-options")]
    public IEnumerable<KeyValue>? BackupOptions { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}