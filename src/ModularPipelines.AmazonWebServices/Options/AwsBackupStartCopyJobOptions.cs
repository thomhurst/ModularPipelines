using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("backup", "start-copy-job")]
public record AwsBackupStartCopyJobOptions(
[property: CliOption("--recovery-point-arn")] string RecoveryPointArn,
[property: CliOption("--source-backup-vault-name")] string SourceBackupVaultName,
[property: CliOption("--destination-backup-vault-arn")] string DestinationBackupVaultArn,
[property: CliOption("--iam-role-arn")] string IamRoleArn
) : AwsOptions
{
    [CliOption("--idempotency-token")]
    public string? IdempotencyToken { get; set; }

    [CliOption("--lifecycle")]
    public string? Lifecycle { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}