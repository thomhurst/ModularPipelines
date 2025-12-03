using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ebs", "complete-snapshot")]
public record AwsEbsCompleteSnapshotOptions(
[property: CliOption("--snapshot-id")] string SnapshotId,
[property: CliOption("--changed-blocks-count")] int ChangedBlocksCount
) : AwsOptions
{
    [CliOption("--checksum")]
    public string? Checksum { get; set; }

    [CliOption("--checksum-algorithm")]
    public string? ChecksumAlgorithm { get; set; }

    [CliOption("--checksum-aggregation-method")]
    public string? ChecksumAggregationMethod { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}