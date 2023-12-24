using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redshift-serverless", "update-snapshot-copy-configuration")]
public record AwsRedshiftServerlessUpdateSnapshotCopyConfigurationOptions(
[property: CommandSwitch("--snapshot-copy-configuration-id")] string SnapshotCopyConfigurationId
) : AwsOptions
{
    [CommandSwitch("--snapshot-retention-period")]
    public int? SnapshotRetentionPeriod { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}