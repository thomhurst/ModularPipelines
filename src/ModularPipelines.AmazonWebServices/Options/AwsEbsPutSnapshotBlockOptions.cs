using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ebs", "put-snapshot-block")]
public record AwsEbsPutSnapshotBlockOptions(
[property: CommandSwitch("--snapshot-id")] string SnapshotId,
[property: CommandSwitch("--block-index")] int BlockIndex,
[property: CommandSwitch("--block-data")] string BlockData,
[property: CommandSwitch("--data-length")] int DataLength,
[property: CommandSwitch("--checksum")] string Checksum,
[property: CommandSwitch("--checksum-algorithm")] string ChecksumAlgorithm
) : AwsOptions
{
    [CommandSwitch("--progress")]
    public int? Progress { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}