using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "snapshots", "add-labels")]
public record GcloudComputeSnapshotsAddLabelsOptions(
[property: CliArgument] string SnapshotName,
[property: CliOption("--labels")] IEnumerable<KeyValue> Labels
) : GcloudOptions;