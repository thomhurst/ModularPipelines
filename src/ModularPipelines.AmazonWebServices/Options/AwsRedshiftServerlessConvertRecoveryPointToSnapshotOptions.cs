using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redshift-serverless", "convert-recovery-point-to-snapshot")]
public record AwsRedshiftServerlessConvertRecoveryPointToSnapshotOptions(
[property: CommandSwitch("--recovery-point-id")] string RecoveryPointId,
[property: CommandSwitch("--snapshot-name")] string SnapshotName
) : AwsOptions
{
    [CommandSwitch("--retention-period")]
    public int? RetentionPeriod { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}