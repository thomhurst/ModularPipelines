using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ebs", "put-snapshot-block")]
public record AwsEbsPutSnapshotBlockOptions(
[property: CliOption("--snapshot-id")] string SnapshotId,
[property: CliOption("--block-index")] int BlockIndex,
[property: CliOption("--block-data")] string BlockData,
[property: CliOption("--data-length")] int DataLength,
[property: CliOption("--checksum")] string Checksum,
[property: CliOption("--checksum-algorithm")] string ChecksumAlgorithm
) : AwsOptions
{
    [CliOption("--progress")]
    public int? Progress { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}