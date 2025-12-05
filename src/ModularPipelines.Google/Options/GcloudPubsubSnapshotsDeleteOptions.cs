using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pubsub", "snapshots", "delete")]
public record GcloudPubsubSnapshotsDeleteOptions(
[property: CliArgument] string Snapshot
) : GcloudOptions;