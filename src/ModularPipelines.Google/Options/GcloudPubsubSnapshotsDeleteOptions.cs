using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pubsub", "snapshots", "delete")]
public record GcloudPubsubSnapshotsDeleteOptions(
[property: PositionalArgument] string Snapshot
) : GcloudOptions;