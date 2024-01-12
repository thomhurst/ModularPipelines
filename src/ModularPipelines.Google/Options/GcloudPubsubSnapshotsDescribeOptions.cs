using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pubsub", "snapshots", "describe")]
public record GcloudPubsubSnapshotsDescribeOptions(
[property: PositionalArgument] string Snapshot
) : GcloudOptions;