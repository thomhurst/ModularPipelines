using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("backup", "start-copy-job")]
public record AwsBackupStartCopyJobOptions(
[property: CommandSwitch("--recovery-point-arn")] string RecoveryPointArn,
[property: CommandSwitch("--source-backup-vault-name")] string SourceBackupVaultName,
[property: CommandSwitch("--destination-backup-vault-arn")] string DestinationBackupVaultArn,
[property: CommandSwitch("--iam-role-arn")] string IamRoleArn
) : AwsOptions
{
    [CommandSwitch("--idempotency-token")]
    public string? IdempotencyToken { get; set; }

    [CommandSwitch("--lifecycle")]
    public string? Lifecycle { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}