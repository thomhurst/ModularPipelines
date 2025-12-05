using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "snapshots", "remove-labels")]
public record GcloudComputeSnapshotsRemoveLabelsOptions(
[property: CliArgument] string SnapshotName,
[property: CliFlag("--all")] bool All,
[property: CliOption("--labels")] string[] Labels
) : GcloudOptions;