using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ebs", "complete-snapshot")]
public record AwsEbsCompleteSnapshotOptions(
[property: CommandSwitch("--snapshot-id")] string SnapshotId,
[property: CommandSwitch("--changed-blocks-count")] int ChangedBlocksCount
) : AwsOptions
{
    [CommandSwitch("--checksum")]
    public string? Checksum { get; set; }

    [CommandSwitch("--checksum-algorithm")]
    public string? ChecksumAlgorithm { get; set; }

    [CommandSwitch("--checksum-aggregation-method")]
    public string? ChecksumAggregationMethod { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}