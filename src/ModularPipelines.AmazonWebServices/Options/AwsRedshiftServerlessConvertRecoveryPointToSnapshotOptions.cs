using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redshift-serverless", "convert-recovery-point-to-snapshot")]
public record AwsRedshiftServerlessConvertRecoveryPointToSnapshotOptions(
[property: CliOption("--recovery-point-id")] string RecoveryPointId,
[property: CliOption("--snapshot-name")] string SnapshotName
) : AwsOptions
{
    [CliOption("--retention-period")]
    public int? RetentionPeriod { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}