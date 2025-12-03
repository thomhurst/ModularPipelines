using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pubsub", "snapshots", "list")]
public record GcloudPubsubSnapshotsListOptions : GcloudOptions;