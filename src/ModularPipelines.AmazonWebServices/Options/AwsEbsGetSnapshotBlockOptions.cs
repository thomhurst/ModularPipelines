using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ebs", "get-snapshot-block")]
public record AwsEbsGetSnapshotBlockOptions(
[property: CommandSwitch("--snapshot-id")] string SnapshotId,
[property: CommandSwitch("--block-index")] int BlockIndex,
[property: CommandSwitch("--block-token")] string BlockToken
) : AwsOptions;