using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pubsub", "snapshots", "list")]
public record GcloudPubsubSnapshotsListOptions : GcloudOptions;