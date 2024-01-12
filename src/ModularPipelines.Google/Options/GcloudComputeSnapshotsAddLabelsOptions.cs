using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "snapshots", "add-labels")]
public record GcloudComputeSnapshotsAddLabelsOptions(
[property: PositionalArgument] string SnapshotName,
[property: CommandSwitch("--labels")] IEnumerable<KeyValue> Labels
) : GcloudOptions;