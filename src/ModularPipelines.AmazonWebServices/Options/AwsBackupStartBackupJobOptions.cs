using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("backup", "start-backup-job")]
public record AwsBackupStartBackupJobOptions(
[property: CommandSwitch("--backup-vault-name")] string BackupVaultName,
[property: CommandSwitch("--resource-arn")] string ResourceArn,
[property: CommandSwitch("--iam-role-arn")] string IamRoleArn
) : AwsOptions
{
    [CommandSwitch("--idempotency-token")]
    public string? IdempotencyToken { get; set; }

    [CommandSwitch("--start-window-minutes")]
    public long? StartWindowMinutes { get; set; }

    [CommandSwitch("--complete-window-minutes")]
    public long? CompleteWindowMinutes { get; set; }

    [CommandSwitch("--lifecycle")]
    public string? Lifecycle { get; set; }

    [CommandSwitch("--recovery-point-tags")]
    public IEnumerable<KeyValue>? RecoveryPointTags { get; set; }

    [CommandSwitch("--backup-options")]
    public IEnumerable<KeyValue>? BackupOptions { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}