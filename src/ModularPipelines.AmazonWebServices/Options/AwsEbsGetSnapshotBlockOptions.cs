using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ebs", "get-snapshot-block")]
public record AwsEbsGetSnapshotBlockOptions(
[property: CliOption("--snapshot-id")] string SnapshotId,
[property: CliOption("--block-index")] int BlockIndex,
[property: CliOption("--block-token")] string BlockToken
) : AwsOptions;