using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pubsub", "snapshots", "describe")]
public record GcloudPubsubSnapshotsDescribeOptions(
[property: CliArgument] string Snapshot
) : GcloudOptions;