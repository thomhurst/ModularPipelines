using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redshift", "modify-snapshot-copy-retention-period")]
public record AwsRedshiftModifySnapshotCopyRetentionPeriodOptions(
[property: CommandSwitch("--cluster-identifier")] string ClusterIdentifier,
[property: CommandSwitch("--retention-period")] int RetentionPeriod
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}