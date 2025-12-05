using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("backup", "start-restore-job")]
public record AwsBackupStartRestoreJobOptions(
[property: CliOption("--recovery-point-arn")] string RecoveryPointArn,
[property: CliOption("--metadata")] IEnumerable<KeyValue> Metadata
) : AwsOptions
{
    [CliOption("--iam-role-arn")]
    public string? IamRoleArn { get; set; }

    [CliOption("--idempotency-token")]
    public string? IdempotencyToken { get; set; }

    [CliOption("--resource-type")]
    public string? ResourceType { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}