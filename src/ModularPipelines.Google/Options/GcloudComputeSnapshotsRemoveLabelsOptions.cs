using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "snapshots", "remove-labels")]
public record GcloudComputeSnapshotsRemoveLabelsOptions(
[property: PositionalArgument] string SnapshotName,
[property: BooleanCommandSwitch("--all")] bool All,
[property: CommandSwitch("--labels")] string[] Labels
) : GcloudOptions;